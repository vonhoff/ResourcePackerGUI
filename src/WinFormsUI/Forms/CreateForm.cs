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

using System.Media;
using MediatR;
using ResourcePackerGUI.Application.Packaging.Commands.BuildPackage;
using ResourcePackerGUI.Application.PathEntries.Commands.ExportPathEntries;
using ResourcePackerGUI.Application.PathEntries.Queries.CreatePathEntries;
using ResourcePackerGUI.Domain.Entities;
using Serilog;
using WinFormsUI.Extensions;
using WinFormsUI.Helpers;

namespace WinFormsUI.Forms
{
    public partial class CreateForm : Form
    {
        private const int ReportInterval = 25;

        private readonly List<PathEntry> _selectedPathEntries = new();
        private CancellationTokenSource _cancellationTokenSource;
        private string _definitionsLocation = string.Empty;
        private string _packageLocation = string.Empty;
        private string _resourcesLocation = string.Empty;
        private bool _automaticDefinitionFile = true;
        private bool _createDefinitionFile = true;
        private int _relativePackageLocationDepth;
        private readonly IProgress<float> _progressPrimary;
        private readonly IProgress<float> _progressSecondary;
        private readonly IMediator _mediator;
        private IReadOnlyList<PathEntry>? _pathEntries;

        public CreateForm(IMediator mediator)
        {
            _mediator = mediator;
            _progressPrimary = new Progress<float>(UpdateFileCollectionProgress);
            _progressSecondary = new Progress<float>(UpdateEncryptionProgress);
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationTokenSource.Cancel();
            InitializeComponent();
        }

        private void BtnResourcesExplore_Click(object sender, EventArgs e)
        {
            using var browserDialog = new FolderBrowserDialog();
            var result = browserDialog.ShowDialog();

            if (result != DialogResult.OK || string.IsNullOrWhiteSpace(browserDialog.SelectedPath))
            {
                return;
            }

            _resourcesLocation = browserDialog.SelectedPath;
            ReloadFilesFromSourceFolder();
        }

        private void ReloadFilesFromSourceFolder()
        {
            if (string.IsNullOrWhiteSpace(_resourcesLocation))
            {
                return;
            }

            Log.Information("Retrieving files from: {folder}", _resourcesLocation);

            try
            {
                if (!Directory.EnumerateFileSystemEntries(_resourcesLocation,
                        string.Empty, SearchOption.AllDirectories).Any())
                {
                    MessageBox.Show("The specified directory does not contain any files.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not open the specified directory or subdirectories. \n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnCancel.Text = "Cancel";
            txtAssetFolder.Text = _resourcesLocation;

            var pathNodes = _resourcesLocation
                .Replace(@"\", "/")
                .Split('/')
                .Where(n => n.Length > 0)
                .ToArray();

            _relativePackageLocationDepth = pathNodes.Length;
            var rootNode = new TreeNode(pathNodes.Last() + " (root)")
            {
                Checked = true,
                StateImageIndex = 1
            };

            _cancellationTokenSource = new CancellationTokenSource();
            Task.Run(async () =>
            {
                string[]? files = null;

                try
                {
                    files = CollectAvailableFiles(_resourcesLocation);
                    if (files.Length == 0)
                    {
                        MessageBox.Show("The specified directory does not contain any files.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        _cancellationTokenSource.Cancel();
                    }
                    else
                    {
                        if (IsDisposed)
                        {
                            return;
                        }

                        Log.Information("Collected {amount} items available for packaging.", files.Length);

                        var pathEntriesQuery = new CreatePathEntriesQuery(files, _relativePackageLocationDepth)
                        {
                            Progress = _progressSecondary,
                            ProgressReportInterval = ReportInterval
                        };

                        _pathEntries = await _mediator.Send(pathEntriesQuery, _cancellationTokenSource.Token);

                        CreateSelectorNodes(rootNode, _pathEntries);
                        _cancellationTokenSource.Token.ThrowIfCancellationRequested();

                        Invoke(() =>
                        {
                            lblStatusFile.Text = string.Empty;
                            lblStatus.Text = "Updating tree view...";
                            lblStatus.Refresh();

                            Cursor.Current = Cursors.WaitCursor;
                            selectorTreeView.BeginUpdate();
                            selectorTreeView.Nodes.Add(rootNode);
                            selectorTreeView.ExpandAll();
                            selectorTreeView.Nodes[0].EnsureVisible();
                            selectorTreeView.EndUpdate();
                            Cursor.Current = Cursors.Default;

                            btnCancel.Text = "Close";
                            lblStatus.Text = "Ready";
                            lblPercentage.Text = "0%";
                            progressBarPrimary.Value = 0;

                            SetBtnCreateState(_packageLocation != string.Empty);

                            lblAvailableItems.Text = $"Available items: {_selectedPathEntries.Count}";
                            lblSelectedItems.Text = $"Selected items: {_selectedPathEntries.Count}";
                            this.FlashNotification();
                        });
                    }
                }
                catch (OperationCanceledException ex)
                {
                    Log.Information(ex.Message);
                    MessageBox.Show(ex.Message, "Operation canceled",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                if (!IsDisposed)
                {
                    Invoke(() =>
                    {
                        if (files?.Length == 0 || _cancellationTokenSource.IsCancellationRequested)
                        {
                            _selectedPathEntries.Clear();
                            SetBtnCreateState(false);
                            txtAssetFolder.Text = string.Empty;
                            txtAssetFolder.Refresh();
                        }

                        lblStatusFile.Text = string.Empty;
                        btnCancel.Text = "Close";
                        lblStatus.Text = "Ready";
                        lblPercentage.Text = "0%";
                        progressBarPrimary.Style = ProgressBarStyle.Blocks;
                        progressBarPrimary.Value = 0;
                    });
                }

                _cancellationTokenSource.Cancel();
            });
        }

        private void SetBtnCreateState(bool enabled)
        {
            btnCreate.Enabled = enabled;
            if (enabled)
            {
                btnCreate.Focus();
            }
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

            btnCancel.Text = "Cancel";
            lblStatus.Text = "Creating definitions...";
            progressBarSecondary.Visible = true;
            SetConfigurationState(false);
            _pathEntries = _selectedPathEntries;

            Task.Run(async () =>
            {
                var definitionsLocation = _createDefinitionFile ? _definitionsLocation : string.Empty;
                if (!string.IsNullOrEmpty(definitionsLocation))
                {
                    var exportDefinitionQuery = new ExportPathEntriesCommand(_selectedPathEntries, definitionsLocation)
                    {
                        Progress = _progressSecondary,
                        ProgressReportInterval = ReportInterval
                    };

                    await _mediator.Send(exportDefinitionQuery, _cancellationTokenSource.Token);
                    Log.Information("Created new definitions file: {path}", _definitionsLocation);
                }

                Invoke(() => lblStatus.Text = "Packing resources...");

                try
                {
                    var buildQuery = new BuildPackageCommand(_selectedPathEntries, _packageLocation, txtPassword.Text)
                    {
                        ProgressPrimary = _progressPrimary,
                        ProgressSecondary = _progressSecondary,
                        ProgressReportInterval = ReportInterval
                    };

                    await _mediator.Send(buildQuery, _cancellationTokenSource.Token);
                    Log.Information("Created new resource package: {path}", _packageLocation);
                }
                catch (OperationCanceledException ex)
                {
                    Log.Information(ex.Message);
                    MessageBox.Show(ex.Message, "Operation canceled",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (!IsDisposed)
                    {
                        Invoke(() =>
                        {
                            lblStatus.Text = "Ready";
                            lblPercentage.Text = "0%";
                            lblStatusFile.Text = string.Empty;
                            progressBarPrimary.Value = 0;
                            progressBarSecondary.Visible = false;
                            btnCancel.Text = "Close";
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Could not build resource package. {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (!IsDisposed)
                {
                    Invoke(() => SetConfigurationState(true));
                }

                if (_cancellationTokenSource.IsCancellationRequested)
                {
                    CleanUpUnfinishedFiles();
                    return;
                }

                // Set every relevant labels and elements to completion.
                Invoke(() =>
                {
                    lblStatus.Text = "Done";
                    lblPercentage.Text = "100%";
                    lblStatusFile.Text = string.Empty;
                    progressBarPrimary.Value = 100;
                    progressBarSecondary.Visible = false;
                    btnCancel.Text = "Close";
                    btnCancel.Focus();
                });

                SystemSounds.Asterisk.Play();
                this.FlashNotification();
                _cancellationTokenSource.Cancel();
            });
        }

        private void CleanUpUnfinishedFiles()
        {
            try
            {
                if (File.Exists(_packageLocation))
                {
                    File.Delete(_packageLocation);
                    Log.Information("Unfinished package file deleted: {path}", _packageLocation);
                }

                if (File.Exists(_definitionsLocation))
                {
                    File.Delete(_definitionsLocation);
                    Log.Information("Unfinished definitions file deleted: {path}", _definitionsLocation);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An exception occurred during cleanup.");
            }
        }

        private void SetConfigurationState(bool enabled)
        {
            SetBtnCreateState(enabled);
            btnResourcesExplore.Enabled = enabled;
            btnDefinitionsExplore.Enabled = enabled;
            btnPackageExplore.Enabled = enabled;
            chkCreateDefinitionFile.Enabled = enabled;
            txtPassword.Enabled = enabled;
            selectorTreeView.ReadOnly = !enabled;
        }

        private void BtnDefinitionsExplore_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Text file (*.txt)|*.txt",
                FileName = Path.GetFileName(_packageLocation) + ".txt"
            };

            var result = saveFileDialog.ShowDialog();
            if (result != DialogResult.OK || string.IsNullOrEmpty(saveFileDialog.FileName))
            {
                return;
            }

            if (Path.GetExtension(saveFileDialog.FileName) != ".txt")
            {
                saveFileDialog.FileName += ".txt";
            }

            if (saveFileDialog.FileName.Equals(_definitionsLocation))
            {
                return;
            }

            _automaticDefinitionFile = false;
            _definitionsLocation = saveFileDialog.FileName;
            txtDefinitionsLocation.Text = saveFileDialog.FileName;
        }

        private void BtnPackageExplore_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "ResourcePack (*.dat)|*.dat",
                FileName = Path.GetFileNameWithoutExtension(_resourcesLocation) + ".dat"
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
            SetBtnCreateState(_selectedPathEntries.Count > 0);
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

        private string[] CollectAvailableFiles(string path)
        {
            Invoke(() =>
            {
                lblPercentage.Text = "0%";
                lblStatus.Text = "Collecting available files...";
                progressBarPrimary.Style = ProgressBarStyle.Marquee;
            });

            var files = DirectoryHelper.GetAllFiles(path,
                _cancellationTokenSource.Token).ToArray();

            if (files.Length == 0)
            {
                return files;
            }

            _selectedPathEntries.Clear();

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

        private void CreateSelectorNodes(TreeNode rootNode, IReadOnlyList<PathEntry> pathEntries)
        {
            var percentage = 0f;

            using var progressTimer = new System.Timers.Timer(ReportInterval);

            // ReSharper disable once AccessToModifiedClosure
            progressTimer.Elapsed += delegate { _progressPrimary.Report(percentage); };
            progressTimer.Enabled = true;

            for (var i = 0; i < pathEntries.Count; i++)
            {
                _cancellationTokenSource.Token.ThrowIfCancellationRequested();

                var relativePath = pathEntries[i].RelativePath;
                var currentNode = rootNode;

                var pathNodes = relativePath.Replace(@"\", "/").Split('/');
                for (var j = 0; j < pathNodes.Length; j++)
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
                            currentNode.Tag = pathEntries[i];
                            _selectedPathEntries.Add(pathEntries[i]);
                        }
                    }
                }

                percentage = (float)(i + 1) / pathEntries.Count * 100f;
            }
        }

        private void SelectorTreeView_AfterStateChanged(object sender, TreeViewEventArgs e)
        {
            SetBtnCreateState(_selectedPathEntries.Count != 0 && _packageLocation != string.Empty);
        }

        private void SelectorTreeView_NodeStateChanged(object sender, TreeViewEventArgs e)
        {
            var node = e.Node;
            if (node is not { Tag: PathEntry })
            {
                return;
            }

            var item = (PathEntry)node.Tag;

            var hasItem = _selectedPathEntries.Contains(item);
            if (node.Checked)
            {
                if (!hasItem)
                {
                    _selectedPathEntries.Add(item);
                }
            }
            else if (hasItem)
            {
                _selectedPathEntries.Remove(item);
            }

            lblSelectedItems.Text = $"Selected items: {_selectedPathEntries.Count}";
        }

        private void UpdateEncryptionProgress(float percentage)
        {
            progressBarSecondary.Value = (int)percentage;
        }

        private void UpdateFileCollectionProgress(float percentage)
        {
            progressBarPrimary.Value = (int)percentage;
            lblPercentage.Text = $"{Math.Round(percentage, 2)}%";
            lblPercentage.Refresh();

            if (_pathEntries == null)
            {
                return;
            }

            var index = (int)Math.Round((_pathEntries.Count - 1) / 100f * percentage);
            lblStatusFile.Text = _pathEntries[index].RelativePath;
            lblStatusFile.Refresh();
        }

        private void CreateForm_SizeChanged(object sender, EventArgs e)
        {
            splitContainer2.SplitterDistance = Math.Min(splitContainer2.SplitterDistance,
                Width - splitContainer2.Panel2MinSize);
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            ReloadFilesFromSourceFolder();
        }
    }
}