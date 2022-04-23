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
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnCreate = new System.Windows.Forms.ToolStripButton();
            this.btnOpen = new System.Windows.Forms.ToolStripButton();
            this.btnClose = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.btnAbout = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.btnDefinitions = new System.Windows.Forms.ToolStripButton();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblResultCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.prgRunning = new System.Windows.Forms.ToolStripProgressBar();
            this.lblElapsed = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCreate,
            this.btnOpen,
            this.btnClose,
            this.toolStripSeparator1,
            this.btnRefresh,
            this.btnAbout,
            this.toolStripButton1,
            this.btnDefinitions});
            resources.ApplyResources(this.toolStrip, "toolStrip");
            this.toolStrip.Name = "toolStrip";
            // 
            // btnCreate
            // 
            this.btnCreate.Image = global::ResourcePacker.Properties.Images.database_add;
            resources.ApplyResources(this.btnCreate, "btnCreate");
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Padding = new System.Windows.Forms.Padding(3);
            // 
            // btnOpen
            // 
            this.btnOpen.Image = global::ResourcePacker.Properties.Images.database_connect;
            resources.ApplyResources(this.btnOpen, "btnOpen");
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Padding = new System.Windows.Forms.Padding(3);
            this.btnOpen.Click += new System.EventHandler(this.BtnOpen_Click);
            // 
            // btnClose
            // 
            this.btnClose.Image = global::ResourcePacker.Properties.Images.database_delete;
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Name = "btnClose";
            this.btnClose.Padding = new System.Windows.Forms.Padding(3);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // btnRefresh
            // 
            resources.ApplyResources(this.btnRefresh, "btnRefresh");
            this.btnRefresh.Image = global::ResourcePacker.Properties.Images.arrow_refresh;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Padding = new System.Windows.Forms.Padding(3);
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
            // toolStripButton1
            // 
            resources.ApplyResources(this.toolStripButton1, "toolStripButton1");
            this.toolStripButton1.Image = global::ResourcePacker.Properties.Images.lightning;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Padding = new System.Windows.Forms.Padding(3);
            // 
            // btnDefinitions
            // 
            resources.ApplyResources(this.btnDefinitions, "btnDefinitions");
            this.btnDefinitions.Image = global::ResourcePacker.Properties.Images.book_link;
            this.btnDefinitions.Name = "btnDefinitions";
            this.btnDefinitions.Padding = new System.Windows.Forms.Padding(3);
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
            // MainForm
            // 
            this.AllowDrop = true;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip);
            this.Name = "MainForm";
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ToolStrip toolStrip;
        private ToolStripButton btnCreate;
        private ToolStripButton btnOpen;
        private ToolStripButton btnClose;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton btnRefresh;
        private ToolStripButton btnAbout;
        private ToolStripButton toolStripButton1;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;
        private ToolStripStatusLabel lblResultCount;
        private ToolStripProgressBar prgRunning;
        private ToolStripStatusLabel lblElapsed;
        private ToolStripButton btnDefinitions;
    }
}