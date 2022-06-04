namespace ResourcePacker.Controls
{
    partial class TriStateTreeView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TriStateTreeView));
            this.treeView = new System.Windows.Forms.TreeView();
            this.checkboxStateImages = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(150, 150);
            this.treeView.StateImageList = this.checkboxStateImages;
            this.treeView.TabIndex = 0;
            this.treeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.AfterCheck);
            this.treeView.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.AfterExpand);
            this.treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.NodeMouseClick);
            // 
            // checkboxStateImages
            // 
            this.checkboxStateImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.checkboxStateImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("checkboxStateImages.ImageStream")));
            this.checkboxStateImages.TransparentColor = System.Drawing.Color.Transparent;
            this.checkboxStateImages.Images.SetKeyName(0, "checkbox_unchecked.png");
            this.checkboxStateImages.Images.SetKeyName(1, "checkbox_checked.png");
            this.checkboxStateImages.Images.SetKeyName(2, "checkbox_tristate.png");
            // 
            // TriStateTreeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeView);
            this.Name = "TriStateTreeView";
            this.ResumeLayout(false);

        }

        #endregion

        private TreeView treeView;
        private ImageList checkboxStateImages;
    }
}
