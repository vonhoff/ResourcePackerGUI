using System.IO;
using System.Runtime.InteropServices;
using ResourcePacker.Entities;
using ResourcePacker.Helpers;

namespace ResourcePacker.Forms
{
    public partial class MainForm : Form
    {
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
            new OpenForm().ShowDialog();
        }
    }
}