#region GNU General Public License

/* Copyright 2022 Vonhoff, MaxtorCoder
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

using System.Media;
using ResourcePacker.Helpers;
using Serilog;

namespace ResourcePacker.Forms
{
    public partial class CreateForm : Form
    {
        private readonly List<string> _assetsToInclude = new();
        private CancellationTokenSource _cancellationTokenSource;
        private string _definitionsLocation = string.Empty;
        private string _packageLocation = string.Empty;
        private bool _automaticDefinitionFile = true;
        private bool _createDefinitionFile = true;
        private int _relativePackageLocationDepth;

        // Progress variables
        private readonly IProgress<(int percentage, string path)> _progressPrimary;
        private readonly IProgress<int> _progressSecondary;
        private readonly TimeSpan _progressTimeInterval = TimeSpan.FromMilliseconds(50);
        private DateTime _progressLastUpdatedPrimary;
        private DateTime _progressLastUpdatedSecondary;

        public CreateForm()
        {
            InitializeComponent();
            _progressPrimary = new Progress<(int percentage, string path)>(UpdateFileCollectionProgress);
            _progressSecondary = new Progress<int>(UpdateEncryptionProgress);
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

        private void BtnAssetExplore_Click(object sender, EventArgs e)
        {
            using var browserDialog = new FolderBrowserDialog();
            var result = browserDialog.ShowDialog();

            if (result != DialogResult.OK || string.IsNullOrWhiteSpace(browserDialog.SelectedPath))
            {
                return;
            }

            var selectedPath = browserDialog.SelectedPath;
            if (!Directory.EnumerateFiles(selectedPath, string.Empty, SearchOption.AllDirectories).Any())
            {
                MessageBox.Show("The specified directory does not contain any files.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            txtAssetFolder.Text = selectedPath;

            var assetPathNodes = selectedPath
                .Replace(@"\", "/")
                .Split('/')
                .Where(n => n.Length > 0)
                .ToArray();

            _relativePackageLocationDepth = assetPathNodes.Length;
            var rootNode = new TreeNode(assetPathNodes.Last() + " (root)")
            {
                Checked = true,
                StateImageIndex = 1
            };

            _cancellationTokenSource = new CancellationTokenSource();
            Task.Run(() =>
            {
                try
                {
                    var files = CollectFiles(selectedPath);
                    CreateSelectorNodes(rootNode, _relativePackageLocationDepth, files);

                    _cancellationTokenSource.Token.ThrowIfCancellationRequested();
                    Invoke(() =>
                    {
                        lblStatusFile.Text = string.Empty;
                        Cursor.Current = Cursors.WaitCursor;
                        lblStatus.Text = "Updating tree view...";
                        lblStatus.Refresh();

                        selectorTreeView.BeginUpdate();
                        selectorTreeView.Nodes.Add(rootNode);
                        selectorTreeView.ExpandAll();
                        selectorTreeView.Nodes[0].EnsureVisible();
                        selectorTreeView.EndUpdate();

                        lblStatus.Text = "Ready";
                        lblPercentage.Text = "0%";
                        progressBarPrimary.Value = 0;
                        btnCreate.Enabled = _packageLocation != string.Empty;
                        Cursor.Current = Cursors.Default;

                        lblAvailableItems.Text = $"Available items: {_assetsToInclude.Count}";
                        lblSelectedItems.Text = $"Selected items: {_assetsToInclude.Count}";
                    });
                }
                catch (OperationCanceledException ex)
                {
                    MessageBox.Show(ex.Message, "Operation canceled",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (!IsDisposed)
                    {
                        _assetsToInclude.Clear();
                        Invoke(() =>
                        {
                            lblStatusFile.Text = string.Empty;
                            txtAssetFolder.Text = string.Empty;
                            lblStatus.Text = "Ready";
                            lblPercentage.Text = "0%";
                            progressBarPrimary.Style = ProgressBarStyle.Blocks;
                            progressBarPrimary.Value = 0;
                            btnCreate.Enabled = false;
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                _cancellationTokenSource.Cancel();
            });
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

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource = new CancellationTokenSource();

            Task.Run(() =>
            {
                var definitionsLocation = _createDefinitionFile ? _definitionsLocation : string.Empty;

                Invoke(() => lblStatus.Text = "Creating definitions...");
                var paths = DefinitionHelper.CreateDefinitionFile(
                    _assetsToInclude, _relativePackageLocationDepth, definitionsLocation,
                    _packageLocation, _progressSecondary);

                Invoke(() => lblStatus.Text = "Packing assets...");
                try
                {
                    PackageHelper.BuildPackage(paths, _packageLocation,
                        txtPassword.Text, _progressPrimary, _progressSecondary,
                        _cancellationTokenSource.Token);
                }
                catch (OperationCanceledException ex)
                {
                    Invoke(() =>
                    {
                        lblStatus.Text = "Ready";
                        lblPercentage.Text = "0%";
                        lblStatusFile.Text = string.Empty;
                        progressBarPrimary.Value = 0;
                        progressBarSecondary.Value = 0;
                    });

                    MessageBox.Show(ex.Message, "Operation canceled",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Could not build resource package. {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Cleanup after cancellation.
                if (_cancellationTokenSource.IsCancellationRequested)
                {
                    try
                    {
                        if (File.Exists(_packageLocation))
                        {
                            File.Delete(_packageLocation);
                        }

                        if (File.Exists(_definitionsLocation))
                        {
                            File.Delete(_definitionsLocation);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "An exception occurred during cleanup after cancellation.");
                    }

                    return;
                }

                // Set every relevant labels and elements to completion.
                Invoke(() =>
                {
                    lblStatus.Text = "Done";
                    lblPercentage.Text = "100%";
                    lblStatusFile.Text = string.Empty;
                    progressBarPrimary.Value = 100;
                    progressBarSecondary.Value = 100;
                });

                SystemSounds.Asterisk.Play();
                _cancellationTokenSource.Cancel();
            });
        }

        private void BtnDefinitionsExplore_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Text file (*.txt)|*.txt"
            };

            var result = saveFileDialog.ShowDialog();
            if (result == DialogResult.OK && !string.IsNullOrEmpty(saveFileDialog.FileName))
            {
                _automaticDefinitionFile = false;
                _definitionsLocation = saveFileDialog.FileName;
                txtDefinitionsLocation.Text = saveFileDialog.FileName;
            }
        }

        private void BtnPackageExplore_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "ResourcePack (*.dat)|*.dat"
            };

            var result = saveFileDialog.ShowDialog();
            if (result != DialogResult.OK || string.IsNullOrEmpty(saveFileDialog.FileName))
            {
                return;
            }

            if (Path.GetExtension(saveFileDialog.FileName) != ".dat")
            {
                saveFileDialog.FileName += ".dat";
            }

            txtPackageLocation.Text = saveFileDialog.FileName;
            btnCreate.Enabled = _assetsToInclude.Count > 0;

            _packageLocation = saveFileDialog.FileName;
            if (_createDefinitionFile && _automaticDefinitionFile)
            {
                _definitionsLocation = _packageLocation + ".txt";
                txtDefinitionsLocation.Text = _definitionsLocation;
            }
        }

        private void ChkCreateDefinitionFile_CheckedChanged(object sender, EventArgs e)
        {
            _createDefinitionFile = chkCreateDefinitionFile.Checked;
            splitContainerDefinitionFile.Enabled = _createDefinitionFile;
            txtDefinitionsLocation.Text = _createDefinitionFile ? _definitionsLocation : string.Empty;
        }

        private void ChkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        private string[] CollectFiles(string selectedPath)
        {
            Invoke(() =>
            {
                lblPercentage.Text = "0%";
                lblStatus.Text = "Collecting available files...";
                progressBarPrimary.Style = ProgressBarStyle.Marquee;
            });

            var files = DirectoryHelper.GetAllFiles(selectedPath,
                _cancellationTokenSource.Token).ToArray();

            if (files.Length == 0)
            {
                throw new Exception("The specified folder is empty.");
            }

            _assetsToInclude.Clear();

            Invoke(() =>
            {
                selectorTreeView.Nodes.Clear();
                lblAvailableItems.Text = "Available items: 0";
                lblSelectedItems.Text = "Selected items: 0";
                progressBarPrimary.Style = ProgressBarStyle.Blocks;
                lblStatus.Text = "Creating tree nodes...";
            });

            return files;
        }

        private void CreateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Dispose();
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

        private void CreateSelectorNodes(TreeNode rootNode, int relativeDepth, IReadOnlyList<string> files)
        {
            for (var i = 0; i < files.Count; i++)
            {
                _cancellationTokenSource.Token.ThrowIfCancellationRequested();

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

                _progressPrimary.Report(((int)((double)(i + 1) / files.Count * 100),
                    string.Join("/", pathNodes[relativeDepth..])));
            }
        }

        private void ExplorerTreeView_AfterStateChanged(object sender, TreeViewEventArgs e)
        {
            btnCreate.Enabled = _assetsToInclude.Count != 0 && _packageLocation != string.Empty;
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

        private void UpdateEncryptionProgress(int percentage)
        {
            if (_progressLastUpdatedSecondary >= DateTime.UtcNow)
            {
                return;
            }

            _progressLastUpdatedSecondary = DateTime.UtcNow + _progressTimeInterval;
            progressBarSecondary.Value = percentage;
        }

        private void UpdateFileCollectionProgress((int percentage, string path) progress)
        {
            if (_progressLastUpdatedPrimary >= DateTime.UtcNow)
            {
                return;
            }

            _progressLastUpdatedPrimary = DateTime.UtcNow + _progressTimeInterval;

            var (percentage, path) = progress;
            progressBarPrimary.Value = percentage;
            lblPercentage.Text = $"{percentage}%";
            lblPercentage.Refresh();
            lblStatusFile.Text = path;
            lblStatusFile.Refresh();
        }
    }
}