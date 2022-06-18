namespace ResourcePacker.Forms
{
    partial class ReplaceForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.btnKeep = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.btnSkip = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
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
            this.label1.Size = new System.Drawing.Size(463, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "There is already a file with the same name as the file to be extracted:";
            // 
            // chkRepeatAll
            // 
            this.chkRepeatAll.AutoSize = true;
            this.chkRepeatAll.Location = new System.Drawing.Point(12, 265);
            this.chkRepeatAll.Name = "chkRepeatAll";
            this.chkRepeatAll.Size = new System.Drawing.Size(218, 24);
            this.chkRepeatAll.TabIndex = 1;
            this.chkRepeatAll.Text = "Do this for all duplicate files";
            this.chkRepeatAll.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(12, 61);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnReplace);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(3);
            this.splitContainer1.Size = new System.Drawing.Size(508, 54);
            this.splitContainer1.SplitterDistance = 156;
            this.splitContainer1.TabIndex = 2;
            // 
            // btnReplace
            // 
            this.btnReplace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReplace.Location = new System.Drawing.Point(3, 3);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(150, 48);
            this.btnReplace.TabIndex = 0;
            this.btnReplace.Text = "Replace file";
            this.btnReplace.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(279, 40);
            this.label2.TabIndex = 0;
            this.label2.Text = "Replace the file in the destination folder \r\nwith the file that will be extracted" +
    ".";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Location = new System.Drawing.Point(12, 121);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.btnKeep);
            this.splitContainer2.Panel1.Padding = new System.Windows.Forms.Padding(3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.label3);
            this.splitContainer2.Panel2.Padding = new System.Windows.Forms.Padding(3);
            this.splitContainer2.Size = new System.Drawing.Size(508, 54);
            this.splitContainer2.SplitterDistance = 156;
            this.splitContainer2.TabIndex = 3;
            // 
            // btnKeep
            // 
            this.btnKeep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnKeep.Location = new System.Drawing.Point(3, 3);
            this.btnKeep.Name = "btnKeep";
            this.btnKeep.Size = new System.Drawing.Size(150, 48);
            this.btnKeep.TabIndex = 0;
            this.btnKeep.Text = "Keep both files";
            this.btnKeep.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(233, 40);
            this.label3.TabIndex = 0;
            this.label3.Text = "The file that will be extracted will \r\nbe renamed to \'File (2).txt\'.";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(426, 262);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(94, 29);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Location = new System.Drawing.Point(12, 181);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.btnSkip);
            this.splitContainer3.Panel1.Padding = new System.Windows.Forms.Padding(3);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.label4);
            this.splitContainer3.Panel2.Padding = new System.Windows.Forms.Padding(3);
            this.splitContainer3.Size = new System.Drawing.Size(508, 54);
            this.splitContainer3.SplitterDistance = 156;
            this.splitContainer3.TabIndex = 5;
            // 
            // btnSkip
            // 
            this.btnSkip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSkip.Location = new System.Drawing.Point(3, 3);
            this.btnSkip.Name = "btnSkip";
            this.btnSkip.Size = new System.Drawing.Size(150, 48);
            this.btnSkip.TabIndex = 0;
            this.btnSkip.Text = "Don\'t extract";
            this.btnSkip.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(220, 40);
            this.label4.TabIndex = 0;
            this.label4.Text = "The file will not be extracted to \r\nthe destination folder.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(12, 32);
            this.label5.Margin = new System.Windows.Forms.Padding(3, 0, 3, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 20);
            this.label5.TabIndex = 6;
            this.label5.Text = "C:\\path\\file.txt";
            // 
            // ReplaceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 303);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.splitContainer3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.chkRepeatAll);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReplaceForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Duplicate file";
            this.Load += new System.EventHandler(this.ReplaceForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
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
        private Label label2;
        private SplitContainer splitContainer2;
        private Button btnKeep;
        private Label label3;
        private Button btnCancel;
        private SplitContainer splitContainer3;
        private Button btnSkip;
        private Label label4;
        private Label label5;
    }
}