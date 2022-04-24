namespace ResourcePacker.Forms
{
    partial class OpenForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkShowPassword = new System.Windows.Forms.CheckBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnDefinitionsExplore = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDefinitions = new System.Windows.Forms.TextBox();
            this.btnLocationExplore = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFileLocation = new System.Windows.Forms.TextBox();
            this.btnExecute = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.logTextBox1 = new ResourcePacker.Controls.LogTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.chkShowPassword);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.btnDefinitionsExplore);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtDefinitions);
            this.groupBox1.Controls.Add(this.btnLocationExplore);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtFileLocation);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(572, 209);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Resource file";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 141);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Password";
            // 
            // chkShowPassword
            // 
            this.chkShowPassword.AutoSize = true;
            this.chkShowPassword.Checked = true;
            this.chkShowPassword.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowPassword.Location = new System.Drawing.Point(432, 140);
            this.chkShowPassword.Name = "chkShowPassword";
            this.chkShowPassword.Size = new System.Drawing.Size(134, 24);
            this.chkShowPassword.TabIndex = 7;
            this.chkShowPassword.Text = "Show password";
            this.chkShowPassword.UseVisualStyleBackColor = true;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(6, 164);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(560, 27);
            this.txtPassword.TabIndex = 6;
            // 
            // btnDefinitionsExplore
            // 
            this.btnDefinitionsExplore.Image = global::ResourcePacker.Properties.Images.folder_explore;
            this.btnDefinitionsExplore.Location = new System.Drawing.Point(472, 104);
            this.btnDefinitionsExplore.Name = "btnDefinitionsExplore";
            this.btnDefinitionsExplore.Size = new System.Drawing.Size(94, 29);
            this.btnDefinitionsExplore.TabIndex = 5;
            this.btnDefinitionsExplore.Text = "Explore";
            this.btnDefinitionsExplore.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDefinitionsExplore.UseVisualStyleBackColor = true;
            this.btnDefinitionsExplore.Click += new System.EventHandler(this.BtnDefinitionsExplore_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Definitions";
            // 
            // txtDefinitions
            // 
            this.txtDefinitions.Enabled = false;
            this.txtDefinitions.Location = new System.Drawing.Point(6, 105);
            this.txtDefinitions.Margin = new System.Windows.Forms.Padding(3, 3, 3, 9);
            this.txtDefinitions.Name = "txtDefinitions";
            this.txtDefinitions.ReadOnly = true;
            this.txtDefinitions.Size = new System.Drawing.Size(460, 27);
            this.txtDefinitions.TabIndex = 3;
            // 
            // btnLocationExplore
            // 
            this.btnLocationExplore.Image = global::ResourcePacker.Properties.Images.folder_explore;
            this.btnLocationExplore.Location = new System.Drawing.Point(472, 45);
            this.btnLocationExplore.Name = "btnLocationExplore";
            this.btnLocationExplore.Size = new System.Drawing.Size(94, 29);
            this.btnLocationExplore.TabIndex = 2;
            this.btnLocationExplore.Text = "Explore";
            this.btnLocationExplore.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLocationExplore.UseVisualStyleBackColor = true;
            this.btnLocationExplore.Click += new System.EventHandler(this.BtnLocationExplore_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "File location";
            // 
            // txtFileLocation
            // 
            this.txtFileLocation.Enabled = false;
            this.txtFileLocation.Location = new System.Drawing.Point(6, 46);
            this.txtFileLocation.Margin = new System.Windows.Forms.Padding(3, 3, 3, 9);
            this.txtFileLocation.Name = "txtFileLocation";
            this.txtFileLocation.ReadOnly = true;
            this.txtFileLocation.Size = new System.Drawing.Size(460, 27);
            this.txtFileLocation.TabIndex = 0;
            // 
            // btnExecute
            // 
            this.btnExecute.Enabled = false;
            this.btnExecute.Location = new System.Drawing.Point(490, 462);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(94, 29);
            this.btnExecute.TabIndex = 2;
            this.btnExecute.Text = "OK";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.BtnExecute_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.logTextBox1);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.progressBar);
            this.groupBox3.Location = new System.Drawing.Point(12, 227);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(572, 229);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Output";
            // 
            // logTextBox1
            // 
            this.logTextBox1.ForContext = "";
            this.logTextBox1.Location = new System.Drawing.Point(7, 83);
            this.logTextBox1.LogBorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.logTextBox1.LogPadding = new System.Windows.Forms.Padding(3);
            this.logTextBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.logTextBox1.Name = "logTextBox1";
            this.logTextBox1.ReadOnly = false;
            this.logTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTextBox1.Size = new System.Drawing.Size(558, 138);
            this.logTextBox1.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "Progress";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(6, 46);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(560, 29);
            this.progressBar.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(390, 462);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(94, 29);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // OpenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 503);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OpenForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Open";
            this.Load += new System.EventHandler(this.OpenForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox1;
        private Button btnDefinitionsExplore;
        private Label label2;
        private TextBox txtDefinitions;
        private Button btnLocationExplore;
        private Label label1;
        private TextBox txtFileLocation;
        private Button btnExecute;
        private GroupBox groupBox3;
        private ProgressBar progressBar;
        private Label label4;
        private Label label3;
        private CheckBox chkShowPassword;
        private TextBox txtPassword;
        private Button btnCancel;
        private Controls.LogTextBox logTextBox1;
    }
}