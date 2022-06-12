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
    partial class CreateForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblStatusFile = new System.Windows.Forms.Label();
            this.progressBarPrimary = new System.Windows.Forms.ProgressBar();
            this.progressBarSecondary = new System.Windows.Forms.ProgressBar();
            this.splitContainer6 = new System.Windows.Forms.SplitContainer();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblPercentage = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.chkCreateDefinitionFile = new System.Windows.Forms.CheckBox();
            this.splitContainerDefinitionFile = new System.Windows.Forms.SplitContainer();
            this.txtDefinitionsLocation = new System.Windows.Forms.TextBox();
            this.btnDefinitionsExplore = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.txtPackageLocation = new System.Windows.Forms.TextBox();
            this.btnPackageExplore = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkShowPassword = new System.Windows.Forms.CheckBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.txtAssetFolder = new System.Windows.Forms.TextBox();
            this.btnAssetExplore = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.lblAvailableItems = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lblSelectedItems = new System.Windows.Forms.ToolStripLabel();
            this.grpBoxItemSelector = new System.Windows.Forms.GroupBox();
            this.selectorTreeView = new ResourcePacker.Controls.TriStateTreeView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCreate = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).BeginInit();
            this.splitContainer6.Panel1.SuspendLayout();
            this.splitContainer6.Panel2.SuspendLayout();
            this.splitContainer6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerDefinitionFile)).BeginInit();
            this.splitContainerDefinitionFile.Panel1.SuspendLayout();
            this.splitContainerDefinitionFile.Panel2.SuspendLayout();
            this.splitContainerDefinitionFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.grpBoxItemSelector.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(1217, 733);
            this.splitContainer1.SplitterDistance = 680;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tabControl1);
            this.splitContainer2.Panel1.Padding = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.splitContainer2.Panel1MinSize = 435;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tabControl2);
            this.splitContainer2.Panel2.Padding = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.splitContainer2.Panel2MinSize = 435;
            this.splitContainer2.Size = new System.Drawing.Size(1217, 680);
            this.splitContainer2.SplitterDistance = 545;
            this.splitContainer2.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(542, 677);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Controls.Add(this.groupBox7);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(534, 644);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Configuration";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lblStatusFile);
            this.groupBox4.Controls.Add(this.progressBarPrimary);
            this.groupBox4.Controls.Add(this.progressBarSecondary);
            this.groupBox4.Controls.Add(this.splitContainer6);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox4.Location = new System.Drawing.Point(3, 433);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(9);
            this.groupBox4.Size = new System.Drawing.Size(528, 133);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Progress";
            // 
            // lblStatusFile
            // 
            this.lblStatusFile.AutoEllipsis = true;
            this.lblStatusFile.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblStatusFile.Location = new System.Drawing.Point(9, 90);
            this.lblStatusFile.Name = "lblStatusFile";
            this.lblStatusFile.Size = new System.Drawing.Size(510, 20);
            this.lblStatusFile.TabIndex = 19;
            // 
            // progressBarPrimary
            // 
            this.progressBarPrimary.Dock = System.Windows.Forms.DockStyle.Top;
            this.progressBarPrimary.Location = new System.Drawing.Point(9, 64);
            this.progressBarPrimary.MarqueeAnimationSpeed = 75;
            this.progressBarPrimary.Name = "progressBarPrimary";
            this.progressBarPrimary.Size = new System.Drawing.Size(510, 26);
            this.progressBarPrimary.TabIndex = 18;
            // 
            // progressBarSecondary
            // 
            this.progressBarSecondary.Dock = System.Windows.Forms.DockStyle.Top;
            this.progressBarSecondary.Location = new System.Drawing.Point(9, 54);
            this.progressBarSecondary.MarqueeAnimationSpeed = 75;
            this.progressBarSecondary.Name = "progressBarSecondary";
            this.progressBarSecondary.Size = new System.Drawing.Size(510, 10);
            this.progressBarSecondary.TabIndex = 16;
            this.progressBarSecondary.Visible = false;
            // 
            // splitContainer6
            // 
            this.splitContainer6.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer6.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer6.IsSplitterFixed = true;
            this.splitContainer6.Location = new System.Drawing.Point(9, 29);
            this.splitContainer6.Name = "splitContainer6";
            // 
            // splitContainer6.Panel1
            // 
            this.splitContainer6.Panel1.Controls.Add(this.lblStatus);
            // 
            // splitContainer6.Panel2
            // 
            this.splitContainer6.Panel2.Controls.Add(this.lblPercentage);
            this.splitContainer6.Size = new System.Drawing.Size(510, 25);
            this.splitContainer6.SplitterDistance = 407;
            this.splitContainer6.TabIndex = 8;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblStatus.Location = new System.Drawing.Point(0, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(50, 20);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Ready";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPercentage
            // 
            this.lblPercentage.AutoSize = true;
            this.lblPercentage.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblPercentage.Location = new System.Drawing.Point(70, 0);
            this.lblPercentage.Name = "lblPercentage";
            this.lblPercentage.Size = new System.Drawing.Size(29, 20);
            this.lblPercentage.TabIndex = 0;
            this.lblPercentage.Text = "0%";
            this.lblPercentage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.chkCreateDefinitionFile);
            this.groupBox7.Controls.Add(this.splitContainerDefinitionFile);
            this.groupBox7.Controls.Add(this.label4);
            this.groupBox7.Controls.Add(this.splitContainer3);
            this.groupBox7.Controls.Add(this.label3);
            this.groupBox7.Controls.Add(this.groupBox2);
            this.groupBox7.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.groupBox7.Location = new System.Drawing.Point(3, 98);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox7.Size = new System.Drawing.Size(528, 335);
            this.groupBox7.TabIndex = 1;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Resource package";
            // 
            // chkCreateDefinitionFile
            // 
            this.chkCreateDefinitionFile.AutoSize = true;
            this.chkCreateDefinitionFile.Checked = true;
            this.chkCreateDefinitionFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCreateDefinitionFile.Location = new System.Drawing.Point(12, 152);
            this.chkCreateDefinitionFile.Name = "chkCreateDefinitionFile";
            this.chkCreateDefinitionFile.Padding = new System.Windows.Forms.Padding(3);
            this.chkCreateDefinitionFile.Size = new System.Drawing.Size(173, 30);
            this.chkCreateDefinitionFile.TabIndex = 8;
            this.chkCreateDefinitionFile.Text = "Create definition file";
            this.chkCreateDefinitionFile.UseVisualStyleBackColor = true;
            this.chkCreateDefinitionFile.CheckedChanged += new System.EventHandler(this.ChkCreateDefinitionFile_CheckedChanged);
            // 
            // splitContainerDefinitionFile
            // 
            this.splitContainerDefinitionFile.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainerDefinitionFile.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainerDefinitionFile.IsSplitterFixed = true;
            this.splitContainerDefinitionFile.Location = new System.Drawing.Point(6, 119);
            this.splitContainerDefinitionFile.Name = "splitContainerDefinitionFile";
            // 
            // splitContainerDefinitionFile.Panel1
            // 
            this.splitContainerDefinitionFile.Panel1.Controls.Add(this.txtDefinitionsLocation);
            // 
            // splitContainerDefinitionFile.Panel2
            // 
            this.splitContainerDefinitionFile.Panel2.Controls.Add(this.btnDefinitionsExplore);
            this.splitContainerDefinitionFile.Panel2.Padding = new System.Windows.Forms.Padding(3, 0, 3, 6);
            this.splitContainerDefinitionFile.Size = new System.Drawing.Size(516, 35);
            this.splitContainerDefinitionFile.SplitterDistance = 413;
            this.splitContainerDefinitionFile.TabIndex = 6;
            // 
            // txtDefinitionsLocation
            // 
            this.txtDefinitionsLocation.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtDefinitionsLocation.Location = new System.Drawing.Point(0, 0);
            this.txtDefinitionsLocation.Name = "txtDefinitionsLocation";
            this.txtDefinitionsLocation.ReadOnly = true;
            this.txtDefinitionsLocation.Size = new System.Drawing.Size(413, 27);
            this.txtDefinitionsLocation.TabIndex = 0;
            // 
            // btnDefinitionsExplore
            // 
            this.btnDefinitionsExplore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDefinitionsExplore.Image = global::ResourcePacker.Properties.Images.folder_explore;
            this.btnDefinitionsExplore.Location = new System.Drawing.Point(3, 0);
            this.btnDefinitionsExplore.Name = "btnDefinitionsExplore";
            this.btnDefinitionsExplore.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnDefinitionsExplore.Size = new System.Drawing.Size(93, 29);
            this.btnDefinitionsExplore.TabIndex = 0;
            this.btnDefinitionsExplore.Text = "Explore";
            this.btnDefinitionsExplore.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDefinitionsExplore.UseVisualStyleBackColor = true;
            this.btnDefinitionsExplore.Click += new System.EventHandler(this.BtnDefinitionsExplore_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(6, 90);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.label4.Size = new System.Drawing.Size(157, 29);
            this.label4.TabIndex = 5;
            this.label4.Text = "Definition file output:";
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer3.IsSplitterFixed = true;
            this.splitContainer3.Location = new System.Drawing.Point(6, 55);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.txtPackageLocation);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.btnPackageExplore);
            this.splitContainer3.Panel2.Padding = new System.Windows.Forms.Padding(3, 0, 3, 6);
            this.splitContainer3.Size = new System.Drawing.Size(516, 35);
            this.splitContainer3.SplitterDistance = 413;
            this.splitContainer3.TabIndex = 4;
            // 
            // txtPackageLocation
            // 
            this.txtPackageLocation.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtPackageLocation.Location = new System.Drawing.Point(0, 0);
            this.txtPackageLocation.Name = "txtPackageLocation";
            this.txtPackageLocation.ReadOnly = true;
            this.txtPackageLocation.Size = new System.Drawing.Size(413, 27);
            this.txtPackageLocation.TabIndex = 0;
            // 
            // btnPackageExplore
            // 
            this.btnPackageExplore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPackageExplore.Image = global::ResourcePacker.Properties.Images.folder_explore;
            this.btnPackageExplore.Location = new System.Drawing.Point(3, 0);
            this.btnPackageExplore.Name = "btnPackageExplore";
            this.btnPackageExplore.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnPackageExplore.Size = new System.Drawing.Size(93, 29);
            this.btnPackageExplore.TabIndex = 0;
            this.btnPackageExplore.Text = "Explore";
            this.btnPackageExplore.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPackageExplore.UseVisualStyleBackColor = true;
            this.btnPackageExplore.Click += new System.EventHandler(this.BtnPackageExplore_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(6, 26);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.label3.Size = new System.Drawing.Size(120, 29);
            this.label3.TabIndex = 3;
            this.label3.Text = "Package output:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkShowPassword);
            this.groupBox2.Controls.Add(this.txtPassword);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(6, 199);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox2.Size = new System.Drawing.Size(516, 130);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Encryption";
            // 
            // chkShowPassword
            // 
            this.chkShowPassword.AutoSize = true;
            this.chkShowPassword.Checked = true;
            this.chkShowPassword.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowPassword.Location = new System.Drawing.Point(12, 91);
            this.chkShowPassword.Name = "chkShowPassword";
            this.chkShowPassword.Padding = new System.Windows.Forms.Padding(3);
            this.chkShowPassword.Size = new System.Drawing.Size(140, 30);
            this.chkShowPassword.TabIndex = 7;
            this.chkShowPassword.Text = "Show password";
            this.chkShowPassword.UseVisualStyleBackColor = true;
            this.chkShowPassword.CheckedChanged += new System.EventHandler(this.ChkShowPassword_CheckedChanged);
            // 
            // txtPassword
            // 
            this.txtPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPassword.Location = new System.Drawing.Point(6, 55);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(6);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(504, 27);
            this.txtPassword.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(6, 26);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.label1.Size = new System.Drawing.Size(119, 29);
            this.label1.TabIndex = 5;
            this.label1.Text = "Enter password:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.splitContainer4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.MaximumSize = new System.Drawing.Size(0, 350);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox1.Size = new System.Drawing.Size(528, 95);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Source";
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer4.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer4.IsSplitterFixed = true;
            this.splitContainer4.Location = new System.Drawing.Point(6, 55);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.txtAssetFolder);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.btnAssetExplore);
            this.splitContainer4.Panel2.Padding = new System.Windows.Forms.Padding(3, 0, 3, 6);
            this.splitContainer4.Size = new System.Drawing.Size(516, 35);
            this.splitContainer4.SplitterDistance = 413;
            this.splitContainer4.TabIndex = 1;
            // 
            // txtAssetFolder
            // 
            this.txtAssetFolder.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtAssetFolder.Location = new System.Drawing.Point(0, 0);
            this.txtAssetFolder.Name = "txtAssetFolder";
            this.txtAssetFolder.ReadOnly = true;
            this.txtAssetFolder.Size = new System.Drawing.Size(413, 27);
            this.txtAssetFolder.TabIndex = 0;
            // 
            // btnAssetExplore
            // 
            this.btnAssetExplore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAssetExplore.Image = global::ResourcePacker.Properties.Images.folder_explore;
            this.btnAssetExplore.Location = new System.Drawing.Point(3, 0);
            this.btnAssetExplore.Name = "btnAssetExplore";
            this.btnAssetExplore.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnAssetExplore.Size = new System.Drawing.Size(93, 29);
            this.btnAssetExplore.TabIndex = 0;
            this.btnAssetExplore.Text = "Explore";
            this.btnAssetExplore.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAssetExplore.UseVisualStyleBackColor = true;
            this.btnAssetExplore.Click += new System.EventHandler(this.BtnAssetExplore_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(6, 26);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.label2.Size = new System.Drawing.Size(97, 29);
            this.label2.TabIndex = 0;
            this.label2.Text = "Asset folder:";
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 3);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(665, 677);
            this.tabControl2.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.splitContainer5);
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(657, 644);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Package";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer5.IsSplitterFixed = true;
            this.splitContainer5.Location = new System.Drawing.Point(3, 3);
            this.splitContainer5.Name = "splitContainer5";
            this.splitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.grpBoxItemSelector);
            this.splitContainer5.Size = new System.Drawing.Size(651, 638);
            this.splitContainer5.SplitterDistance = 32;
            this.splitContainer5.TabIndex = 4;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblAvailableItems,
            this.toolStripSeparator1,
            this.lblSelectedItems});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 2, 6, 2);
            this.toolStrip1.Size = new System.Drawing.Size(651, 32);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // lblAvailableItems
            // 
            this.lblAvailableItems.Name = "lblAvailableItems";
            this.lblAvailableItems.Padding = new System.Windows.Forms.Padding(3);
            this.lblAvailableItems.Size = new System.Drawing.Size(132, 25);
            this.lblAvailableItems.Text = "Available items: 0";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 28);
            // 
            // lblSelectedItems
            // 
            this.lblSelectedItems.Name = "lblSelectedItems";
            this.lblSelectedItems.Padding = new System.Windows.Forms.Padding(3);
            this.lblSelectedItems.Size = new System.Drawing.Size(127, 25);
            this.lblSelectedItems.Text = "Selected items: 0";
            // 
            // grpBoxItemSelector
            // 
            this.grpBoxItemSelector.Controls.Add(this.selectorTreeView);
            this.grpBoxItemSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBoxItemSelector.Location = new System.Drawing.Point(0, 0);
            this.grpBoxItemSelector.Name = "grpBoxItemSelector";
            this.grpBoxItemSelector.Padding = new System.Windows.Forms.Padding(9);
            this.grpBoxItemSelector.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.grpBoxItemSelector.Size = new System.Drawing.Size(651, 602);
            this.grpBoxItemSelector.TabIndex = 3;
            this.grpBoxItemSelector.TabStop = false;
            this.grpBoxItemSelector.Text = "Items to pack";
            // 
            // selectorTreeView
            // 
            this.selectorTreeView.BackColor = System.Drawing.Color.Transparent;
            this.selectorTreeView.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.selectorTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectorTreeView.Location = new System.Drawing.Point(9, 29);
            this.selectorTreeView.Name = "selectorTreeView";
            this.selectorTreeView.Size = new System.Drawing.Size(633, 564);
            this.selectorTreeView.TabIndex = 1;
            this.selectorTreeView.TriStateStyleProperty = ResourcePacker.Controls.TriStateTreeView.TriStateStyles.Installer;
            this.selectorTreeView.NodeStateChanged += new System.EventHandler<System.Windows.Forms.TreeViewEventArgs>(this.ExplorerTreeView_NodeStateChanged);
            this.selectorTreeView.AfterStateChanged += new System.EventHandler<System.Windows.Forms.TreeViewEventArgs>(this.ExplorerTreeView_AfterStateChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(977, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(6, 6, 3, 9);
            this.panel2.Size = new System.Drawing.Size(120, 49);
            this.panel2.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.Location = new System.Drawing.Point(6, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(111, 34);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCreate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(1097, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(3, 6, 9, 9);
            this.panel1.Size = new System.Drawing.Size(120, 49);
            this.panel1.TabIndex = 0;
            // 
            // btnCreate
            // 
            this.btnCreate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCreate.Enabled = false;
            this.btnCreate.Location = new System.Drawing.Point(3, 6);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(108, 34);
            this.btnCreate.TabIndex = 0;
            this.btnCreate.Text = "Create";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.BtnCreate_Click);
            // 
            // CreateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1217, 733);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create new resource package";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CreateForm_FormClosing);
            this.ResizeBegin += new System.EventHandler(this.CreateForm_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.CreateForm_ResizeEnd);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.splitContainer6.Panel1.ResumeLayout(false);
            this.splitContainer6.Panel1.PerformLayout();
            this.splitContainer6.Panel2.ResumeLayout(false);
            this.splitContainer6.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).EndInit();
            this.splitContainer6.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.splitContainerDefinitionFile.Panel1.ResumeLayout(false);
            this.splitContainerDefinitionFile.Panel1.PerformLayout();
            this.splitContainerDefinitionFile.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerDefinitionFile)).EndInit();
            this.splitContainerDefinitionFile.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel1.PerformLayout();
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel1.PerformLayout();
            this.splitContainer5.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.grpBoxItemSelector.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private Panel panel1;
        private Button btnCreate;
        private Panel panel2;
        private Button btnCancel;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabControl tabControl2;
        private TabPage tabPage3;
        private GroupBox groupBox2;
        private CheckBox chkShowPassword;
        private TextBox txtPassword;
        private Label label1;
        private GroupBox grpBoxItemSelector;
        private GroupBox groupBox7;
        private GroupBox groupBox1;
        private SplitContainer splitContainer4;
        private TextBox txtAssetFolder;
        private Button btnAssetExplore;
        private Label label2;
        private SplitContainer splitContainer5;
        private ToolStrip toolStrip1;
        private ToolStripLabel lblAvailableItems;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripLabel lblSelectedItems;
        private Controls.TriStateTreeView selectorTreeView;
        private GroupBox groupBox4;
        private SplitContainer splitContainer6;
        private Label lblStatus;
        private Label lblPercentage;
        private CheckBox chkCreateDefinitionFile;
        private SplitContainer splitContainerDefinitionFile;
        private TextBox txtDefinitionsLocation;
        private Button btnDefinitionsExplore;
        private Label label4;
        private SplitContainer splitContainer3;
        private TextBox txtPackageLocation;
        private Button btnPackageExplore;
        private Label label3;
        private ProgressBar progressBarSecondary;
        private Label lblStatusFile;
        private ProgressBar progressBarPrimary;
    }
}