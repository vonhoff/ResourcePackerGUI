#region GNU General Public License

/* Copyright 2022 Simon Vonhoff
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

using System.Runtime.InteropServices;
using ResourcePacker.Helpers;
using Serilog;

namespace ResourcePacker.Forms
{
    public partial class CreateForm : Form
    {
        private readonly HashSet<string> _assetsToInclude = new();
        private DateTime _progressLastUpdated = DateTime.UtcNow;
        private readonly TimeSpan _progressTimeInterval = TimeSpan.FromMilliseconds(32);
        private CancellationTokenSource _cancellationTokenSource;

        public CreateForm()
        {
            InitializeComponent();

            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationTokenSource.Cancel();
        }

        private void ChkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        private void BtnAssetExplore_Click(object sender, EventArgs e)
        {
            using var browserDialog = new FolderBrowserDialog();
            var result = browserDialog.ShowDialog();

            if (result != DialogResult.OK || string.IsNullOrWhiteSpace(browserDialog.SelectedPath))
            {
                return;
            }

            var selectedPath = browserDialog.SelectedPath;

            // Check if the specified directory is empty.
            if (DirectoryHelper.CheckDirectoryEmpty(selectedPath))
            {
                MessageBox.Show("The specified directory does not contain any files.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            txtAssetFolder.Text = selectedPath;

            var assetPathNodes = selectedPath.Replace(@"\", "/").Split('/');
            var relativeDepth = assetPathNodes.Length;
            var rootNode = new TreeNode(assetPathNodes.Last() + " (root)")
            {
                Checked = true,
                StateImageIndex = 1
            };

            IProgress<(int percentage, string path)> progress = new Progress<(int percentage, string path)>(progress =>
            {
                if (_progressLastUpdated >= DateTime.UtcNow)
                {
                    return;
                }

                if (progressBar.Style == ProgressBarStyle.Marquee)
                {
                    progressBar.Style = ProgressBarStyle.Blocks;
                    lblStatus.Text = "Creating tree nodes...";
                    lblStatus.Refresh();
                }

                _progressLastUpdated = DateTime.UtcNow + _progressTimeInterval;

                progressBar.Value = progress.percentage;
                lblPercentage.Text = $"{progress.percentage}%";
                lblPercentage.Refresh();
                lblStatusFile.Text = progress.path;
                lblStatusFile.Refresh();
            });

            lblPercentage.Text = "0%";
            lblStatus.Text = "Collecting available files...";
            progressBar.Style = ProgressBarStyle.Marquee;
            _cancellationTokenSource = new CancellationTokenSource();

            Task.Run(() =>
            {
                string[] files;

                try
                {
                    files = GetAllFiles(selectedPath, string.Empty, _cancellationTokenSource.Token).ToArray();
                }
                catch
                {
                    return;
                }

                if (files.Length == 0)
                {
                    return;
                }

                _assetsToInclude.Clear();

                for (var i = 0; i < files.Length; i++)
                {
                    if (_cancellationTokenSource.IsCancellationRequested)
                    {
                        return;
                    }

                    var filePath = files[i];
                    var currentNode = rootNode;
                    var pathNodes = filePath.Replace(@"\", "/").Split('/');
                    for (var j = relativeDepth; j < pathNodes.Length; j++)
                    {
                        var item = pathNodes[j];
                        var folder = currentNode.Nodes.Cast<TreeNode>().FirstOrDefault(x => x.Text.Equals(item));

                        if (folder != null)
                        {
                            currentNode = folder;
                        }
                        else
                        {
                            currentNode = currentNode.Nodes.Add(item);

                            if (j == pathNodes.Length - 1)
                            {
                                currentNode.Tag = filePath;
                                _assetsToInclude.Add(filePath);
                            }
                        }
                    }

                    // Use the last 2 percent for updating the tree view.
                    progress.Report(((int)((double)(i + 1) / files.Length * 98), 
                        string.Join("/", pathNodes[relativeDepth..])));
                }
            }, _cancellationTokenSource.Token)
                .ContinueWith(_ =>
                {
                    Invoke(() =>
                    {
                        if (_cancellationTokenSource.IsCancellationRequested)
                        {
                            txtAssetFolder.Text = string.Empty;
                            _assetsToInclude.Clear();
                        }

                        lblStatusFile.Text = string.Empty;
                        lblStatusFile.Refresh();

                        explorerTreeView.Nodes.Clear();

                        if (!_cancellationTokenSource.IsCancellationRequested)
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            lblStatus.Text = "Updating tree view...";
                            lblStatus.Refresh();

                            explorerTreeView.BeginUpdate();
                            explorerTreeView.Nodes.Add(rootNode);
                            explorerTreeView.ExpandAll();
                            explorerTreeView.Nodes[0].EnsureVisible();
                            explorerTreeView.EndUpdate();
                            Cursor.Current = Cursors.Default;
                        }

                        lblAvailableItems.Text = $"Available items: {_assetsToInclude.Count}";
                        lblSelectedItems.Text = $"Selected items: {_assetsToInclude.Count}";

                        progressBar.Style = ProgressBarStyle.Blocks;
                        lblStatus.Text = "Ready";

                        if (_cancellationTokenSource.IsCancellationRequested)
                        {
                            lblPercentage.Text = "0%";
                            progressBar.Value = 0;
                            btnCreate.Enabled = false;
                            MessageBox.Show("The operation has been cancelled.",
                                "Operation cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            lblPercentage.Text = "100%";
                            progressBar.Value = 100;
                            btnCreate.Enabled = true;
                            btnCreate.Focus();
                            _cancellationTokenSource.Cancel();
                        }
                    });
                });
        }


        private static IEnumerable<string> GetAllFiles(string path, string searchPattern, CancellationToken cancellationToken)
        {
            return Directory.EnumerateFiles(path, searchPattern).Union(
                Directory.EnumerateDirectories(path).SelectMany(d =>
                {
                    try
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        return GetAllFiles(d, searchPattern, cancellationToken);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Log.Warning(ex, "Could not access directory: {path}", d);
                        return Enumerable.Empty<string>();
                    }
                }));
        }

        private void BtnPackageExplore_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "ResourcePack (*.dat)|*.dat";
            var result = saveFileDialog.ShowDialog();

            if (result != DialogResult.OK || string.IsNullOrEmpty(saveFileDialog.FileName))
            {
                return;
            }

            txtPackageLocation.Text = saveFileDialog.FileName;
        }

        private void ExplorerTreeView_NodeStateChanged(object sender, TreeViewEventArgs e)
        {
            var node = e.Node;
            if (node is not { Tag: string })
            {
                return;
            }

            var path = node.Tag.ToString();
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            var hasItem = _assetsToInclude.Contains(path);
            if (node.Checked)
            {
                if (!hasItem)
                {
                    _assetsToInclude.Add(path);
                }
            }
            else if (hasItem)
            {
                _assetsToInclude.Remove(path);
            }

            lblSelectedItems.Text = $"Selected items: {_assetsToInclude.Count}";
        }

        private void ExplorerTreeView_AfterStateChanged(object sender, TreeViewEventArgs e)
        {
            btnCreate.Enabled = _assetsToInclude.Count != 0;
        }

        private void CreateForm_ResizeBegin(object sender, EventArgs e)
        {
            SuspendLayout();
        }

        private void CreateForm_ResizeEnd(object sender, EventArgs e)
        {
            ResumeLayout(true);
        }

        private void CreateForm_SizeChanged(object sender, EventArgs e)
        {
            splitContainer2.SplitterDistance =
                Math.Min(splitContainer2.SplitterDistance,
                    splitContainer2.Width - splitContainer2.Panel2MinSize);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                Close();
                return;
            }

            _cancellationTokenSource.Cancel();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
    }
}