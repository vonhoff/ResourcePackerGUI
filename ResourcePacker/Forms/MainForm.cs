using System.Diagnostics;
using ResourcePacker.Entities;
using ResourcePacker.Helpers;
using ResourcePacker.Properties;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.RichTextBoxForms.Themes;

namespace ResourcePacker.Forms
{
    public partial class MainForm : Form
    {
        private List<EntryDefinition>? _entries;
        private bool _showDebugMessages = true;
        private readonly LoggingLevelSwitch _loggingLevelSwitch = new(LogEventLevel.Debug);
        private PackHeader _packHeader;
        private Pack _packInformation;
        private string _packageName = string.Empty;

        public MainForm()
        {
            InitializeComponent();
        }

        private void BtnAbout_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
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
                    _packHeader = PackHelper.GetHeader(fileStream);
                    _packageName = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                    Log.Information("ResourcePackage: {@info}",
                        new { _packHeader.Id, _packHeader.NumberOfEntries });

                    _packInformation = PackHelper.LoadEntryInformation(_packHeader, fileStream);
                    lblResultCount.Text = $"{_packHeader.NumberOfEntries} Entries";
                    btnLoadDefinitions.Enabled = true;

                    Task.Run(() =>
                    {
                        _entries = DefinitionHelper.CreateEntryDefinitions(_packInformation);
                        RefreshFileTree();
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Could not open resource package. {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.RichTextBox(outputBox, theme: ThemePresets.Light,
                    levelSwitch: _loggingLevelSwitch)
                .CreateLogger();
        }

        private void BtnToggleDebugMessages_Click(object sender, EventArgs e)
        {
            _showDebugMessages = !_showDebugMessages;

            btnToggleDebugMessages.Image =
                _showDebugMessages ? Images.checkbox_checked : Images.checkbox_unchecked;

            _loggingLevelSwitch.MinimumLevel =
                _showDebugMessages ? LogEventLevel.Debug : LogEventLevel.Information;
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
                    _entries = DefinitionHelper.CreateEntryDefinitions(_packInformation, definitionStream);
                    RefreshFileTree();
                });
            }
        }

        private void RefreshFileTree()
        {
            explorerTreeView.Invoke(() => explorerTreeView.Nodes.Clear());

            if (_entries == null)
            {
                return;
            }

            var rootNode = new TreeNode(_packageName);
            foreach (var path in _entries.Select(e => e.Name))
            {
                var currentNode = rootNode;
                foreach (var item in path.Split('/'))
                {
                    var tmp = currentNode.Nodes.Cast<TreeNode>().Where(x => x.Text.Equals(item)).ToList();
                    currentNode = tmp.Count > 0 ? tmp.Single() : currentNode.Nodes.Add(item);
                }
            }

            explorerTreeView.Invoke(() =>
            {
                explorerTreeView.Nodes.Add(rootNode);
                rootNode.Expand();
            });
        }
    }
}