namespace WinFormsUI.Forms
{
    partial class ReplaceDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.chkRepeatAll = new System.Windows.Forms.CheckBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnReplace = new System.Windows.Forms.Button();
            this.lblReplaceFileDescription = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.btnKeep = new System.Windows.Forms.Button();
            this.lblBothFilesDescription = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.btnSkip = new System.Windows.Forms.Button();
            this.lblNoExtractDescription = new System.Windows.Forms.Label();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(380, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "A file with the same name as the resource already exists:";
            // 
            // chkRepeatAll
            // 
            this.chkRepeatAll.AutoSize = true;
            this.chkRepeatAll.Location = new System.Drawing.Point(15, 250);
            this.chkRepeatAll.Name = "chkRepeatAll";
            this.chkRepeatAll.Size = new System.Drawing.Size(229, 24);
            this.chkRepeatAll.TabIndex = 1;
            this.chkRepeatAll.Text = "Do this for the next 5 conflicts";
            this.chkRepeatAll.UseVisualStyleBackColor = true;
            this.chkRepeatAll.CheckedChanged += new System.EventHandler(this.ChkRepeatAll_CheckedChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(12, 61);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnReplace);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lblReplaceFileDescription);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(3);
            this.splitContainer1.Size = new System.Drawing.Size(558, 54);
            this.splitContainer1.SplitterDistance = 191;
            this.splitContainer1.TabIndex = 2;
            // 
            // btnReplace
            // 
            this.btnReplace.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnReplace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReplace.Location = new System.Drawing.Point(3, 3);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(185, 48);
            this.btnReplace.TabIndex = 0;
            this.btnReplace.Text = "Replace file";
            this.btnReplace.UseVisualStyleBackColor = true;
            // 
            // lblReplaceFileDescription
            // 
            this.lblReplaceFileDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblReplaceFileDescription.Location = new System.Drawing.Point(3, 3);
            this.lblReplaceFileDescription.Name = "lblReplaceFileDescription";
            this.lblReplaceFileDescription.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.lblReplaceFileDescription.Size = new System.Drawing.Size(357, 48);
            this.lblReplaceFileDescription.TabIndex = 0;
            this.lblReplaceFileDescription.Text = "Replace the file in the destination folder with \r\nthe resource that will be extra" +
    "cted.";
            // 
            // splitContainer2
            // 
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(12, 115);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.btnKeep);
            this.splitContainer2.Panel1.Padding = new System.Windows.Forms.Padding(3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.lblBothFilesDescription);
            this.splitContainer2.Panel2.Padding = new System.Windows.Forms.Padding(3);
            this.splitContainer2.Size = new System.Drawing.Size(558, 54);
            this.splitContainer2.SplitterDistance = 191;
            this.splitContainer2.TabIndex = 3;
            // 
            // btnKeep
            // 
            this.btnKeep.DialogResult = System.Windows.Forms.DialogResult.Continue;
            this.btnKeep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnKeep.Location = new System.Drawing.Point(3, 3);
            this.btnKeep.Name = "btnKeep";
            this.btnKeep.Size = new System.Drawing.Size(185, 48);
            this.btnKeep.TabIndex = 0;
            this.btnKeep.Text = "Keep both files";
            this.btnKeep.UseVisualStyleBackColor = true;
            // 
            // lblBothFilesDescription
            // 
            this.lblBothFilesDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBothFilesDescription.Location = new System.Drawing.Point(3, 3);
            this.lblBothFilesDescription.Name = "lblBothFilesDescription";
            this.lblBothFilesDescription.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.lblBothFilesDescription.Size = new System.Drawing.Size(357, 48);
            this.lblBothFilesDescription.TabIndex = 0;
            this.lblBothFilesDescription.Text = "The resource will be renamed to \'File (2).txt\',\r\nand extracted to the destination" +
    " folder.";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(476, 247);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(94, 29);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.IsSplitterFixed = true;
            this.splitContainer3.Location = new System.Drawing.Point(12, 169);
            this.splitContainer3.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.btnSkip);
            this.splitContainer3.Panel1.Padding = new System.Windows.Forms.Padding(3);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.lblNoExtractDescription);
            this.splitContainer3.Panel2.Padding = new System.Windows.Forms.Padding(3);
            this.splitContainer3.Size = new System.Drawing.Size(558, 54);
            this.splitContainer3.SplitterDistance = 191;
            this.splitContainer3.TabIndex = 5;
            // 
            // btnSkip
            // 
            this.btnSkip.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            this.btnSkip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSkip.Location = new System.Drawing.Point(3, 3);
            this.btnSkip.Name = "btnSkip";
            this.btnSkip.Size = new System.Drawing.Size(185, 48);
            this.btnSkip.TabIndex = 0;
            this.btnSkip.Text = "Don\'t extract";
            this.btnSkip.UseVisualStyleBackColor = true;
            // 
            // lblNoExtractDescription
            // 
            this.lblNoExtractDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNoExtractDescription.Location = new System.Drawing.Point(3, 3);
            this.lblNoExtractDescription.Name = "lblNoExtractDescription";
            this.lblNoExtractDescription.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.lblNoExtractDescription.Size = new System.Drawing.Size(357, 48);
            this.lblNoExtractDescription.TabIndex = 0;
            this.lblNoExtractDescription.Text = "The resource will not be extracted to \r\nthe destination folder.";
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(12, 31);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(558, 27);
            this.txtFilePath.TabIndex = 6;
            this.txtFilePath.Text = "C:\\path\\file.txt";
            // 
            // ReplaceDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 288);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.splitContainer3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.chkRepeatAll);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReplaceDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Duplicate file names";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private CheckBox chkRepeatAll;
        private SplitContainer splitContainer1;
        private Button btnReplace;
        private Label lblReplaceFileDescription;
        private SplitContainer splitContainer2;
        private Button btnKeep;
        private Label lblBothFilesDescription;
        private Button btnCancel;
        private SplitContainer splitContainer3;
        private Button btnSkip;
        private Label lblNoExtractDescription;
        private TextBox txtFilePath;
    }
}