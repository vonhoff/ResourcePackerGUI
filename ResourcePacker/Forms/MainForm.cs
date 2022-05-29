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
        private readonly LoggingLevelSwitch _loggingLevelSwitch = new(LogEventLevel.Debug);
        private readonly ActionDebouncer _outputScrollDebouncer;
        private readonly ActionDebouncer _searchDebouncer;
        private List<Asset>? _assets;
        private Package _package;
        private PackageHeader _packageHeader;
        private Asset? _selectedPreviewAsset;
        private string _packagePath = string.Empty;
        private string _searchQuery = string.Empty;
        private string _password = string.Empty;
        private bool _formatPreviewText = true;
        private bool _showDebugMessages = true;
        private bool _packageExists;

        public MainForm()
        {
            InitializeComponent();
            _outputScrollDebouncer = new ActionDebouncer(ScrollOutputToEnd, TimeSpan.FromSeconds(0.25));
            _searchDebouncer = new ActionDebouncer(RefreshFileTree, TimeSpan.FromSeconds(0.35));
        }

        #region Custom methods

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
            else if (tabPage == previewHexTab)
            {
                previewHexBox.ByteProvider = new DynamicByteProvider(_selectedPreviewAsset.Data);
            }
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

            PreparePreviewTab(previewTabs.SelectedTab);
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

            try
            {
                using (var stream = openFileDialog.OpenFile())
                {
                    _packageHeader = PackageHelper.GetHeader(stream);
                    Log.Information("ResourcePackage: {@info}",
                        new { _packageHeader.Id, _packageHeader.NumberOfEntries });

                    _package = PackageHelper.LoadAllEntryInformation(_packageHeader, stream);

                    // Try to load the first asset to check whether the archive is encrypted.
                    if (!AssetHelper.LoadSingleFromPackage(_package, _package.Entries[0], out _))
                    {
                        var passwordDialog = new PasswordForm();
                        if (passwordDialog.ShowDialog() != DialogResult.OK)
                        {
                            return;
                        }

                        _password = passwordDialog.Password;
                        if (!AssetHelper.LoadSingleFromPackage(_package, _package.Entries[0], out _, _password))
                        {
                            throw new Exception("The password entered is incorrect.");
                        }
                    }

                    _assets = AssetHelper.LoadAllFromPackage(_package, _password);
                }

                _packageExists = true;
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

            PreparePreviewTab(previewTabs.SelectedTab);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.RichTextBox(outputBox, theme: ThemePresets.Light,
                    levelSwitch: _loggingLevelSwitch)
                .CreateLogger();
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            _outputScrollDebouncer.Invoke();
        }

        private void PreviewTabs_Selecting(object sender, TabControlCancelEventArgs e)
        {
            PreparePreviewTab(e.TabPage);
        }

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            _searchQuery = ((TextBox)sender).Text;
            _searchDebouncer.Invoke();
        }

        private void SplitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {
            _outputScrollDebouncer.Invoke();
        }

        #endregion Component events
    }
}