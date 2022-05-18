using System.Collections;
using System.Diagnostics;
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
        private bool _showDebugMessages = true;
        private List<Asset>? _assets;

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
            using var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Definition file (*.txt)|*.txt";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var definitionStream = openFileDialog.OpenFile();
            var crcDictionary = DefinitionHelper.CreateCrcDictionary(definitionStream);
            var assets = AssetHelper.LoadAssetsFromPackage(_pack, crcDictionary);
            if (assets.Count == 0)
            {
                MessageBox.Show("Could not set definitions. The specified file does not contain valid file definitions.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _assets = assets;
            RefreshFileTree();
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

                _packageName = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                _pack = PackHelper.LoadEntryInformation(_packHeader, fileStream);

                _assets = AssetHelper.LoadAssetsFromPackage(_pack);
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

            var asset = (Asset)e.Node.Tag;
            if (asset.MimeType?.PrimaryType != "image")
            {
                return;
            }

            var bitmap = asset.Data.ToBitmap();
            if (bitmap == null)
            {
                return;
            }

            imageBox.Image = bitmap;
        }
    }
}