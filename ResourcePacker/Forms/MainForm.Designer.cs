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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.searchBox = new System.Windows.Forms.TextBox();
            this.lblNoResults = new System.Windows.Forms.Label();
            this.explorerTreeView = new System.Windows.Forms.TreeView();
            this.explorerImages = new System.Windows.Forms.ImageList(this.components);
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.lblMediaType = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.lblDataSize = new System.Windows.Forms.ToolStripLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.outputBox = new System.Windows.Forms.RichTextBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblResultCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.prgRunning = new System.Windows.Forms.ToolStripProgressBar();
            this.lblElapsed = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnCreate = new System.Windows.Forms.ToolStripButton();
            this.btnOpen = new System.Windows.Forms.ToolStripButton();
            this.btnAbout = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnLoadDefinitions = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnToggleDebugMessages = new System.Windows.Forms.ToolStripButton();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnExtract = new System.Windows.Forms.ToolStripButton();
            this.btnFormattedText = new System.Windows.Forms.ToolStripButton();
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
            this.toolStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
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
            this.splitContainer3.Panel2.Controls.Add(this.explorerTreeView);
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
            // explorerTreeView
            // 
            resources.ApplyResources(this.explorerTreeView, "explorerTreeView");
            this.explorerTreeView.ImageList = this.explorerImages;
            this.explorerTreeView.Name = "explorerTreeView";
            this.explorerTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.ExplorerTreeView_NodeMouseDoubleClick);
            // 
            // explorerImages
            // 
            this.explorerImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.explorerImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("explorerImages.ImageStream")));
            this.explorerImages.TransparentColor = System.Drawing.Color.Transparent;
            this.explorerImages.Images.SetKeyName(0, "application_view_tile.png");
            this.explorerImages.Images.SetKeyName(1, "music.png");
            this.explorerImages.Images.SetKeyName(2, "font.png");
            this.explorerImages.Images.SetKeyName(3, "picture.png");
            this.explorerImages.Images.SetKeyName(4, "page.png");
            this.explorerImages.Images.SetKeyName(5, "film.png");
            this.explorerImages.Images.SetKeyName(6, "help.png");
            this.explorerImages.Images.SetKeyName(7, "folder.png");
            this.explorerImages.Images.SetKeyName(8, "database.png");
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
            this.tabPage2.Controls.Add(this.toolStrip1);
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
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblMediaType,
            this.toolStripSeparator3,
            this.lblDataSize});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // lblMediaType
            // 
            this.lblMediaType.Name = "lblMediaType";
            this.lblMediaType.Padding = new System.Windows.Forms.Padding(3);
            resources.ApplyResources(this.lblMediaType, "lblMediaType");
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // lblDataSize
            // 
            this.lblDataSize.Name = "lblDataSize";
            this.lblDataSize.Padding = new System.Windows.Forms.Padding(3);
            resources.ApplyResources(this.lblDataSize, "lblDataSize");
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
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // outputBox
            // 
            this.outputBox.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.outputBox, "outputBox");
            this.outputBox.Name = "outputBox";
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.lblResultCount,
            this.prgRunning,
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
            // prgRunning
            // 
            this.prgRunning.Name = "prgRunning";
            resources.ApplyResources(this.prgRunning, "prgRunning");
            // 
            // lblElapsed
            // 
            resources.ApplyResources(this.lblElapsed, "lblElapsed");
            this.lblElapsed.Name = "lblElapsed";
            // 
            // btnCreate
            // 
            this.btnCreate.Image = global::ResourcePacker.Properties.Images.database_add;
            resources.ApplyResources(this.btnCreate, "btnCreate");
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Padding = new System.Windows.Forms.Padding(3);
            this.btnCreate.Click += new System.EventHandler(this.BtnCreate_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Image = global::ResourcePacker.Properties.Images.database_connect;
            resources.ApplyResources(this.btnOpen, "btnOpen");
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Padding = new System.Windows.Forms.Padding(3);
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
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // btnToggleDebugMessages
            // 
            this.btnToggleDebugMessages.Image = global::ResourcePacker.Properties.Images.checkbox_checked;
            this.btnToggleDebugMessages.Name = "btnToggleDebugMessages";
            this.btnToggleDebugMessages.Padding = new System.Windows.Forms.Padding(3);
            resources.ApplyResources(this.btnToggleDebugMessages, "btnToggleDebugMessages");
            this.btnToggleDebugMessages.Click += new System.EventHandler(this.BtnToggleDebugMessages_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCreate,
            this.btnOpen,
            this.btnAbout,
            this.toolStripSeparator2,
            this.btnLoadDefinitions,
            this.btnExtract,
            this.toolStripSeparator1,
            this.btnToggleDebugMessages,
            this.btnFormattedText});
            resources.ApplyResources(this.toolStrip, "toolStrip");
            this.toolStrip.Name = "toolStrip";
            // 
            // btnExtract
            // 
            resources.ApplyResources(this.btnExtract, "btnExtract");
            this.btnExtract.Image = global::ResourcePacker.Properties.Images.compress;
            this.btnExtract.Name = "btnExtract";
            this.btnExtract.Padding = new System.Windows.Forms.Padding(3);
            // 
            // btnFormattedText
            // 
            this.btnFormattedText.Image = global::ResourcePacker.Properties.Images.checkbox_checked;
            this.btnFormattedText.Name = "btnFormattedText";
            this.btnFormattedText.Padding = new System.Windows.Forms.Padding(3);
            resources.ApplyResources(this.btnFormattedText, "btnFormattedText");
            this.btnFormattedText.Click += new System.EventHandler(this.BtnFormattedText_Click);
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
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResizeBegin += new System.EventHandler(this.MainForm_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
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
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
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
        private ToolStripProgressBar prgRunning;
        private ToolStripStatusLabel lblElapsed;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private TabControl tabControl3;
        private TabPage tabPage3;
        private TabControl tabControl2;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private RichTextBox outputBox;
        private ImageList explorerImages;
        private SplitContainer splitContainer3;
        private TextBox searchBox;
        private TreeView explorerTreeView;
        private Label lblNoResults;
        private TabPage tabPage2;
        private TabControl previewTabs;
        private TabPage previewImageTab;
        private TabPage previewTextTab;
        private ToolStrip toolStrip1;
        private ToolStripLabel lblMediaType;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripLabel lblDataSize;
        private Cyotek.Windows.Forms.ImageBox previewImageBox;
        private RichTextBox previewTextBox;
        private TabPage previewHexTab;
        private Be.Windows.Forms.HexBox previewHexBox;
        private ToolStripButton btnCreate;
        private ToolStripButton btnOpen;
        private ToolStripButton btnAbout;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton btnLoadDefinitions;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton btnToggleDebugMessages;
        private ToolStrip toolStrip;
        private ToolStripButton btnFormattedText;
        private ToolStripButton btnExtract;
    }
}