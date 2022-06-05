#region GNU General Public License

/* Copyright 2022 Simon Vonhoff
 *
 * This file is part of ResourcePackerGUI.
 *
 * ResourcePackerGUI is free software: you can redistribute it and/or modify it under the terms of the
 * GNU General Public License as published by the Free Software Foundation, either version 3 of the License,
 * or (at your option) any later version.
 *
 * ResourcePackerGUI is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with ResourcePackerGUI.
 * If not, see <https://www.gnu.org/licenses/>.
 */

#endregion

using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Xml;
using Be.Windows.Forms;
using ResourcePacker.Common;
using ResourcePacker.Entities;
using ResourcePacker.Helpers;
using ResourcePacker.Properties;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.RichTextBoxForms.Themes;
using Winista.Mime;

namespace ResourcePacker.Forms
{
    public partial class MainForm : Form
    {
        private readonly LoggingLevelSwitch _loggingLevelSwitch = new(LogEventLevel.Debug);
        private readonly ActionDebouncer _searchDebouncer;
        private List<Asset>? _assets;
        private PackageHeader _packageHeader;
        private Asset? _selectedPreviewAsset;
        private string _packagePath = string.Empty;
        private string _searchQuery = string.Empty;
        private string _password = string.Empty;
        private bool _formatPreviewText = true;
        private bool _showDebugMessages = true;

        public MainForm()
        {
            InitializeComponent();
            _searchDebouncer = new ActionDebouncer(RefreshFileTree, TimeSpan.FromSeconds(0.35));
        }

        #region Custom methods

        #region Preview methods

        /// <summary>
        /// Prepares the preview tab with the corresponding values.
        /// </summary>
        /// <param name="tabPage">The tab to initialize.</param>
        private void ReloadPreviewTab(TabPage? tabPage)
        {
            // Clear all previews.
            previewHexBox.ByteProvider = null;
            previewImageBox.Image = null;
            previewTextBox.Clear();

            if (_selectedPreviewAsset?.Data == null)
            {
                return;
            }

            // Set status labels.
            lblMediaType.Text = $"Media type: {_selectedPreviewAsset.MimeType?.Name ?? "n/a"}";
            lblDataSize.Text = _selectedPreviewAsset.Entry.DataSize switch
            {
                > 1000000000 => $"Size: {_selectedPreviewAsset.Entry.DataSize / 1000000000} GB",
                > 1000000 => $"Size: {_selectedPreviewAsset.Entry.DataSize / 1000000} MB",
                > 1000 => $"Size: {_selectedPreviewAsset.Entry.DataSize / 1000} KB",
                _ => $"Size: {_selectedPreviewAsset.Entry.DataSize} bytes"
            };

            // Set values for the corresponding tab.
            if (tabPage == previewImageTab)
            {
                SetImagePreviewValue();
            }
            else if (tabPage == previewTextTab)
            {
                SetTextPreviewValue();
            }
            else if (tabPage == previewHexTab)
            {
                SetHexPreviewValue();
            }
        }

        /// <summary>
        /// Sets the hex preview to the data from <see cref="_selectedPreviewAsset"/>.
        /// </summary>
        private void SetHexPreviewValue()
        {
            if (_selectedPreviewAsset?.Data == null)
            {
                return;
            }

            previewHexBox.ByteProvider = new DynamicByteProvider(_selectedPreviewAsset.Data);
        }

        /// <summary>
        /// Sets the image preview to the data from <see cref="_selectedPreviewAsset"/>.
        /// </summary>
        private void SetImagePreviewValue()
        {
            if (_selectedPreviewAsset?.Data == null)
            {
                return;
            }

            if (_selectedPreviewAsset.MimeType?.PrimaryType != "image")
            {
                previewImageBox.Text = "No image available.";
                return;
            }

            previewImageBox.Text = string.Empty;

            try
            {
                var memoryStream = new MemoryStream();
                memoryStream.Write(_selectedPreviewAsset.Data, 0,
                    _selectedPreviewAsset.Data.Length);

                var bitmap = new Bitmap(memoryStream, false);
                memoryStream.Dispose();
                previewImageBox.Image = bitmap;
            }
            catch (Exception exception)
            {
                Log.Error("Could not convert byte array to bitmap: \n{exception}", exception);
            }
        }

        /// <summary>
        /// Sets the text preview to the data from <see cref="_selectedPreviewAsset"/>.
        /// </summary>
        private void SetTextPreviewValue()
        {
            if (_selectedPreviewAsset?.Data == null)
            {
                return;
            }

            var text = Encoding.UTF8.GetString(_selectedPreviewAsset.Data);
            text = Regex.Replace(text, @"[^\t\r\n -~]", string.Empty);

            if (!_formatPreviewText)
            {
                previewTextBox.Text = text;
                return;
            }

            switch (_selectedPreviewAsset.MimeType)
            {
                case { SubType: "json" }:
                {
                    var json = JsonNode.Parse(text);
                    var options = new JsonSerializerOptions { WriteIndented = true };
                    previewTextBox.Text = json!.ToJsonString(options);
                    break;
                }
                case { SubType: "xml" }:
                {
                    var xmlDocument = new XmlDocument();
                    try
                    {
                        xmlDocument.LoadXml(text);
                        var stringWriter = new StringWriter();
                        var xmlTextWriter = new XmlTextWriter(stringWriter);
                        xmlTextWriter.Formatting = Formatting.Indented;
                        xmlDocument.WriteTo(xmlTextWriter);
                        previewTextBox.Text = stringWriter.ToString();
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message);
                        previewTextBox.Text = text;
                    }

                    break;
                }
                default:
                {
                    previewTextBox.Text = text;
                    break;
                }
            }
        }

        #endregion Preview methods

        /// <summary>
        /// Gets the corresponding icon for the provided <paramref name="mimeType"/>.
        /// </summary>
        /// <param name="mimeType">A mimetype.</param>
        /// <returns>The index for an icon.</returns>
        private static int GetMimeTypeIconIndex(MimeType? mimeType)
        {
            if (mimeType == null)
            {
                return 6;
            }

            return mimeType.PrimaryType switch
            {
                "application" => 0,
                "audio" => 1,
                "font" => 2,
                "image" => 3,
                "text" => 4,
                "video" => 5,
                _ => 6
            };
        }

        private void RefreshFileTree()
        {
            if (_assets == null)
            {
                return;
            }

            var packageName = Path.GetFileNameWithoutExtension(_packagePath);
            var rootNode = new TreeNode(packageName)
            {
                ImageIndex = 8,
                SelectedImageIndex = 8
            };

            foreach (var asset in _assets.Where(a => a.Name.Contains(_searchQuery)))
            {
                var path = asset.Name;
                var currentNode = rootNode;
                var pathNodes = path.Split('/');
                for (var j = 0; j < pathNodes.Length; j++)
                {
                    var item = pathNodes[j];
                    var folder = currentNode.Nodes.Cast<TreeNode>().FirstOrDefault(x => x.Text.Equals(item));

                    if (folder != null)
                    {
                        currentNode = folder;
                        currentNode.ImageIndex = 7;
                    }
                    else
                    {
                        currentNode = currentNode.Nodes.Add(item);

                        if (j == pathNodes.Length - 1)
                        {
                            currentNode.ImageIndex = GetMimeTypeIconIndex(asset.MimeType);
                            currentNode.Tag = asset;
                        }
                        else
                        {
                            currentNode.ImageIndex = 7;
                        }
                    }

                    currentNode.SelectedImageIndex = currentNode.ImageIndex;
                }
            }

            explorerTreeView.Invoke(() =>
            {
                explorerTreeView.Nodes.Clear();
                if (rootNode.GetNodeCount(true) < 1)
                {
                    lblNoResults.Visible = true;
                    return;
                }

                lblNoResults.Visible = false;
                explorerTreeView.Nodes.Add(rootNode);
                explorerTreeView.ExpandAll();
            });
        }

        private void ScrollOutputToEnd()
        {
            outputBox.Invoke(() =>
            {
                outputBox.SelectionStart = Math.Max(0, outputBox.TextLength - 2);
                outputBox.ScrollToCaret();
            });
        }

        #endregion Custom methods

        #region Component events

        private void BtnAbout_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        private void BtnFormattedText_Click(object sender, EventArgs e)
        {
            _formatPreviewText = !_formatPreviewText;

            btnFormattedText.Image =
                _formatPreviewText ? Images.checkbox_checked : Images.checkbox_unchecked;

            if (previewTabs.SelectedTab == previewTextTab)
            {
                SetTextPreviewValue();
            }
        }

        private void BtnLoadDefinitions_Click(object sender, EventArgs e)
        {
            if (_assets == null)
            {
                MessageBox.Show("A resource package must be selected before a definition source can be set.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Definition file (*.txt)|*.txt";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                IReadOnlyDictionary<uint, string> crcDictionary;
                using (var fileStream = openFileDialog.OpenFile())
                {
                    crcDictionary = DefinitionHelper.CreateCrcDictionary(fileStream);
                }

                AssetHelper.UpdateAssetsWithDefinitions(_assets, crcDictionary);
                RefreshFileTree();
            }
        }

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            using var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "ResourcePack (*.dat)|*.dat";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                using (var stream = openFileDialog.OpenFile())
                {
                    _packageHeader = PackageHelper.GetHeader(stream);
                    Log.Information("ResourcePackage: {@info}",
                        new { _packageHeader.Id, _packageHeader.NumberOfEntries });

                    var entries = PackageHelper.LoadAllEntryInformation(_packageHeader, stream);
                    _password = string.Empty;

                    // Try to load the first asset to check whether the archive is encrypted.
                    if (!AssetHelper.LoadSingleFromPackage(stream, entries[0], out _))
                    {
                        var passwordDialog = new PasswordForm();
                        if (passwordDialog.ShowDialog() != DialogResult.OK)
                        {
                            return;
                        }

                        _password = passwordDialog.Password;
                        if (!AssetHelper.LoadSingleFromPackage(stream, entries[0], out _, _password))
                        {
                            throw new Exception("The password entered is incorrect.");
                        }
                    }

                    _assets = AssetHelper.LoadAllFromPackage(entries, stream, _password);
                }

                _packagePath = openFileDialog.FileName;
                RefreshFileTree();
                lblResultCount.Text = $"{_packageHeader.NumberOfEntries} Entries";

                btnLoadDefinitions.Enabled = true;
                btnExtract.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not open resource package. {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnToggleDebugMessages_Click(object sender, EventArgs e)
        {
            _showDebugMessages = !_showDebugMessages;

            btnToggleDebugMessages.Image =
                _showDebugMessages ? Images.checkbox_checked : Images.checkbox_unchecked;

            _loggingLevelSwitch.MinimumLevel =
                _showDebugMessages ? LogEventLevel.Debug : LogEventLevel.Information;
        }

        private void ExplorerTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag == null)
            {
                return;
            }

            _selectedPreviewAsset = (Asset)e.Node.Tag;

            switch (_selectedPreviewAsset.MimeType)
            {
                case { PrimaryType: "image" }:
                {
                    previewTabs.SelectedTab = previewImageTab;
                    break;
                }
                case { SubType: "xml" }:
                case { SubType: "json" }:
                case { PrimaryType: "text" }:
                {
                    previewTabs.SelectedTab = previewTextTab;
                    break;
                }
                default:
                {
                    previewTabs.SelectedTab = previewHexTab;
                    break;
                }
            }

            ReloadPreviewTab(previewTabs.SelectedTab);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.RichTextBox(outputBox, theme: ThemePresets.Light,
                    levelSwitch: _loggingLevelSwitch)
                .CreateLogger();
        }

        private void PreviewTabs_Selecting(object sender, TabControlCancelEventArgs e)
        {
            ReloadPreviewTab(e.TabPage);
        }

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            _searchQuery = ((TextBox)sender).Text;
            _searchDebouncer.Invoke();
        }

        private void SplitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {
            ScrollOutputToEnd();
        }

        #endregion Component events

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            var createDialogResult = new CreateForm().ShowDialog();
        }

        private void MainForm_ResizeBegin(object sender, EventArgs e)
        {
            SuspendLayout();
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            ScrollOutputToEnd();
            ResumeLayout(true);
        }
    }
}