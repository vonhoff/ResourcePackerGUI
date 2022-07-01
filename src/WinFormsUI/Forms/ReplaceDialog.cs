namespace WinFormsUI.Forms
{
    public partial class ReplaceDialog : Form
    {
        public bool UseForAllCases { get; set; }

        public ReplaceDialog()
        {
            InitializeComponent();
        }

        public ReplaceDialog(string filePath, string availablePath, int duplicates)
        {
            InitializeComponent();
            txtFilePath.Text = filePath;

            if (duplicates == 0)
            {
                chkRepeatAll.Visible = false;
            }
            else
            {
                chkRepeatAll.Text = "Do this for the next " +
                                    (duplicates == 1 ? "conflict" : $"{duplicates} conflicts");
            }

            var secondaryName = Path.GetFileName(availablePath);
            lblBothFilesDescription.Text = $"The resource will be renamed to '{secondaryName}',\r\n" +
                                           "and extracted to the destination folder.";
        }

        private void ChkRepeatAll_CheckedChanged(object sender, EventArgs e)
        {
            UseForAllCases = chkRepeatAll.Checked;
        }
    }
}