using ResourcePacker.Entities;
using ResourcePacker.Helpers;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.RichTextBoxForms.Themes;

namespace ResourcePacker.Forms
{
    public partial class OpenForm : Form
    {
        private PackHeader _packHeader;
        private Pack _pack;
        private Dictionary<uint, string> _items = new();
        private CancellationTokenSource _cancellationTokenSource = new();
        private readonly LoggingLevelSwitch _loggingLevelSwitch = new(LogEventLevel.Debug);
        private bool _processing;

        public OpenForm()
        {
            InitializeComponent();
        }

        private void BtnLocationExplore_Click(object sender, EventArgs e)
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
                    _pack = PackHelper.Load(_packHeader, fileStream);
                    txtFileLocation.Text = openFileDialog.FileName;
                    btnExecute.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Could not open resource package. {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UpdateProgress(object? sender, int percentage)
        {
            progressBar.Value = percentage;
        }

        private void BtnExecute_Click(object sender, EventArgs e)
        {
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (_processing)
            {
                _processing = false;
                if (_cancellationTokenSource.IsCancellationRequested)
                {
                    return;
                }

                _cancellationTokenSource.Cancel();
                _cancellationTokenSource = new CancellationTokenSource();
                return;
            }

            Close();
        }

        private void BtnDefinitionsExplore_Click(object sender, EventArgs e)
        {
            using var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Definition file (*.txt)|*.txt";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var fileStream = openFileDialog.OpenFile();

                try
                {
                    txtDefinitions.Text = openFileDialog.FileName;
                    var validate = DefinitionHelper.LoadDefinitions(fileStream);
                    if (validate == null)
                    {
                        return;
                    }
                    //_items = DefinitionHelper.Create(fileStream);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Could not open resource package. {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void OpenForm_Load(object sender, EventArgs e)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.RichTextBox(outputBox, theme: ThemePresets.Light, levelSwitch: _loggingLevelSwitch)
                .CreateLogger();
        }

        private void ChkShowDebug_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is CheckBox chk)
            {
                _loggingLevelSwitch.MinimumLevel =
                    chk.Checked ? LogEventLevel.Debug : LogEventLevel.Information;
            }
        }
    }
}