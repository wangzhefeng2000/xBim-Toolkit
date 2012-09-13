﻿namespace XBim.COBie.Client
{
    partial class COBieGenerator
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
            this.btnBrowseTemplate = new System.Windows.Forms.Button();
            this.txtTemplate = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.ComboBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.RichTextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnBrowseTemplate);
            this.groupBox1.Controls.Add(this.txtTemplate);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtPath);
            this.groupBox1.Controls.Add(this.btnBrowse);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(498, 77);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "IFC File Location";
            // 
            // btnBrowseTemplate
            // 
            this.btnBrowseTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseTemplate.Location = new System.Drawing.Point(417, 44);
            this.btnBrowseTemplate.Name = "btnBrowseTemplate";
            this.btnBrowseTemplate.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseTemplate.TabIndex = 4;
            this.btnBrowseTemplate.Text = "&Browse...";
            this.btnBrowseTemplate.UseVisualStyleBackColor = true;
            this.btnBrowseTemplate.Click += new System.EventHandler(this.btnBrowseTemplate_Click);
            // 
            // txtTemplate
            // 
            this.txtTemplate.FormattingEnabled = true;
            this.txtTemplate.Items.AddRange(new object[] {
            "COBie-UK-2012-template.xls",
            "COBie-US-2_4-template.xls"});
            this.txtTemplate.Location = new System.Drawing.Point(68, 46);
            this.txtTemplate.Name = "txtTemplate";
            this.txtTemplate.Size = new System.Drawing.Size(337, 21);
            this.txtTemplate.TabIndex = 3;
            this.txtTemplate.Text = "COBie-UK-2012-template.xls";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Template:";
            // 
            // txtPath
            // 
            this.txtPath.FormattingEnabled = true;
            this.txtPath.Items.AddRange(new object[] {
            "2012-03-23-Duplex-Design.ifc",
            "2012-03-23-Duplex-Design.xbim",
            "Clinic_A_20110906.ifc",
            "Clinic_A_20110906.xbim",
            "2012-09-03-Clinic-Handover.ifc",
            "2012-09-03-Clinic-Handover.xbim"});
            this.txtPath.Location = new System.Drawing.Point(68, 20);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(337, 21);
            this.txtPath.TabIndex = 0;
            this.txtPath.Text = "2012-03-23-Duplex-Design.ifc";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(417, 18);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "&Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select file:";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerate.Location = new System.Drawing.Point(430, 239);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 2;
            this.btnGenerate.Text = "&Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(430, 209);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "&Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutput.Location = new System.Drawing.Point(13, 96);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(400, 166);
            this.txtOutput.TabIndex = 5;
            this.txtOutput.Text = "";
            // 
            // COBieGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(523, 273);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.groupBox1);
            this.Name = "COBieGenerator";
            this.Text = "Xbim COBie Test Harness";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.RichTextBox txtOutput;
        private System.Windows.Forms.ComboBox txtPath;
        private System.Windows.Forms.Button btnBrowseTemplate;
        private System.Windows.Forms.ComboBox txtTemplate;
        private System.Windows.Forms.Label label2;
    }
}
