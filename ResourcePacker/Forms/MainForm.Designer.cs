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

namespace ResourcePacker.Forms
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.searchBox = new System.Windows.Forms.TextBox();
            this.lblNoResults = new System.Windows.Forms.Label();
            this.packageExplorerTreeView = new ResourcePacker.Controls.MultiNodeSelectionTreeView();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.previewTabs = new System.Windows.Forms.TabControl();
            this.previewHexTab = new System.Windows.Forms.TabPage();
            this.previewHexBox = new Be.Windows.Forms.HexBox();
            this.previewTextTab = new System.Windows.Forms.TabPage();
            this.previewTextBox = new System.Windows.Forms.RichTextBox();
            this.previewImageTab = new System.Windows.Forms.TabPage();
            this.previewImageBox = new Cyotek.Windows.Forms.ImageBox();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.lblDataSize = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lblMediaType = new System.Windows.Forms.ToolStripLabel();
            this.btnFormattedText = new System.Windows.Forms.ToolStripButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.outputBox = new System.Windows.Forms.RichTextBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.btnToggleDebugMessages = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnClearConsole = new System.Windows.Forms.ToolStripButton();
            this.lblLogEntries = new System.Windows.Forms.ToolStripLabel();
            this.btnExportLogEntries = new System.Windows.Forms.ToolStripButton();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblResultCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBarSecondary = new System.Windows.Forms.ToolStripProgressBar();
            this.progressBarPrimary = new System.Windows.Forms.ToolStripProgressBar();
            this.lblElapsed = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnCreate = new System.Windows.Forms.ToolStripButton();
            this.btnOpen = new System.Windows.Forms.ToolStripButton();
            this.btnAbout = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnLoadDefinitions = new System.Windows.Forms.ToolStripButton();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnCancel = new System.Windows.Forms.ToolStripButton();
            this.btnExtractAll = new System.Windows.Forms.ToolStripButton();
            this.btnExtractSelected = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDisplayOutput = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.previewTabs.SuspendLayout();
            this.previewHexTab.SuspendLayout();
            this.previewTextTab.SuspendLayout();
            this.previewImageTab.SuspendLayout();
            this.toolStrip3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl3);
            resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tabPage3);
            resources.ApplyResources(this.tabControl3, "tabControl3");
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.splitContainer3);
            resources.ApplyResources(this.tabPage3, "tabPage3");
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            resources.ApplyResources(this.splitContainer3, "splitContainer3");
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.searchBox);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.lblNoResults);
            this.splitContainer3.Panel2.Controls.Add(this.packageExplorerTreeView);
            // 
            // searchBox
            // 
            resources.ApplyResources(this.searchBox, "searchBox");
            this.searchBox.Name = "searchBox";
            this.searchBox.TextChanged += new System.EventHandler(this.SearchBox_TextChanged);
            // 
            // lblNoResults
            // 
            resources.ApplyResources(this.lblNoResults, "lblNoResults");
            this.lblNoResults.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblNoResults.Name = "lblNoResults";
            // 
            // packageExplorerTreeView
            // 
            resources.ApplyResources(this.packageExplorerTreeView, "packageExplorerTreeView");
            this.packageExplorerTreeView.Name = "packageExplorerTreeView";
            this.packageExplorerTreeView.NodeMouseClick += new System.EventHandler<System.Windows.Forms.TreeNodeMouseClickEventArgs>(this.PackageExplorerTreeView_NodeMouseClick);
            this.packageExplorerTreeView.NodeMouseDoubleClick += new System.EventHandler<System.Windows.Forms.TreeNodeMouseClickEventArgs>(this.PackageExplorerTreeView_NodeMouseDoubleClick);
            this.packageExplorerTreeView.Leave += new System.EventHandler(this.PackageExplorerTreeView_Leave);
            // 
            // splitContainer2
            // 
            resources.ApplyResources(this.splitContainer2, "splitContainer2");
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tabControl2);
            resources.ApplyResources(this.splitContainer2.Panel1, "splitContainer2.Panel1");
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.SplitContainer2_SplitterMoved);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage2);
            resources.ApplyResources(this.tabControl2, "tabControl2");
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.previewTabs);
            this.tabPage2.Controls.Add(this.toolStrip3);
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // previewTabs
            // 
            this.previewTabs.Controls.Add(this.previewHexTab);
            this.previewTabs.Controls.Add(this.previewTextTab);
            this.previewTabs.Controls.Add(this.previewImageTab);
            resources.ApplyResources(this.previewTabs, "previewTabs");
            this.previewTabs.Name = "previewTabs";
            this.previewTabs.SelectedIndex = 0;
            this.previewTabs.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.PreviewTabs_Selecting);
            // 
            // previewHexTab
            // 
            this.previewHexTab.Controls.Add(this.previewHexBox);
            resources.ApplyResources(this.previewHexTab, "previewHexTab");
            this.previewHexTab.Name = "previewHexTab";
            this.previewHexTab.UseVisualStyleBackColor = true;
            // 
            // previewHexBox
            // 
            this.previewHexBox.ColumnInfoVisible = true;
            resources.ApplyResources(this.previewHexBox, "previewHexBox");
            this.previewHexBox.LineInfoVisible = true;
            this.previewHexBox.Name = "previewHexBox";
            this.previewHexBox.ReadOnly = true;
            this.previewHexBox.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.previewHexBox.StringViewVisible = true;
            this.previewHexBox.UseFixedBytesPerLine = true;
            this.previewHexBox.VScrollBarVisible = true;
            // 
            // previewTextTab
            // 
            this.previewTextTab.Controls.Add(this.previewTextBox);
            resources.ApplyResources(this.previewTextTab, "previewTextTab");
            this.previewTextTab.Name = "previewTextTab";
            this.previewTextTab.UseVisualStyleBackColor = true;
            // 
            // previewTextBox
            // 
            this.previewTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.previewTextBox.DetectUrls = false;
            resources.ApplyResources(this.previewTextBox, "previewTextBox");
            this.previewTextBox.Name = "previewTextBox";
            this.previewTextBox.ReadOnly = true;
            // 
            // previewImageTab
            // 
            this.previewImageTab.Controls.Add(this.previewImageBox);
            resources.ApplyResources(this.previewImageTab, "previewImageTab");
            this.previewImageTab.Name = "previewImageTab";
            this.previewImageTab.UseVisualStyleBackColor = true;
            // 
            // previewImageBox
            // 
            resources.ApplyResources(this.previewImageBox, "previewImageBox");
            this.previewImageBox.Name = "previewImageBox";
            // 
            // toolStrip3
            // 
            this.toolStrip3.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblDataSize,
            this.toolStripSeparator1,
            this.lblMediaType,
            this.btnFormattedText});
            resources.ApplyResources(this.toolStrip3, "toolStrip3");
            this.toolStrip3.Name = "toolStrip3";
            // 
            // lblDataSize
            // 
            this.lblDataSize.Name = "lblDataSize";
            this.lblDataSize.Padding = new System.Windows.Forms.Padding(3);
            resources.ApplyResources(this.lblDataSize, "lblDataSize");
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // lblMediaType
            // 
            this.lblMediaType.Name = "lblMediaType";
            this.lblMediaType.Padding = new System.Windows.Forms.Padding(3);
            resources.ApplyResources(this.lblMediaType, "lblMediaType");
            // 
            // btnFormattedText
            // 
            this.btnFormattedText.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnFormattedText.Image = global::ResourcePacker.Properties.Images.checkbox_checked;
            this.btnFormattedText.Name = "btnFormattedText";
            this.btnFormattedText.Padding = new System.Windows.Forms.Padding(3);
            resources.ApplyResources(this.btnFormattedText, "btnFormattedText");
            this.btnFormattedText.Click += new System.EventHandler(this.BtnFormattedText_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.outputBox);
            this.tabPage1.Controls.Add(this.toolStrip2);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // outputBox
            // 
            this.outputBox.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.outputBox, "outputBox");
            this.outputBox.Name = "outputBox";
            this.outputBox.TextChanged += new System.EventHandler(this.OutputBox_TextChanged);
            // 
            // toolStrip2
            // 
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnToggleDebugMessages,
            this.toolStripSeparator4,
            this.btnClearConsole,
            this.lblLogEntries,
            this.btnExportLogEntries});
            resources.ApplyResources(this.toolStrip2, "toolStrip2");
            this.toolStrip2.Name = "toolStrip2";
            // 
            // btnToggleDebugMessages
            // 
            this.btnToggleDebugMessages.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnToggleDebugMessages.Image = global::ResourcePacker.Properties.Images.checkbox_checked;
            this.btnToggleDebugMessages.Name = "btnToggleDebugMessages";
            this.btnToggleDebugMessages.Padding = new System.Windows.Forms.Padding(3);
            resources.ApplyResources(this.btnToggleDebugMessages, "btnToggleDebugMessages");
            this.btnToggleDebugMessages.Click += new System.EventHandler(this.BtnToggleDebugMessages_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // btnClearConsole
            // 
            this.btnClearConsole.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            resources.ApplyResources(this.btnClearConsole, "btnClearConsole");
            this.btnClearConsole.Image = global::ResourcePacker.Properties.Images.report_delete;
            this.btnClearConsole.Name = "btnClearConsole";
            this.btnClearConsole.Padding = new System.Windows.Forms.Padding(3);
            this.btnClearConsole.Click += new System.EventHandler(this.BtnClearConsole_Click);
            // 
            // lblLogEntries
            // 
            this.lblLogEntries.Name = "lblLogEntries";
            this.lblLogEntries.Padding = new System.Windows.Forms.Padding(3);
            resources.ApplyResources(this.lblLogEntries, "lblLogEntries");
            // 
            // btnExportLogEntries
            // 
            this.btnExportLogEntries.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            resources.ApplyResources(this.btnExportLogEntries, "btnExportLogEntries");
            this.btnExportLogEntries.Image = global::ResourcePacker.Properties.Images.disk;
            this.btnExportLogEntries.Name = "btnExportLogEntries";
            this.btnExportLogEntries.Padding = new System.Windows.Forms.Padding(3);
            this.btnExportLogEntries.Click += new System.EventHandler(this.BtnExportLogEntries_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.lblResultCount,
            this.progressBarSecondary,
            this.progressBarPrimary,
            this.lblElapsed});
            resources.ApplyResources(this.statusStrip, "statusStrip");
            this.statusStrip.Name = "statusStrip";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            resources.ApplyResources(this.lblStatus, "lblStatus");
            this.lblStatus.Spring = true;
            // 
            // lblResultCount
            // 
            resources.ApplyResources(this.lblResultCount, "lblResultCount");
            this.lblResultCount.Name = "lblResultCount";
            // 
            // progressBarSecondary
            // 
            this.progressBarSecondary.MarqueeAnimationSpeed = 75;
            this.progressBarSecondary.Name = "progressBarSecondary";
            resources.ApplyResources(this.progressBarSecondary, "progressBarSecondary");
            // 
            // progressBarPrimary
            // 
            this.progressBarPrimary.MarqueeAnimationSpeed = 75;
            this.progressBarPrimary.Name = "progressBarPrimary";
            resources.ApplyResources(this.progressBarPrimary, "progressBarPrimary");
            // 
            // lblElapsed
            // 
            resources.ApplyResources(this.lblElapsed, "lblElapsed");
            this.lblElapsed.Name = "lblElapsed";
            // 
            // btnCreate
            // 
            this.btnCreate.Image = global::ResourcePacker.Properties.Images.database_add;
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Padding = new System.Windows.Forms.Padding(3);
            resources.ApplyResources(this.btnCreate, "btnCreate");
            this.btnCreate.Click += new System.EventHandler(this.BtnCreate_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Image = global::ResourcePacker.Properties.Images.database_connect;
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Padding = new System.Windows.Forms.Padding(3);
            resources.ApplyResources(this.btnOpen, "btnOpen");
            this.btnOpen.Click += new System.EventHandler(this.BtnOpen_Click);
            // 
            // btnAbout
            // 
            this.btnAbout.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnAbout.AutoToolTip = false;
            this.btnAbout.Image = global::ResourcePacker.Properties.Images.information;
            resources.ApplyResources(this.btnAbout, "btnAbout");
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Padding = new System.Windows.Forms.Padding(3);
            this.btnAbout.Click += new System.EventHandler(this.BtnAbout_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // btnLoadDefinitions
            // 
            resources.ApplyResources(this.btnLoadDefinitions, "btnLoadDefinitions");
            this.btnLoadDefinitions.Image = global::ResourcePacker.Properties.Images.book_link;
            this.btnLoadDefinitions.Name = "btnLoadDefinitions";
            this.btnLoadDefinitions.Padding = new System.Windows.Forms.Padding(3);
            this.btnLoadDefinitions.Click += new System.EventHandler(this.BtnLoadDefinitions_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCreate,
            this.btnOpen,
            this.btnCancel,
            this.btnAbout,
            this.toolStripSeparator2,
            this.btnLoadDefinitions,
            this.btnExtractAll,
            this.btnExtractSelected,
            this.toolStripSeparator3,
            this.btnDisplayOutput});
            resources.ApplyResources(this.toolStrip, "toolStrip");
            this.toolStrip.Name = "toolStrip";
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::ResourcePacker.Properties.Images.cross;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Windows.Forms.Padding(3);
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnExtractAll
            // 
            resources.ApplyResources(this.btnExtractAll, "btnExtractAll");
            this.btnExtractAll.Image = global::ResourcePacker.Properties.Images.compress;
            this.btnExtractAll.Name = "btnExtractAll";
            this.btnExtractAll.Padding = new System.Windows.Forms.Padding(3);
            // 
            // btnExtractSelected
            // 
            resources.ApplyResources(this.btnExtractSelected, "btnExtractSelected");
            this.btnExtractSelected.Image = global::ResourcePacker.Properties.Images.table_go;
            this.btnExtractSelected.Name = "btnExtractSelected";
            this.btnExtractSelected.Padding = new System.Windows.Forms.Padding(3);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // btnDisplayOutput
            // 
            this.btnDisplayOutput.Image = global::ResourcePacker.Properties.Images.checkbox_checked;
            resources.ApplyResources(this.btnDisplayOutput, "btnDisplayOutput");
            this.btnDisplayOutput.Name = "btnDisplayOutput";
            this.btnDisplayOutput.Padding = new System.Windows.Forms.Padding(3);
            this.btnDisplayOutput.Click += new System.EventHandler(this.BtnDisplayOutput_Click);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip);
            this.Name = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResizeBegin += new System.EventHandler(this.MainForm_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.previewTabs.ResumeLayout(false);
            this.previewHexTab.ResumeLayout(false);
            this.previewTextTab.ResumeLayout(false);
            this.previewImageTab.ResumeLayout(false);
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;
        private ToolStripStatusLabel lblResultCount;
        private ToolStripStatusLabel lblElapsed;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private TabControl tabControl3;
        private TabPage tabPage3;
        private TabControl tabControl2;
        private TabControl tabControl1;
        private SplitContainer splitContainer3;
        private TextBox searchBox;
        private TabPage tabPage2;
        private ToolStripButton btnCreate;
        private ToolStripButton btnOpen;
        private ToolStripButton btnAbout;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton btnLoadDefinitions;
        private ToolStrip toolStrip;
        private ToolStripButton btnExtractAll;
        private ToolStripProgressBar progressBarPrimary;
        private ToolStripButton btnCancel;
        private ToolStripProgressBar progressBarSecondary;
        private Controls.MultiNodeSelectionTreeView packageExplorerTreeView;
        private Label lblNoResults;
        private ToolStripButton btnExtractSelected;
        private TabPage tabPage1;
        private RichTextBox outputBox;
        private ToolStrip toolStrip2;
        private ToolStripButton btnClearConsole;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripButton btnToggleDebugMessages;
        private ToolStripLabel lblLogEntries;
        private ToolStrip toolStrip3;
        private ToolStripLabel lblDataSize;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripLabel lblMediaType;
        private ToolStripButton btnFormattedText;
        private TabControl previewTabs;
        private TabPage previewHexTab;
        private Be.Windows.Forms.HexBox previewHexBox;
        private TabPage previewTextTab;
        private RichTextBox previewTextBox;
        private TabPage previewImageTab;
        private Cyotek.Windows.Forms.ImageBox previewImageBox;
        private ToolStripButton btnExportLogEntries;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton btnDisplayOutput;
    }
}