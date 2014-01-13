using System;

namespace DotSpatial.Plugins.BruTileLayer.Configuration.Forms
{
    partial class BruTileLayerDialog
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
            this.tpBruTile = new System.Windows.Forms.TabControl();
            this.tpAdd = new System.Windows.Forms.TabPage();
            this.ucConfigurationContainer1 = new DotSpatial.Plugins.BruTileLayer.Configuration.Forms.ucConfigurationContainer();
            this.tpSettings = new System.Windows.Forms.TabPage();
            this.ucGlobalSettings1 = new DotSpatial.Plugins.BruTileLayer.Configuration.Forms.ucGlobalSettings();
            this.tpInfo = new System.Windows.Forms.TabPage();
            this.ucInfo1 = new DotSpatial.Plugins.BruTileLayer.Configuration.Forms.ucInfo();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tpBruTile.SuspendLayout();
            this.tpAdd.SuspendLayout();
            this.tpSettings.SuspendLayout();
            this.tpInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // tpBruTile
            // 
            this.tpBruTile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tpBruTile.Controls.Add(this.tpAdd);
            this.tpBruTile.Controls.Add(this.tpSettings);
            this.tpBruTile.Controls.Add(this.tpInfo);
            this.tpBruTile.Location = new System.Drawing.Point(12, 12);
            this.tpBruTile.Name = "tpBruTile";
            this.tpBruTile.SelectedIndex = 0;
            this.tpBruTile.Size = new System.Drawing.Size(421, 393);
            this.tpBruTile.TabIndex = 0;
            // 
            // tpAdd
            // 
            this.tpAdd.Controls.Add(this.ucConfigurationContainer1);
            this.tpAdd.Location = new System.Drawing.Point(4, 22);
            this.tpAdd.Name = "tpAdd";
            this.tpAdd.Padding = new System.Windows.Forms.Padding(3);
            this.tpAdd.Size = new System.Drawing.Size(413, 367);
            this.tpAdd.TabIndex = 0;
            this.tpAdd.Text = "Add";
            this.tpAdd.UseVisualStyleBackColor = true;
            // 
            // ucConfigurationContainer1
            // 
            this.ucConfigurationContainer1.AddBruTileLayer = null;
            this.ucConfigurationContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucConfigurationContainer1.Location = new System.Drawing.Point(3, 3);
            this.ucConfigurationContainer1.Name = "ucConfigurationContainer1";
            this.ucConfigurationContainer1.Size = new System.Drawing.Size(407, 361);
            this.ucConfigurationContainer1.TabIndex = 0;
            this.ucConfigurationContainer1.Load += new System.EventHandler(this.ucConfigurationContainer1_Load);
            // 
            // tpSettings
            // 
            this.tpSettings.Controls.Add(this.ucGlobalSettings1);
            this.tpSettings.Location = new System.Drawing.Point(4, 22);
            this.tpSettings.Name = "tpSettings";
            this.tpSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tpSettings.Size = new System.Drawing.Size(413, 367);
            this.tpSettings.TabIndex = 1;
            this.tpSettings.Text = "Settings";
            this.tpSettings.UseVisualStyleBackColor = true;
            // 
            // ucGlobalSettings1
            // 
            this.ucGlobalSettings1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucGlobalSettings1.Location = new System.Drawing.Point(3, 3);
            this.ucGlobalSettings1.Name = "ucGlobalSettings1";
            this.ucGlobalSettings1.Size = new System.Drawing.Size(407, 361);
            this.ucGlobalSettings1.TabIndex = 0;
            // 
            // tpInfo
            // 
            this.tpInfo.Controls.Add(this.ucInfo1);
            this.tpInfo.Location = new System.Drawing.Point(4, 22);
            this.tpInfo.Name = "tpInfo";
            this.tpInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tpInfo.Size = new System.Drawing.Size(413, 367);
            this.tpInfo.TabIndex = 2;
            this.tpInfo.Text = "Info";
            this.tpInfo.UseVisualStyleBackColor = true;
            // 
            // ucInfo1
            // 
            this.ucInfo1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucInfo1.Location = new System.Drawing.Point(3, 3);
            this.ucInfo1.Name = "ucInfo1";
            this.ucInfo1.Size = new System.Drawing.Size(407, 361);
            this.ucInfo1.TabIndex = 1;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(358, 413);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btn_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(277, 413);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btn_Click);
            // 
            // BruTileLayerDialog
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(445, 448);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.tpBruTile);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(301, 301);
            this.Name = "BruTileLayerDialog";
            this.ShowInTaskbar = false;
            this.Text = "BruTile Layer Dialog";
            this.tpBruTile.ResumeLayout(false);
            this.tpAdd.ResumeLayout(false);
            this.tpSettings.ResumeLayout(false);
            this.tpInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tpBruTile;
        private System.Windows.Forms.TabPage tpAdd;
        private System.Windows.Forms.TabPage tpSettings;
        private System.Windows.Forms.TabPage tpInfo;
        private ucInfo ucInfo1;
        private ucGlobalSettings ucGlobalSettings1;
        private System.Windows.Forms.Button btnOk;
        private ucConfigurationContainer ucConfigurationContainer1;
        private System.Windows.Forms.Button btnCancel;

        private void btn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}