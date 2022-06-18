namespace ResourcePacker.Forms
{
    public partial class ReplaceDialog : Form
    {
        public bool UseForAllCases { get; set; }

        public ReplaceDialog()
        {
            InitializeComponent();
        }

        public ReplaceDialog(string filePath, string availableFileName)
        {
            InitializeComponent();
            txtFilePath.Text = filePath;

            var secondaryName = Path.GetFileName(availableFileName);
            lblBothFilesDescription.Text = $"The asset will be extracted and \r\nrenamed to '{secondaryName}'.";
        }

        private void ChkRepeatAll_CheckedChanged(object sender, EventArgs e)
        {
            UseForAllCases = chkRepeatAll.Checked;
        }
    }
}