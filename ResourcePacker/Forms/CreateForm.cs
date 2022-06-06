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

using ResourcePacker.Helpers;
using Serilog;

namespace ResourcePacker.Forms
{
    public partial class CreateForm : Form
    {
        private readonly HashSet<string> _assetsToInclude = new();
        private readonly TimeSpan _progressTimeInterval = TimeSpan.FromMilliseconds(20);
        private CancellationTokenSource _cancellationTokenSource;
        private DateTime _progressLastUpdated = DateTime.UtcNow;

        public CreateForm()
        {
            InitializeComponent();

            _cancellationTokenSource = new CancellationTokenSource();
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

        private void BtnAssetExplore_Click(object sender, EventArgs e)
        {
            using var browserDialog = new FolderBrowserDialog();
            var result = browserDialog.ShowDialog();

            if (result != DialogResult.OK || string.IsNullOrWhiteSpace(browserDialog.SelectedPath))
            {
                return;
            }

            var selectedPath = browserDialog.SelectedPath;
            if (DirectoryHelper.CheckDirectoryEmpty(selectedPath))
            {
                MessageBox.Show("The specified directory does not contain any files.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            txtAssetFolder.Text = selectedPath;

            var assetPathNodes = selectedPath.Replace(@"\", "/")
                .Split('/').Where(n => n.Length > 0).ToArray();
            var relativeDepth = assetPathNodes.Length;
            var rootNode = new TreeNode(assetPathNodes.Last() + " (root)")
            {
                Checked = true,
                StateImageIndex = 1
            };

            IProgress<(int percentage, string path)> progress =
                new Progress<(int percentage, string path)>(UpdateFileCollectionProgress);

            _cancellationTokenSource = new CancellationTokenSource();
            Task.Run(() => FileCollectionTask(selectedPath, rootNode, relativeDepth, progress));
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

        private void ChkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        private void CreateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _cancellationTokenSource.Cancel();
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

        private void ExplorerTreeView_AfterStateChanged(object sender, TreeViewEventArgs e)
        {
            btnCreate.Enabled = _assetsToInclude.Count != 0;
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

        private void FileCollectionTask(string selectedPath, TreeNode rootNode, int relativeDepth, IProgress<(int percentage, string path)> progress)
        {
            var hideCancelNotification = false;
            var files = Array.Empty<string>();

            Invoke(() =>
            {
                lblPercentage.Text = "0%";
                lblStatus.Text = "Collecting available files...";
                progressBar.Style = ProgressBarStyle.Marquee;
            });

            try
            {
                files = GetAllFiles(selectedPath, string.Empty, _cancellationTokenSource.Token).ToArray();
            }
            catch (OperationCanceledException ex)
            {
                Log.Debug(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An exception occurred during file collection.");
                return;
            }

            if (files.Length == 0 && !_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
                hideCancelNotification = true;
                MessageBox.Show("The specified directory does not contain any files.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            _assetsToInclude.Clear();

            Invoke(() =>
            {
                selectorTreeView.Nodes.Clear();

                lblAvailableItems.Text = "Available items: 0";
                lblSelectedItems.Text = "Selected items: 0";
                progressBar.Style = ProgressBarStyle.Blocks;
                lblStatus.Text = "Creating tree nodes...";
            });

            PopulateSelectorRootNode(rootNode, relativeDepth, progress, files);
            UpdateSelectorComponents(rootNode, hideCancelNotification);
        }

        private void PopulateSelectorRootNode(TreeNode rootNode, int relativeDepth, IProgress<(int percentage, string path)> progress, string[] files)
        {
            for (var i = 0; i < files.Length; i++)
            {
                if (_cancellationTokenSource.IsCancellationRequested)
                {
                    break;
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

                // Use the last 1 percent for updating the tree view.
                progress.Report(((int)((double)(i + 1) / files.Length * 99),
                    string.Join("/", pathNodes[relativeDepth..])));
            }
        }

        private void UpdateFileCollectionProgress((int percentage, string path) progress)
        {
            if (_progressLastUpdated >= DateTime.UtcNow)
            {
                return;
            }

            _progressLastUpdated = DateTime.UtcNow + _progressTimeInterval;

            var (percentage, path) = progress;
            progressBar.Value = percentage;
            lblPercentage.Text = $"{percentage}%";
            lblPercentage.Refresh();
            lblStatusFile.Text = path;
            lblStatusFile.Refresh();
        }

        private void UpdateSelectorComponents(TreeNode rootNode, bool hideCancelNotification)
        {
            lblStatusFile?.Invoke(() => lblStatusFile.Text = string.Empty);
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                _assetsToInclude.Clear();

                Invoke(() =>
                {
                    txtAssetFolder.Text = string.Empty;
                    lblPercentage.Text = "0%";
                    lblStatus.Text = "Ready";
                    progressBar.Value = 0;
                    btnCreate.Enabled = false;
                });

                if (!hideCancelNotification)
                {
                    MessageBox.Show("The operation has been cancelled.", "Operation cancelled",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                Invoke(() =>
                {
                    Cursor.Current = Cursors.WaitCursor;
                    lblStatus.Text = "Updating tree view...";
                    lblStatus.Refresh();

                    selectorTreeView.BeginUpdate();
                    selectorTreeView.Nodes.Add(rootNode);
                    selectorTreeView.ExpandAll();
                    selectorTreeView.Nodes[0].EnsureVisible();
                    selectorTreeView.EndUpdate();

                    lblStatus.Text = "Ready";
                    lblPercentage.Text = "100%";
                    progressBar.Value = 100;
                    btnCreate.Enabled = true;
                    btnCreate.Focus();
                    Cursor.Current = Cursors.Default;
                });

                _cancellationTokenSource.Cancel();
            }

            Invoke(() =>
            {
                lblAvailableItems.Text = $"Available items: {_assetsToInclude.Count}";
                lblSelectedItems.Text = $"Selected items: {_assetsToInclude.Count}";
            });
        }
    }
}