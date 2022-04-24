using ResourcePacker.Sinks;

namespace ResourcePacker.Controls
{
    public partial class LogTextBox : UserControl
    {
        private bool _isContextConfigured;

        public LogTextBox()
        {
            InitializeComponent();
        }

        public string ForContext { get; set; } = string.Empty;
        public BorderStyle LogBorderStyle { get; set; } = BorderStyle.Fixed3D;
        public Padding LogPadding { get; set; } = new(3, 3, 3, 3);
        public bool ReadOnly { get; set; }
        public ScrollBars ScrollBars { get; set; }

        public void ClearLogs()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)(() => TxtLogControl.Clear()));
            }
            else
            {
                TxtLogControl.Clear();
            }
        }

        private void LogTextBox_Load(object sender, EventArgs e)
        {
            TxtLogControl.ScrollBars = ScrollBars;
            TxtLogControl.Padding = LogPadding;
            TxtLogControl.ReadOnly = ReadOnly;
            TxtLogControl.BorderStyle = LogBorderStyle;
            TxtLogControl.Font = Font;
            TxtLogControl.ForeColor = ForeColor;
            TxtLogControl.BackColor = BackColor;
            _isContextConfigured = !string.IsNullOrEmpty(ForContext);

            WinFormsSink.TextBoxSink.OnLogReceived += TextBoxSinkOnLogReceived;
            HandleDestroyed += (_, _) => WinFormsSink.TextBoxSink.OnLogReceived -= TextBoxSinkOnLogReceived;
        }

        private void PrintText(string str)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    TxtLogControl.AppendText(str);
                    TxtLogControl.ScrollToCaret();
                });
            }
            else
            {
                TxtLogControl.AppendText(str);
                TxtLogControl.ScrollToCaret();
            }
        }

        private void TextBoxSinkOnLogReceived(string? context, string str)
        {
            if (_isContextConfigured)
            {
                if (!string.IsNullOrEmpty(ForContext) &&
                    !string.IsNullOrEmpty(context) &&
                    ForContext.Equals(context, StringComparison.InvariantCultureIgnoreCase))
                {
                    PrintText(str);
                }
            }
            else
            {
                PrintText(str);
            }

            Application.DoEvents();
        }
    }
}