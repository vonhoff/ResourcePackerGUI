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

using System.Diagnostics;
using System.Media;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Xml;
using Be.Windows.Forms;
using ResourcePacker.Common;
using ResourcePacker.Entities;
using ResourcePacker.Extensions;
using ResourcePacker.Helpers;
using ResourcePacker.Properties;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.RichTextBoxForms.Themes;

namespace ResourcePacker.Forms
{
    public partial class MainForm : Form
    {
        private const int ProgressReportInterval = 25;
        private readonly LoggingLevelSwitch _loggingLevelSwitch = new(LogEventLevel.Debug);
        private readonly IProgress<(int percentage, int amount)> _progressPrimary;
        private readonly IProgress<int> _progressSecondary;
        private readonly ActionDebouncer _scrollOutputToEndDebouncer;
        private readonly ActionDebouncer _searchDebouncer;
        private List<Asset>? _assets;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _displayOutputPanel = true;
        private bool _formatPreviewText = true;
        private PackageHeader _packageHeader;
        private string _packagePath = string.Empty;
        private string _password = string.Empty;
        private string _searchQuery = string.Empty;
        private Asset? _selectedPreviewAsset;
        private bool _showDebugMessages = true;
        private string _packageName = string.Empty;

        public MainForm()
        {
            InitializeComponent();
            _searchDebouncer = new ActionDebouncer(RefreshPackageExplorer, TimeSpan.FromMilliseconds(175));
            _scrollOutputToEndDebouncer = new ActionDebouncer(ScrollOutputToEnd, TimeSpan.FromMilliseconds(234));
            _progressPrimary = new Progress<(int percentage, int amount)>(UpdateLoadProgress);
            _progressSecondary = new Progress<int>(UpdateDecryptionProgress);
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationTokenSource.Cancel();
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
            var createForm = new CreateForm();
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
            if (_assets == null)
            {
                MessageBox.Show("A resource package must be selected before a definition source can be set.",
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
            var crcDictionary = DefinitionHelper.CreateCrcDictionary(fileStream);
            var matches = AssetHelper.UpdateAssetsWithDefinitions(_assets, crcDictionary);
            btnLoadDefinitions.Enabled = matches != _assets.Count;
            RefreshPackageExplorer();
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
            try
            {
                _packageHeader = PackageHelper.GetHeader(binaryReader);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not open resource package. {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Log.Information("ResourcePackage: {@info}",
                new { _packageHeader.Id, _packageHeader.NumberOfEntries });

            _packagePath = openFileDialog.FileName;
            lblStatus.Text = _packagePath;
            lblElapsed.Text = "00:00:00.0000";

            _cancellationTokenSource = new CancellationTokenSource();
            var stopwatch = Stopwatch.StartNew();
            Task.Run(() =>
            {
                try
                {
                    var entries = PackageHelper.LoadAllEntryInformation(_packageHeader, binaryReader);
                    _password = string.Empty;

                    // Get the smallest entry for faster integrity checking.
                    var smallestEntry = entries.Aggregate((c, d) =>
                        (c.DataSize < d.DataSize && c.DataSize > 0) ? c : d);

                    // Try to load the first asset to check whether the package is encrypted.
                    if (!PackageHelper.LoadSingleFromPackage(binaryReader, smallestEntry, out _,
                            progress: _progressSecondary))
                    {
                        var passwordDialog = new PasswordForm();
                        if (passwordDialog.ShowDialog() != DialogResult.OK)
                        {
                            _cancellationTokenSource.Cancel(true);
                        }

                        _password = passwordDialog.Password;
                        if (!PackageHelper.LoadSingleFromPackage(binaryReader, smallestEntry, out _,
                                _password, _progressSecondary))
                        {
                            throw new InvalidCredentialException();
                        }
                    }

                    // Show the cancel button, hide the create and open button.
                    Invoke(() =>
                    {
                        btnCancel.Visible = true;
                        btnCreate.Visible = false;
                        btnOpen.Visible = false;

                        btnLoadDefinitions.Enabled = false;
                        btnExtractSelected.Enabled = false;
                        btnExtractAll.Enabled = false;
                    });

                    // Load all assets from package.
                    _assets = PackageHelper.LoadAssetsFromPackage(entries, binaryReader, _password,
                        _progressPrimary, _progressSecondary, ProgressReportInterval, _cancellationTokenSource.Token);

                    var candidateDefinitionsFile = _packagePath + ".txt";
                    var matches = 0;
                    if (File.Exists(candidateDefinitionsFile))
                    {
                        Log.Information("Attempting to create name definitions from: {path}", candidateDefinitionsFile);
                        var definitionsFileStream = File.OpenRead(candidateDefinitionsFile);
                        var crcDictionary = DefinitionHelper.CreateCrcDictionary(definitionsFileStream);
                        matches = AssetHelper.UpdateAssetsWithDefinitions(_assets, crcDictionary);
                    }

                    // Stop stopwatch and refresh file tree.
                    stopwatch.Stop();
                    RefreshPackageExplorer();

                    // When succeeded, show the action buttons.
                    Invoke(() =>
                    {
                        lblResultCount.Text = $"{_assets.Count} " + (_assets.Count > 1 ? "Assets" : "Asset");
                        lblElapsed.Text = stopwatch.Elapsed.ToString(@"hh\:mm\:ss\.ffff");
                        btnLoadDefinitions.Enabled = matches != _assets.Count;
                        btnExtractAll.Enabled = true;
                    });
                }
                catch (OperationCanceledException ex)
                {
                    Log.Information("The package loading operation has been cancelled.");

                    Invoke(() =>
                    {
                        lblResultCount.Text = "0 Assets";
                        lblElapsed.Text = "00:00:00.0000";
                    });

                    if (!string.IsNullOrEmpty(_password))
                    {
                        MessageBox.Show(ex.Message,
                            "Operation canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (InvalidCredentialException)
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

                binaryReader.Close();
                Invoke(() =>
                {
                    // Clear the processing status.
                    lblStatus.Text = string.Empty;

                    // Hide cancel button, show the create and open button.
                    btnCancel.Visible = false;
                    btnCreate.Visible = true;
                    btnOpen.Visible = true;

                    // Set the progress bars to their initial state.
                    progressBarPrimary.Style = ProgressBarStyle.Blocks;
                    progressBarPrimary.Value = 0;
                    progressBarSecondary.Style = ProgressBarStyle.Blocks;
                    progressBarSecondary.Value = 0;

                    // Create a flash notification.
                    this.FlashNotification();
                });
            });
        }

        private void BtnToggleDebugMessages_Click(object sender, EventArgs e)
        {
            _showDebugMessages = !_showDebugMessages;

            btnToggleDebugMessages.Image =
                _showDebugMessages ? Images.checkbox_checked : Images.checkbox_unchecked;

            _loggingLevelSwitch.MinimumLevel =
                _showDebugMessages ? LogEventLevel.Debug : LogEventLevel.Information;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.RichTextBox(outputBox, theme: ThemePresets.Light,
                    levelSwitch: _loggingLevelSwitch)
                .CreateLogger();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            _scrollOutputToEndDebouncer.Invoke();
        }

        private void MainForm_ResizeBegin(object sender, EventArgs e)
        {
            SuspendLayout();
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            ResumeLayout();
        }

        private void OutputBox_TextChanged(object sender, EventArgs e)
        {
            lblLogEntries.Text = $"Log entries: {outputBox.Lines.Length}";
            btnExportLogEntries.Enabled = outputBox.Lines.Length > 0;
            btnClearConsole.Enabled = outputBox.Lines.Length > 0;
        }

        private void PackageExplorerTreeView_Leave(object sender, EventArgs e)
        {
            UpdateExtractButton();
        }

        private void PackageExplorerTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            UpdateExtractButton();
        }

        private void PackageExplorerTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag == null)
            {
                return;
            }

            _selectedPreviewAsset = (Asset)e.Node.Tag;

            switch (_selectedPreviewAsset.MimeType)
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
            if (_assets == null)
            {
                return;
            }

            _packageName = Path.GetFileNameWithoutExtension(_packagePath);
            var filteredAssets = _assets.Where(a => a.Name.Contains(_searchQuery)).ToList();

            Invoke(() =>
            {
                lblNoResults.Visible = filteredAssets.Count == 0;
                if (filteredAssets.Count == 0)
                {
                    packageExplorerTreeView.Clear();
                }
            });

            Task.Run(() =>
            {
                packageExplorerTreeView.CreateNodesFromAssets(filteredAssets, _packageName,
                    _progressSecondary, ProgressReportInterval);

                Invoke(() =>
                {
                    progressBarSecondary.Value = 0;
                    UpdateExtractButton();
                });
            });
        }

        /// <summary>
        /// Prepares the preview tab with the corresponding values.
        /// </summary>
        /// <param name="tabPage">The tab to initialize.</param>
        private void ReloadPreviewTab(TabPage? tabPage)
        {
            // Clear all previews.
            previewHexBox.ByteProvider = null;
            previewImageBox.Image = null;
            previewTextBox.Clear();

            if (_selectedPreviewAsset?.Data == null)
            {
                return;
            }

            // Set status labels.
            lblMediaType.Text = $"Media type: {_selectedPreviewAsset.MimeType?.Name ?? "n/a"}";
            lblDataSize.Text = _selectedPreviewAsset.Entry.DataSize switch
            {
                > 1000000000 => $"Size: {_selectedPreviewAsset.Entry.DataSize / 1000000000} GB",
                > 1000000 => $"Size: {_selectedPreviewAsset.Entry.DataSize / 1000000} MB",
                > 1000 => $"Size: {_selectedPreviewAsset.Entry.DataSize / 1000} KB",
                _ => $"Size: {_selectedPreviewAsset.Entry.DataSize} bytes"
            };

            // Set values for the corresponding tab.
            if (tabPage == previewImageTab)
            {
                SetImagePreviewValue();
            }
            else if (tabPage == previewTextTab)
            {
                SetTextPreviewValue();
            }
            else if (tabPage == previewHexTab)
            {
                SetHexPreviewValue();
            }
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
            if (_selectedPreviewAsset?.Data == null)
            {
                return;
            }

            if (_selectedPreviewAsset.MimeType?.PrimaryType != "image")
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
            text = Regex.Replace(text, @"[^\t\r\n -~]", string.Empty);

            if (!_formatPreviewText)
            {
                previewTextBox.Text = text;
                return;
            }

            switch (_selectedPreviewAsset.MimeType)
            {
                case { SubType: "json" }:
                {
                    var json = JsonNode.Parse(text);
                    var options = new JsonSerializerOptions { WriteIndented = true };
                    previewTextBox.Text = json!.ToJsonString(options);
                    break;
                }
                case { SubType: "xml" }:
                {
                    var xmlDocument = new XmlDocument();
                    try
                    {
                        xmlDocument.LoadXml(text);
                        var stringWriter = new StringWriter();
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

        private void UpdateDecryptionProgress(int percentage)
        {
            if (progressBarSecondary == null)
            {
                return;
            }

            progressBarSecondary.Value = percentage;
            progressBarSecondary.Style = ProgressBarStyle.Blocks;
        }

        private void UpdateExtractButton()
        {
            var selectedNodeCount = packageExplorerTreeView.SelectedNodes.Count;
            btnExtractSelected.Enabled = selectedNodeCount > 0;
            btnExtractSelected.Text = "Extract selected";
            if (selectedNodeCount > 0)
            {
                btnExtractSelected.Text += $" ({selectedNodeCount})";
            }
        }

        private void UpdateLoadProgress((int percentage, int amount) progress)
        {
            var (percentage, amount) = progress;
            progressBarPrimary.Value = percentage;
            lblResultCount.Text = $"{amount} " + (amount > 1 ? "Assets" : "Asset");
        }

        private void BtnExtractAll_Click(object sender, EventArgs e)
        {
            if (_assets is { Count: 0 })
            {
                MessageBox.Show("Could not extract the current package, there are no assets available.",
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

                baseExtractionPath = browserDialog.SelectedPath + Path.DirectorySeparatorChar + _packageName;
            }

            _cancellationTokenSource = new CancellationTokenSource();
            Task.Run(() =>
            {
                var previousBtnLoadDefinitionsState = btnLoadDefinitions.Enabled;
                var previousBtnExtractAllState = btnExtractAll.Enabled;

                Invoke(() =>
                {
                    btnCancel.Visible = true;
                    btnCreate.Visible = false;
                    btnOpen.Visible = false;

                    btnLoadDefinitions.Enabled = false;
                    btnExtractAll.Enabled = false;
                });

                try
                {
                    if (Directory.Exists(baseExtractionPath) &&
                        Directory.EnumerateFileSystemEntries(baseExtractionPath,
                            string.Empty, SearchOption.AllDirectories).Any())
                    {
                        var existsDialogResult = MessageBox.Show($"The destination already contains a folder named '{_packageName}' which is not empty. " +
                                                           "If there are files with the same name, you will be asked if you want to replace these files. \n\n" +
                                                           "Do you want to continue extracting to this folder?",
                                                           "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                        if (existsDialogResult != DialogResult.Yes)
                        {
                            _cancellationTokenSource.Cancel(true);
                        }
                    }

                    var extractedItems = AssetHelper.ExtractAssetsToLocation(_assets!, baseExtractionPath,
                        _progressPrimary, ProgressReportInterval, _cancellationTokenSource.Token);

                    if (extractedItems == 0)
                    {
                        MessageBox.Show("None of the assets could be extracted from the package. \n" +
                                        "All assets either failed to extract or were ignored.",
                            "Extraction completed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        return;
                    }

                    SystemSounds.Asterisk.Play();
                    this.FlashNotification();

                    var extractionCompleteDialogResult =
                        MessageBox.Show("The assets have been successfully extracted to the destination folder. \n" +
                                        "Do you want to view the extracted assets?",
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
                }
                catch (OperationCanceledException ex)
                {
                    Log.Information("The package extraction operation has been canceled.");

                    Invoke(() =>
                    {
                        lblResultCount.Text = "0 Assets";
                        lblElapsed.Text = "00:00:00.0000";
                    });

                    MessageBox.Show(ex.Message, "Operation canceled",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "An exception occurred during extraction.");
                }

                Invoke(() =>
                {
                    btnCancel.Visible = false;
                    btnCreate.Visible = true;
                    btnOpen.Visible = true;

                    btnLoadDefinitions.Enabled = previousBtnLoadDefinitionsState;
                    btnExtractAll.Enabled = previousBtnExtractAllState;

                    // Set the progress bars to their initial state.
                    progressBarPrimary.Style = ProgressBarStyle.Blocks;
                    progressBarPrimary.Value = 0;
                });
            });
        }
    }
}