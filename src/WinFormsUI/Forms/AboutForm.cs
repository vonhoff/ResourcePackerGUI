#region GNU General Public License

/* Copyright 2022 Simon Vonhoff & Contributors
 *
 * This file is part of ResourcePackerGUI.
 *
 * ResourcePackerGUI is free software: you can redistribute it and/or modify it under the terms of the
 * GNU General Public License as published by the Free Software Foundation, either version 3 of the License,
 * or (at your option) any later version.
 *
 * ResourcePackerGUI is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with ResourcePackerGUI.
 * If not, see <https://www.gnu.org/licenses/>.
 */

#endregion

using System.ComponentModel;
using System.Diagnostics;

namespace WinFormsUI.Forms
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

        private void AboutForm_Load(object sender, EventArgs e)
        {
            var assembly = typeof(AboutForm).Assembly;
            var version = assembly.GetName().Version?.ToString();
            var architecture = assembly.GetName().ProcessorArchitecture.ToString();
            lblVersion.Text = $"Version {version} ({architecture})";
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            Close();
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