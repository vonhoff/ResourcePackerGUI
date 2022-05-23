using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Xml;
using Be.Windows.Forms;
using ResourcePacker.Common;
using ResourcePacker.Entities;
using ResourcePacker.Extensions;
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
        private Pack _pack;
        private PackHeader _packHeader;
        private readonly ActionDebouncer _outputScrollDebouncer;
        private readonly ActionDebouncer _searchDebouncer;
        private readonly LoggingLevelSwitch _loggingLevelSwitch = new(LogEventLevel.Debug);
        private string _packageName = string.Empty;
        private string _searchQuery = string.Empty;
        private List<Asset>? _assets;
        private Asset? _selectedPreviewAsset;
        private bool _showDebugMessages = true;
        private bool _formatPreviewText = true;

        public MainForm()
        {
            InitializeComponent();
            _outputScrollDebouncer = new ActionDebouncer(ScrollOutputToEnd, TimeSpan.FromSeconds(0.25));
            _searchDebouncer = new ActionDebouncer(RefreshFileTree, TimeSpan.FromSeconds(0.35));
        }

        private void ScrollOutputToEnd()
        {
            outputBox.Invoke(() =>
            {
                outputBox.SelectionStart = Math.Max(0, outputBox.TextLength - 2);
                outputBox.ScrollToCaret();
            });
        }

        /// <summary>
        /// Returns the corresponding index of the image collection.
        /// </summary>
        /// <param name="mimeType">The MimeType to get the icon from.</param>
        /// <returns>An index for the image collection.</returns>
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

        private void BtnAbout_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
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
                var definitionStream = openFileDialog.OpenFile();
                var crcDictionary = DefinitionHelper.CreateCrcDictionary(definitionStream);
                var matches = AssetHelper.UpdateAssetsWithDefinitions(_assets, crcDictionary);
                if (matches == 0)
                {
                    MessageBox.Show(
                        "Could not set definitions. The specified file does not contain valid file definitions.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

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

            var fileStream = openFileDialog.OpenFile();
            try
            {
                // Get pack information.
                _packHeader = PackHelper.GetHeader(fileStream);
                Log.Information("ResourcePackage: {@info}",
                    new { _packHeader.Id, _packHeader.NumberOfEntries });

                _pack = PackHelper.LoadAllEntryInformation(_packHeader, fileStream);

                // Try to load the first asset to check whether the archive is encrypted.
                if (!AssetHelper.LoadSingleFromPackage(_pack, _pack.Entries[0], out var asset))
                {
                    var passwordDialog = new PasswordForm();
                    if (passwordDialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    //_assets = AssetHelper.UpdateAssetsWithDefinitions(_pack, passwordDialog.Password);
                }
                else
                {
                    _assets = AssetHelper.LoadAllFromPackage(_pack);
                }

                _packageName = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                RefreshFileTree();

                lblResultCount.Text = $"{_packHeader.NumberOfEntries} Entries";
                btnLoadDefinitions.Enabled = true;
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

        private void MainForm_Load(object sender, EventArgs e)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.RichTextBox(outputBox, theme: ThemePresets.Light,
                    levelSwitch: _loggingLevelSwitch)
                .CreateLogger();
        }

        /// <summary>
        /// Refreshes the file explorer <see cref="TreeView"/> with
        /// data from the asset list.
        /// </summary>
        private void RefreshFileTree()
        {
            if (_assets == null)
            {
                return;
            }

            var rootNode = new TreeNode(_packageName)
            {
                ImageIndex = 8,
                SelectedImageIndex = 8
            };

            foreach (var asset in _assets.Where(a => a.Name.Contains(_searchQuery)))
            {
                var path = asset.Name;
                var currentNode = rootNode;
                var nodes = path.Split('/');
                for (var j = 0; j < nodes.Length; j++)
                {
                    var item = nodes[j];
                    var folder = currentNode.Nodes.Cast<TreeNode>().FirstOrDefault(x => x.Text.Equals(item));

                    if (folder != null)
                    {
                        currentNode = folder;
                        currentNode.ImageIndex = 7;
                    }
                    else
                    {
                        currentNode = currentNode.Nodes.Add(item);

                        if (j == nodes.Length - 1)
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

        private void SplitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {
            _outputScrollDebouncer.Invoke();
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            _outputScrollDebouncer.Invoke();
        }
        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            _searchQuery = ((TextBox)sender).Text;
            _searchDebouncer.Invoke();
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

            PreparePreviewTab(previewTabs.SelectedTab);
        }

        private void BtnFormattedText_Click(object sender, EventArgs e)
        {
            _formatPreviewText = !_formatPreviewText;

            btnFormattedText.Image =
                _formatPreviewText ? Images.checkbox_checked : Images.checkbox_unchecked;

            PreparePreviewTab(previewTabs.SelectedTab);
        }

        private void PreparePreviewTab(TabPage? tabPage)
        {
            previewHexBox.ByteProvider = null;
            previewImageBox.Image = null;
            previewTextBox.Clear();

            if (_selectedPreviewAsset?.Data == null)
            {
                return;
            }

            lblMediaType.Text = $"Media type: {_selectedPreviewAsset.MimeType?.Name}";
            lblDataSize.Text = _selectedPreviewAsset.Entry.DataSize switch
            {
                > 1000000000 => $"Size: {_selectedPreviewAsset.Entry.DataSize / 1000000000}gb",
                > 1000000 => $"Size: {_selectedPreviewAsset.Entry.DataSize / 1000000}mb",
                > 1000 => $"Size: {_selectedPreviewAsset.Entry.DataSize / 1000}kb",
                _ => $"Size: {_selectedPreviewAsset.Entry.DataSize}b"
            };

            if (tabPage == previewImageTab)
            {
                if (_selectedPreviewAsset.MimeType?.PrimaryType != "image")
                {
                    previewImageBox.Text = "No image available.";
                    return;
                }

                previewImageBox.Text = string.Empty;
                _selectedPreviewAsset.Bitmap ??= _selectedPreviewAsset.Data.ToBitmap();
                if (_selectedPreviewAsset != null)
                {
                    previewImageBox.Image = _selectedPreviewAsset.Bitmap;
                }
            }
            else if (tabPage == previewTextTab)
            {
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
                        xmlDocument.LoadXml(text);
                        var stringWriter = new StringWriter();
                        var xmlTextWriter = new XmlTextWriter(stringWriter);
                        xmlTextWriter.Formatting = Formatting.Indented;
                        xmlDocument.WriteTo(xmlTextWriter);
                        previewTextBox.Text = stringWriter.ToString();
                        break;
                    }
                    default:
                    {
                        previewTextBox.Text = text;
                        break;
                    }
                }
            }
            else if (tabPage == previewHexTab)
            {
                previewHexBox.ByteProvider = new DynamicByteProvider(_selectedPreviewAsset.Data);
            }
        }

        private void PreviewTabs_Selecting(object sender, TabControlCancelEventArgs e)
        {
            PreparePreviewTab(e.TabPage);
        }
    }
}