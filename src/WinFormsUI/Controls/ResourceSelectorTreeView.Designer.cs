namespace WinFormsUI.Controls
{
    partial class ResourceSelectorTreeView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResourceSelectorTreeView));
            this.treeView = new System.Windows.Forms.TreeView();
            this.mediaTypeImages = new System.Windows.Forms.ImageList(this.components);
            this.lblConflictAmount = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.mediaTypeImages;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageIndex = 0;
            this.treeView.Size = new System.Drawing.Size(268, 150);
            this.treeView.TabIndex = 0;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_AfterSelect);
            this.treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeView_NodeMouseClick);
            this.treeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeView_NodeMouseDoubleClick);
            // 
            // mediaTypeImages
            // 
            this.mediaTypeImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.mediaTypeImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("mediaTypeImages.ImageStream")));
            this.mediaTypeImages.TransparentColor = System.Drawing.Color.Transparent;
            this.mediaTypeImages.Images.SetKeyName(0, "application_view_tile.png");
            this.mediaTypeImages.Images.SetKeyName(1, "music.png");
            this.mediaTypeImages.Images.SetKeyName(2, "font.png");
            this.mediaTypeImages.Images.SetKeyName(3, "picture.png");
            this.mediaTypeImages.Images.SetKeyName(4, "page.png");
            this.mediaTypeImages.Images.SetKeyName(5, "film.png");
            this.mediaTypeImages.Images.SetKeyName(6, "help.png");
            this.mediaTypeImages.Images.SetKeyName(7, "folder.png");
            this.mediaTypeImages.Images.SetKeyName(8, "database.png");
            // 
            // lblConflictAmount
            // 
            this.lblConflictAmount.AutoSize = true;
            this.lblConflictAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblConflictAmount.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblConflictAmount.Location = new System.Drawing.Point(0, 128);
            this.lblConflictAmount.Name = "lblConflictAmount";
            this.lblConflictAmount.Size = new System.Drawing.Size(215, 22);
            this.lblConflictAmount.TabIndex = 1;
            this.lblConflictAmount.Text = "2 potential extraction conflicts.";
            this.lblConflictAmount.Visible = false;
            // 
            // ResourceSelectorTreeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblConflictAmount);
            this.Controls.Add(this.treeView);
            this.DoubleBuffered = true;
            this.Name = "ResourceSelectorTreeView";
            this.Size = new System.Drawing.Size(268, 150);
            this.Leave += new System.EventHandler(this.MultiNodeSelectionTreeView_Leave);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TreeView treeView;
        private ImageList mediaTypeImages;
        private Label lblConflictAmount;
    }
}
