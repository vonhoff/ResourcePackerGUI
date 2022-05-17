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
        private List<Asset>? _assets;
        private Pack _pack;
        private string _packageName = string.Empty;
        private PackHeader _packHeader;
        private bool _showDebugMessages = true;

        public MainForm()
        {
            InitializeComponent();
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

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var definitionStream = openFileDialog.OpenFile();

                Task.Run(() =>
                {
                    var crcDictionary = DefinitionHelper.CreateCrcDictionary(definitionStream);
                    var entries = AssetHelper.LoadAllFromPackage(_pack, crcDictionary);
                    if (entries.Count == 0)
                    {
                        MessageBox.Show("Failed to proceed with setting definitions. The specified file does not contain valid file definitions.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    _assets = entries;
                    RefreshFileTree();
                });
            }
        }

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            using var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "ResourcePack (*.dat)|*.dat";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var fileStream = openFileDialog.OpenFile();
                try
                {
                    // Get pack information.
                    _packHeader = PackHelper.GetHeader(fileStream);
                    _packageName = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                    _pack = PackHelper.LoadEntryInformation(_packHeader, fileStream);
                    _assets = AssetHelper.LoadAllFromPackage(_pack);
                    Log.Information("ResourcePackage: {@info}",
                        new { _packHeader.Id, _packHeader.NumberOfEntries });

                    lblResultCount.Text = $"{_packHeader.NumberOfEntries} Entries";
                    RefreshFileTree();
                    btnLoadDefinitions.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Could not open resource package. {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
        /// data from the entity list.
        /// </summary>
        private void RefreshFileTree()
        {
            explorerTreeView.Invoke(explorerTreeView.Nodes.Clear);

            if (_assets == null)
            {
                return;
            }

            var rootNode = new TreeNode(_packageName)
            {
                ImageIndex = 8, 
                SelectedImageIndex = 8
            };

            rootNode.Expand();

            foreach (var asset in _assets)
            {
                var path = asset.Name;
                var currentNode = rootNode;
                foreach (var item in path.Split('/'))
                {
                    var treeNodes = currentNode.Nodes.Cast<TreeNode>().
                        Where(x => x.Text.Equals(item)).ToList();
                    currentNode = treeNodes.Count > 0 ? treeNodes.Single() : currentNode.Nodes.Add(item);

                    // Set the corresponding icon.
                    currentNode.ImageIndex = treeNodes.Count > 0 ? 7 :
                        GetMimeTypeIconIndex(asset.MimeType);

                    currentNode.SelectedImageIndex = currentNode.ImageIndex;
                }
            }

            explorerTreeView.Invoke(() => explorerTreeView.Nodes.Add(rootNode));
        }
    }
}