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