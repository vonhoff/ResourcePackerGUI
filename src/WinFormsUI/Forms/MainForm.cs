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

using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Xml;
using Be.Windows.Forms;
using MediatR;
using ResourcePackerGUI.Application.Common.Exceptions;
using ResourcePackerGUI.Application.Definitions.Queries.CreateChecksumDefinitions;
using ResourcePackerGUI.Application.Packaging.Queries.GetPackageInformation;
using ResourcePackerGUI.Application.Packaging.Queries.GetPackageResources;
using ResourcePackerGUI.Application.Resources.Commands.ExportResources;
using ResourcePackerGUI.Application.Resources.Queries.GetConflictingResources;
using ResourcePackerGUI.Application.Resources.Queries.UpdateResourceDefinitions;
using ResourcePackerGUI.Domain.Entities;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.RichTextBoxForms.Themes;
using WinFormsUI.Common;
using WinFormsUI.Extensions;
using WinFormsUI.Helpers;
using WinFormsUI.Properties;

namespace WinFormsUI.Forms
{
    public partial class MainForm : Form
    {
        private const int ReportInterval = 25;
        private static readonly Regex EscapedUnicodeRegex = new(@"\\[Uu]([0-9A-Fa-f]{4})", RegexOptions.Compiled);
        private readonly LoggingLevelSwitch _loggingLevelSwitch = new(LogEventLevel.Debug);
        private readonly IMediator _mediator;
        private readonly IProgress<int> _progressPrimary;
        private readonly IProgress<int> _progressSecondary;
        private readonly ActionDebouncer _scrollOutputToEndDebouncer;
        private readonly ActionDebouncer _searchDebouncer;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _displayOutputPanel = true;
        private bool _formatPreviewText = true;
        private bool _showDebugMessages = true;
        private string _packageName = string.Empty;
        private string _packagePath = string.Empty;
        private string _password = string.Empty;
        private string _searchQuery = string.Empty;
        private IReadOnlyList<Resource>? _resources;
        private Resource? _selectedPreviewAsset;

        public MainForm(IMediator mediator)
        {
            _mediator = mediator;
            _searchDebouncer = new ActionDebouncer(RefreshPackageExplorer, TimeSpan.FromMilliseconds(175));
            _scrollOutputToEndDebouncer = new ActionDebouncer(ScrollOutputToEnd, TimeSpan.FromMilliseconds(234));
            _progressPrimary = new Progress<int>(UpdateProgressPrimary);
            _progressSecondary = new Progress<int>(UpdateProgressSecondary);
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationTokenSource.Cancel();
            InitializeComponent();
        }

        private async Task AssignDefinitionsFromStream(Stream fileStream)
        {
            if (_resources == null || _resources.Count == 0)
            {
                return;
            }

            var definitionsQuery = new CreateChecksumDefinitionsQuery(fileStream)
            {
                Progress = _progressSecondary,
                ProgressReportInterval = ReportInterval
            };

            var crcDictionary = await _mediator.Send(definitionsQuery, _cancellationTokenSource.Token);
            var resourceUpdateQuery = new UpdateResourceDefinitionsQuery(_resources, crcDictionary)
            {
                Progress = _progressPrimary,
                ProgressReportInterval = ReportInterval
            };

            var updatedAmount = await _mediator.Send(resourceUpdateQuery, _cancellationTokenSource.Token);
            Invoke(() => btnLoadDefinitions.Enabled = _resources.Count != updatedAmount);
        }

        private void BtnAbout_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource.Cancel();
        }

        private void BtnClearConsole_Click(object sender, EventArgs e)
        {
            outputBox.Clear();
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            var createForm = new CreateForm(_mediator);
            createForm.ShowDialog();
        }

        private void BtnDisplayOutput_Click(object sender, EventArgs e)
        {
            _displayOutputPanel = !_displayOutputPanel;

            btnDisplayOutput.Image =
                _displayOutputPanel ? Images.checkbox_checked : Images.checkbox_unchecked;

            splitContainer2.Panel2Collapsed = !_displayOutputPanel;
        }

        private void BtnExportLogEntries_Click(object sender, EventArgs e)
        {
            if (outputBox.Lines.Length == 0)
            {
                return;
            }

            var fileName = "ResourcePacker_log_" + DateTime.Now.ToLocalTime();
            fileName = string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                FileName = fileName
            };

            var result = saveFileDialog.ShowDialog();
            if (result != DialogResult.OK || string.IsNullOrEmpty(saveFileDialog.FileName))
            {
                return;
            }

            try
            {
                outputBox.SaveFile(saveFileDialog.FileName, RichTextBoxStreamType.PlainText);
                Log.Information("Log entries exported to: {path}", saveFileDialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not export log entries. {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExtractAll_Click(object sender, EventArgs e)
        {
            if (_resources == null || _resources.Count == 0)
            {
                MessageBox.Show("Could not extract the current package, there are no resources available.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string baseExtractionPath;
            using (var browserDialog = new FolderBrowserDialog())
            {
                var result = browserDialog.ShowDialog();

                if (result != DialogResult.OK || string.IsNullOrWhiteSpace(browserDialog.SelectedPath))
                {
                    return;
                }

                baseExtractionPath = Path.Join(browserDialog.SelectedPath, _packageName);
            }

            ExportResourcesToFolder(baseExtractionPath, _resources);
        }

        private void BtnExtractSelected_Click(object sender, EventArgs e)
        {
            var selectedResources = packageExplorerTreeView.GetSelectedResources();

            if (selectedResources.Count == 0)
            {
                MessageBox.Show("No resources are selected.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string baseExtractionPath;
            using (var browserDialog = new FolderBrowserDialog())
            {
                var result = browserDialog.ShowDialog();
                if (result != DialogResult.OK || string.IsNullOrWhiteSpace(browserDialog.SelectedPath))
                {
                    return;
                }

                // Append the package name to the extraction path when the root node is selected.
                baseExtractionPath = packageExplorerTreeView.SelectedNodes.Contains(packageExplorerTreeView.Nodes.First()) ?
                    Path.Join(browserDialog.SelectedPath, _packageName) :
                    browserDialog.SelectedPath;
            }

            ExportResourcesToFolder(baseExtractionPath, selectedResources);
        }

        private void BtnFormattedText_Click(object sender, EventArgs e)
        {
            _formatPreviewText = !_formatPreviewText;

            btnFormattedText.Image =
                _formatPreviewText ? Images.checkbox_checked : Images.checkbox_unchecked;

            if (previewTabs.SelectedTab == previewTextTab)
            {
                SetTextPreviewValue();
            }
        }

        private void BtnLoadDefinitions_Click(object sender, EventArgs e)
        {
            if (_resources == null || _resources.Count == 0)
            {
                MessageBox.Show("Could not load definitions, there are no resources available.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text file (*.txt)|*.txt";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var fileStream = openFileDialog.OpenFile();
            Task.Run(async () =>
            {
                await AssignDefinitionsFromStream(fileStream);
                RefreshPackageExplorer();
                DisplayResourceCount();
            });
        }

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            using var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "ResourcePack (*.dat)|*.dat";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var stream = openFileDialog.OpenFile();
            var binaryReader = new BinaryReader(stream);
            _packagePath = openFileDialog.FileName;
            _packageName = Path.GetFileNameWithoutExtension(_packagePath);

            lblStatus.Text = _packagePath;
            lblElapsed.Text = "00:00:00.0000";
            lblResultCount.Text = "0 Resources";
            _cancellationTokenSource = new CancellationTokenSource();

            Task.Run(async () =>
            {
                var hideCancellationDialog = false;

                try
                {
                    var query = new GetPackageInformationQuery(binaryReader)
                    {
                        Progress = _progressSecondary,
                        ProgressReportInterval = ReportInterval
                    };

                    var packageInformation = await _mediator.Send(query, _cancellationTokenSource.Token);

                    _password = string.Empty;

                    if (packageInformation.Encrypted)
                    {
                        var passwordDialog = new PasswordForm();

                        Invoke(() =>
                        {
                            if (passwordDialog.ShowDialog() != DialogResult.OK)
                            {
                                hideCancellationDialog = true;
                                _cancellationTokenSource.Cancel();
                                throw new OperationCanceledException();
                            }
                        });

                        if (string.IsNullOrEmpty(passwordDialog.Password))
                        {
                            throw new InvalidPasswordException();
                        }

                        _password = passwordDialog.Password;
                    }

                    SetToolbarState(true);

                    // Retrieve all resources
                    var stopwatch = Stopwatch.StartNew();
                    var resourceRetrievalQuery =
                        new GetPackageResourcesQuery(packageInformation.Entries, binaryReader, _password)
                        {
                            ProgressPrimary = _progressPrimary,
                            ProgressSecondary = _progressSecondary,
                            ProgressReportInterval = ReportInterval
                        };

                    _resources = await _mediator.Send(resourceRetrievalQuery, _cancellationTokenSource.Token);

                    // Check for a candidate definition file to automatically assign.
                    var candidateDefinitionsFile = _packagePath + ".txt";

                    if (File.Exists(candidateDefinitionsFile))
                    {
                        Log.Information("Attempting to create name definitions from: {path}", candidateDefinitionsFile);
                        var definitionsFileStream = File.OpenRead(candidateDefinitionsFile);
                        await AssignDefinitionsFromStream(definitionsFileStream);
                    }
                    else
                    {
                        Invoke(() => btnLoadDefinitions.Enabled = true);
                    }

                    // Stop stopwatch and refresh the file tree.
                    stopwatch.Stop();
                    RefreshPackageExplorer();

                    Invoke(() =>
                    {
                        DisplayResourceCount();
                        lblElapsed.Text = stopwatch.Elapsed.ToString(@"hh\:mm\:ss\.ffff");
                        ResetPreviewData();
                    });
                }
                catch (OperationCanceledException ex)
                {
                    Log.Information(ex.Message);
                    if (!hideCancellationDialog)
                    {
                        MessageBox.Show(ex.Message, "Operation canceled",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (InvalidPasswordException)
                {
                    Log.Error("The password entered is incorrect.");
                    MessageBox.Show("The password entered is incorrect.",
                        "Incorrect password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "An exception occurred while opening the specified package.");
                    MessageBox.Show($"Could not open resource package. {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                this.FlashNotification();
                binaryReader.Close();

                Invoke(() =>
                {
                    lblStatus.Text = string.Empty;
                    SetToolbarState(false);
                    DisplayResourceCount();
                    ResetProgressBars();
                });
            });
        }

        private void ResetPreviewData()
        {
            _selectedPreviewAsset = null;
            ReloadPreviewTab(null);
            previewTabs.SelectedTab = previewHexTab;
        }

        private void DisplayResourceCount()
        {
            var count = _resources?.Count ?? 0;
            lblResultCount.Text = $"{count} " + (count == 1 ? "Resource" : "Resources");
        }

        private void SetToolbarState(bool processing)
        {
            btnCancel.Visible = processing;
            btnCreate.Visible = !processing;
            btnOpen.Visible = !processing;
            toolStripSeparator3.Visible = !processing;
            btnLoadDefinitions.Visible = !processing;
            btnExtractAll.Visible = !processing;
            btnExtractSelected.Visible = !processing;
        }

        private void BtnToggleDebugMessages_Click(object sender, EventArgs e)
        {
            _showDebugMessages = !_showDebugMessages;

            btnToggleDebugMessages.Image =
                _showDebugMessages ? Images.checkbox_checked : Images.checkbox_unchecked;

            _loggingLevelSwitch.MinimumLevel =
                _showDebugMessages ? LogEventLevel.Debug : LogEventLevel.Information;
        }

        private void ExportResourcesToFolder(string baseExtractionPath, IReadOnlyList<Resource> resources)
        {
            SetToolbarState(true);
            _cancellationTokenSource = new CancellationTokenSource();

            Task.Run(async () =>
            {
                try
                {
                    var pathReplacements = await GetFileConflictPathReplacements(baseExtractionPath, resources);

                    var exportResourcesCommand = new ExportResourcesCommand(baseExtractionPath, resources, pathReplacements)
                    {
                        Progress = _progressPrimary,
                        ProgressReportInterval = ReportInterval
                    };

                    await _mediator.Send(exportResourcesCommand, _cancellationTokenSource.Token);

                    Invoke(() =>
                    {
                        this.FlashNotification();
                        var extractionCompleteDialogResult =
                            MessageBox.Show("The resources have been successfully extracted to the destination folder. \n" +
                                            "Do you want to view the extracted resources?",
                                "Extraction completed", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);

                        if (extractionCompleteDialogResult == DialogResult.Yes)
                        {
                            Process.Start(new ProcessStartInfo
                            {
                                FileName = baseExtractionPath,
                                UseShellExecute = true,
                                WindowStyle = ProcessWindowStyle.Normal,
                                Verb = "open"
                            });
                        }
                    });
                }
                catch (OperationCanceledException ex)
                {
                    Log.Information(ex.Message);
                    MessageBox.Show(ex.Message, "Operation canceled",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "An exception occurred during extraction.");
                }

                Invoke(() =>
                {
                    SetToolbarState(false);
                    DisplayResourceCount();
                    ResetProgressBars();
                });
            });
        }

        private async Task<Dictionary<Resource, string>> GetFileConflictPathReplacements(
            string baseExtractionPath, IReadOnlyList<Resource> resources)
        {
            var conflictingResourcesQuery = new GetConflictingResourcesQuery(baseExtractionPath, resources)
            {
                Progress = _progressSecondary,
                ProgressReportInterval = ReportInterval
            };

            var resolved = 0;
            var conflictingResources = await _mediator.Send(conflictingResourcesQuery, _cancellationTokenSource.Token);
            var pathReplacements = new Dictionary<Resource, string>();

            if (conflictingResources.Count == 0)
            {
                return pathReplacements;
            }

            var lastAction = DialogResult.Ignore;
            var performLastForAllCases = false;
            foreach (var resource in conflictingResources)
            {
                var filePath = Path.Join(baseExtractionPath, resource.Name).Replace("/", "\\");
                var nextAvailablePath = FilenameHelper.NextAvailablePath(filePath);

                Invoke(() =>
                {
                    var replacementDialog = new ReplaceDialog(filePath, nextAvailablePath,
                        conflictingResources.Count - resolved);

                    if (!performLastForAllCases)
                    {
                        lastAction = replacementDialog.ShowDialog();
                        performLastForAllCases = replacementDialog.UseForAllCases;
                    }

                    switch (lastAction)
                    {
                        case DialogResult.OK:
                            pathReplacements.Add(resource, filePath);
                            resolved++;
                            break;

                        case DialogResult.Continue:
                            pathReplacements.Add(resource, nextAvailablePath);
                            resolved++;
                            break;

                        case DialogResult.Ignore:
                            resolved++;
                            break;

                        default:
                            _cancellationTokenSource.Cancel();
                            throw new OperationCanceledException();
                    }
                });
            }

            return pathReplacements;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.RichTextBox(outputBox, theme: ThemePresets.Light,
                    levelSwitch: _loggingLevelSwitch, messageBatchSize: 500)
                .CreateLogger();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            _scrollOutputToEndDebouncer.Invoke();
        }

        private void OutputBox_TextChanged(object sender, EventArgs e)
        {
            lblLogEntries.Text = $"Log entries: {outputBox.Lines.Length}";
            btnExportLogEntries.Enabled = outputBox.Lines.Length > 0;
            btnClearConsole.Enabled = outputBox.Lines.Length > 0;
        }

        private void PackageExplorerTreeView_Leave(object sender, EventArgs e)
        {
            UpdateExtractSelectedButton();
        }

        private void PackageExplorerTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            UpdateExtractSelectedButton();
        }

        private void PackageExplorerTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag == null)
            {
                return;
            }

            _selectedPreviewAsset = (Resource)e.Node.Tag;

            switch (_selectedPreviewAsset.MediaType)
            {
                case { PrimaryType: "image" }:
                {
                    previewTabs.SelectedTab = previewImageTab;
                    break;
                }
                case { SubType: "xml" }:
                case { SubType: "json" }:
                case { PrimaryType: "text" }:
                {
                    previewTabs.SelectedTab = previewTextTab;
                    break;
                }
                default:
                {
                    previewTabs.SelectedTab = previewHexTab;
                    break;
                }
            }

            ReloadPreviewTab(previewTabs.SelectedTab);
        }

        private void PreviewTabs_Selecting(object sender, TabControlCancelEventArgs e)
        {
            ReloadPreviewTab(e.TabPage);
        }

        private void RefreshPackageExplorer()
        {
            if (_resources == null)
            {
                return;
            }

            var resources = _resources.Where(a => a.Name.Contains(_searchQuery)).ToList();

            Invoke(() =>
            {
                lblNoResults.Visible = resources.Count == 0;
                if (resources.Count == 0)
                {
                    packageExplorerTreeView.Clear();
                }
            });

            packageExplorerTreeView.CreateNodesFromResources(resources, _packageName,
                _progressSecondary, ReportInterval);

            Invoke(() =>
            {
                ResetPreviewData();
                ResetProgressBars();
                UpdateExtractSelectedButton();
            });
        }

        /// <summary>
        /// Prepares the preview tab with the corresponding values.
        /// </summary>
        /// <param name="tabPage">The tab to initialize.</param>
        private void ReloadPreviewTab(TabPage? tabPage)
        {
            // Reset values.
            previewHexBox.ByteProvider = null;
            previewImageBox.Image = null;
            previewTextBox.Clear();

            // Set status labels.
            lblFileName.Text = $"Name: {(string.IsNullOrWhiteSpace(_selectedPreviewAsset?.Name) ? "n/a" : Path.GetFileName(_selectedPreviewAsset.Name))}";
            lblMediaType.Text = $"Media type: {_selectedPreviewAsset?.MediaType?.Name ?? "n/a"}";
            lblDataSize.Text = _selectedPreviewAsset?.Entry.DataSize switch
            {
                > 1000000000 => $"Size: {_selectedPreviewAsset.Entry.DataSize / 1000000000} GB",
                > 1000000 => $"Size: {_selectedPreviewAsset.Entry.DataSize / 1000000} MB",
                > 1000 => $"Size: {_selectedPreviewAsset.Entry.DataSize / 1000} KB",
                > 0 => $"Size: {_selectedPreviewAsset.Entry.DataSize} bytes",
                _ => "Size: 0 bytes"
            };

            // Set values for the corresponding tab.
            if (tabPage == previewTextTab)
            {
                SetTextPreviewValue();
            }
            else if (tabPage == previewHexTab)
            {
                SetHexPreviewValue();
            }
            else
            {
                SetImagePreviewValue();
            }
        }

        private void ResetProgressBars()
        {
            progressBarPrimary.Value = 0;
            progressBarSecondary.Value = 0;
        }

        private void ScrollOutputToEnd()
        {
            outputBox.ScrollToBottom();
        }

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            _searchQuery = ((TextBox)sender).Text;
            _searchDebouncer.Invoke();
        }

        /// <summary>
        /// Sets the hex preview to the data from <see cref="_selectedPreviewAsset"/>.
        /// </summary>
        private void SetHexPreviewValue()
        {
            if (_selectedPreviewAsset?.Data == null)
            {
                return;
            }

            previewHexBox.ByteProvider = new DynamicByteProvider(_selectedPreviewAsset.Data);
        }

        /// <summary>
        /// Sets the image preview to the data from <see cref="_selectedPreviewAsset"/>.
        /// </summary>
        private void SetImagePreviewValue()
        {
            if (_selectedPreviewAsset?.MediaType?.PrimaryType != "image")
            {
                previewImageBox.Text = "No image available.";
                return;
            }

            previewImageBox.Text = string.Empty;

            try
            {
                var memoryStream = new MemoryStream();
                memoryStream.Write(_selectedPreviewAsset.Data, 0,
                    _selectedPreviewAsset.Data.Length);

                var bitmap = new Bitmap(memoryStream, false);
                memoryStream.Dispose();
                previewImageBox.Image = bitmap;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Could not convert byte array to bitmap.");
            }
        }

        /// <summary>
        /// Sets the text preview to the data from <see cref="_selectedPreviewAsset"/>.
        /// </summary>
        private void SetTextPreviewValue()
        {
            if (_selectedPreviewAsset?.Data == null)
            {
                return;
            }

            var text = Encoding.UTF8.GetString(_selectedPreviewAsset.Data);

            // Remove any format characters.
            text = new string(text.Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.Format).ToArray());

            if (!_formatPreviewText)
            {
                previewTextBox.Text = text;
                return;
            }

            switch (_selectedPreviewAsset.MediaType)
            {
                case { SubType: "json" }:
                {
                    var jsonNode = JsonNode.Parse(text);
                    if (jsonNode == null)
                    {
                        previewTextBox.Text = text;
                        return;
                    }

                    var options = new JsonSerializerOptions { WriteIndented = true };
                    var jsonText = jsonNode.ToJsonString(options);
                    text = EscapedUnicodeRegex.Replace(jsonText, m =>
                        char.ToString((char)ushort.Parse(m.Groups[1].Value, NumberStyles.AllowHexSpecifier)));
                    previewTextBox.Text = text;
                    break;
                }
                case { SubType: "xml" }:
                {
                    var xmlDocument = new XmlDocument();
                    try
                    {
                        xmlDocument.LoadXml(text);
                        using var stringWriter = new StringWriter();
                        var xmlTextWriter = new XmlTextWriter(stringWriter)
                        {
                            Formatting = Formatting.Indented
                        };

                        xmlDocument.WriteTo(xmlTextWriter);
                        previewTextBox.Text = stringWriter.ToString();
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Could not parse text to XML.");
                        previewTextBox.Text = text;
                    }

                    break;
                }
                default:
                {
                    previewTextBox.Text = text;
                    break;
                }
            }
        }

        private void SplitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {
            ScrollOutputToEnd();
        }

        private void UpdateExtractSelectedButton()
        {
            var selectedNodeCount = packageExplorerTreeView.SelectedNodes.Count;
            btnExtractSelected.Enabled = selectedNodeCount > 0;
            btnExtractSelected.Text = "Extract selected";
            if (selectedNodeCount > 0)
            {
                btnExtractSelected.Text += $" ({selectedNodeCount})";
            }
        }

        private void UpdateProgressPrimary(int percentage)
        {
            progressBarPrimary.Value = percentage;
            lblResultCount.Text = $"Progress: {percentage}%";
        }

        private void UpdateProgressSecondary(int percentage)
        {
            progressBarSecondary.Value = percentage;
        }
    }
}