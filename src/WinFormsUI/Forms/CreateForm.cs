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

        private readonly List<PathEntry> _resourcesToInclude = new();
        private CancellationTokenSource _cancellationTokenSource;
        private string _definitionsLocation = string.Empty;
        private string _packageLocation = string.Empty;
        private string _assetsLocation = string.Empty;
        private bool _automaticDefinitionFile = true;
        private bool _createDefinitionFile = true;
        private int _relativePackageLocationDepth;
        private readonly IProgress<int> _progressPrimary;
        private readonly IProgress<int> _progressSecondary;
        private readonly IMediator _mediator;

        public CreateForm(IMediator mediator)
        {
            InitializeComponent();
            _progressPrimary = new Progress<int>(UpdateFileCollectionProgress);
            _progressSecondary = new Progress<int>(UpdateEncryptionProgress);
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationTokenSource.Cancel();

            _mediator = mediator;
        }

        private void BtnResourcesExplore_Click(object sender, EventArgs e)
        {
            using var browserDialog = new FolderBrowserDialog();
            var result = browserDialog.ShowDialog();

            if (result != DialogResult.OK || string.IsNullOrWhiteSpace(browserDialog.SelectedPath))
            {
                return;
            }

            _assetsLocation = browserDialog.SelectedPath;

            try
            {
                if (!Directory.EnumerateFileSystemEntries(_assetsLocation,
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
            txtAssetFolder.Text = _assetsLocation;

            var assetPathNodes = _assetsLocation
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
            Task.Run(async () =>
            {
                string[]? files = null;

                try
                {
                    files = CollectAvailableFiles(_assetsLocation);
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
                        var pathEntries = await _mediator.Send(pathEntriesQuery);

                        CreateSelectorNodes(rootNode, pathEntries);
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
                            btnCreate.Enabled = _packageLocation != string.Empty;

                            lblAvailableItems.Text = $"Available items: {_resourcesToInclude.Count}";
                            lblSelectedItems.Text = $"Selected items: {_resourcesToInclude.Count}";
                            this.FlashNotification();
                        });
                    }
                }
                catch (OperationCanceledException ex)
                {
                    Log.Information("The asset collection operation has been canceled.");
                    MessageBox.Show(ex.Message, "Operation canceled",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                if (!IsDisposed)
                {
                    Invoke(() =>
                    {
                        if (files?.Length == 0 || _cancellationTokenSource.IsCancellationRequested)
                        {
                            _resourcesToInclude.Clear();
                            btnCreate.Enabled = false;
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

            Task.Run(async () =>
            {
                var definitionsLocation = _createDefinitionFile ? _definitionsLocation : string.Empty;
                if (!string.IsNullOrEmpty(definitionsLocation))
                {
                    var exportDefinitionQuery = new ExportPathEntriesCommand(_resourcesToInclude, definitionsLocation)
                    {
                        Progress = _progressSecondary,
                        ProgressReportInterval = ReportInterval
                    };

                    await _mediator.Send(exportDefinitionQuery);
                }

                Invoke(() => lblStatus.Text = "Packing assets...");

                try
                {
                    var buildQuery = new BuildPackageCommand(_resourcesToInclude, _packageLocation, txtPassword.Text)
                    {
                        ProgressPrimary = _progressPrimary,
                        ProgressSecondary = _progressSecondary,
                        ProgressReportInterval = ReportInterval
                    };

                    await _mediator.Send(buildQuery);
                }
                catch (OperationCanceledException ex)
                {
                    Log.Information("The package construction operation has been cancelled.");
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

                Log.Information("Created new resource package: {path}", _packageLocation);
                Log.Information("Created new definitions file: {path}", _definitionsLocation);
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
            btnCreate.Enabled = enabled;
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
                FileName = Path.GetFileNameWithoutExtension(_assetsLocation) + ".dat"
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
            btnCreate.Enabled = _resourcesToInclude.Count > 0;

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

            _resourcesToInclude.Clear();

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
            var percentage = 0;

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
                            _resourcesToInclude.Add(pathEntries[i]);
                        }
                    }
                }

                percentage = (int)((double)(i + 1) / pathEntries.Count * 100);
            }
        }

        private void SelectorTreeView_AfterStateChanged(object sender, TreeViewEventArgs e)
        {
            btnCreate.Enabled = _resourcesToInclude.Count != 0 && _packageLocation != string.Empty;
        }

        private void SelectorTreeView_NodeStateChanged(object sender, TreeViewEventArgs e)
        {
            var node = e.Node;
            if (node is not { Tag: PathEntry })
            {
                return;
            }

            var item = (PathEntry)node.Tag;

            var hasItem = _resourcesToInclude.Contains(item);
            if (node.Checked)
            {
                if (!hasItem)
                {
                    _resourcesToInclude.Add(item);
                }
            }
            else if (hasItem)
            {
                _resourcesToInclude.Remove(item);
            }

            lblSelectedItems.Text = $"Selected items: {_resourcesToInclude.Count}";
        }

        private void UpdateEncryptionProgress(int percentage)
        {
            progressBarSecondary.Value = percentage;
        }

        private void UpdateFileCollectionProgress(int percentage)
        {
            progressBarPrimary.Value = percentage;
            lblPercentage.Text = $"{percentage}%";
            lblPercentage.Refresh();
        }

        private void CreateForm_SizeChanged(object sender, EventArgs e)
        {
            splitContainer2.SplitterDistance = Math.Min(splitContainer2.SplitterDistance,
                Width - splitContainer2.Panel2MinSize);
        }
    }
}