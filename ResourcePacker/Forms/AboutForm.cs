using System.ComponentModel;
using System.Diagnostics;

namespace ResourcePacker.Forms
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void AboutForm_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.github.com/vonhoff/ResourcePacker.GUI",
                UseShellExecute = true
            });
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}