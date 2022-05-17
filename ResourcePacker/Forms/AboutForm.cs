using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

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

        private void AboutForm_Load(object sender, EventArgs e)
        {
            var assembly = typeof(AboutForm).Assembly;
            var version = assembly.GetName().Version?.ToString();
            var architecture = assembly.GetName().ProcessorArchitecture.ToString();
            lblVersion.Text = $"Version {version} ({architecture})";
        }

        private void RichTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = e.LinkText,
                UseShellExecute = true
            });
        }
    }
}