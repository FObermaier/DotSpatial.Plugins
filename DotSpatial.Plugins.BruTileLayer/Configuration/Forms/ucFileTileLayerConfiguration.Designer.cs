using System;
using System.IO;
using System.Windows.Forms;

namespace DotSpatial.Plugins.BruTileLayer.Configuration.Forms
{
    partial class ucFileTileLayerConfiguration
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblRootFolder = new System.Windows.Forms.Label();
            this.txtRootFolder = new System.Windows.Forms.TextBox();
            this.lblImageFormat = new System.Windows.Forms.Label();
            this.cboImageFormat = new System.Windows.Forms.ComboBox();
            this.btnRootFolder = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblRootFolder
            // 
            this.lblRootFolder.AutoSize = true;
            this.lblRootFolder.Location = new System.Drawing.Point(3, 0);
            this.lblRootFolder.Name = "lblRootFolder";
            this.lblRootFolder.Size = new System.Drawing.Size(62, 13);
            this.lblRootFolder.TabIndex = 0;
            this.lblRootFolder.Text = "Root Folder";
            // 
            // txtRootFolder
            // 
            this.txtRootFolder.Location = new System.Drawing.Point(6, 16);
            this.txtRootFolder.Name = "txtRootFolder";
            this.txtRootFolder.Size = new System.Drawing.Size(373, 20);
            this.txtRootFolder.TabIndex = 1;
            // 
            // lblImageFormat
            // 
            this.lblImageFormat.AutoSize = true;
            this.lblImageFormat.Location = new System.Drawing.Point(3, 45);
            this.lblImageFormat.Name = "lblImageFormat";
            this.lblImageFormat.Size = new System.Drawing.Size(68, 13);
            this.lblImageFormat.TabIndex = 2;
            this.lblImageFormat.Text = "Image format";
            // 
            // cboImageFormat
            // 
            this.cboImageFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboImageFormat.FormattingEnabled = true;
            this.cboImageFormat.Location = new System.Drawing.Point(89, 42);
            this.cboImageFormat.Name = "cboImageFormat";
            this.cboImageFormat.Size = new System.Drawing.Size(290, 21);
            this.cboImageFormat.TabIndex = 3;
            // 
            // btnRootFolder
            // 
            this.btnRootFolder.Location = new System.Drawing.Point(385, 14);
            this.btnRootFolder.Name = "btnRootFolder";
            this.btnRootFolder.Size = new System.Drawing.Size(24, 23);
            this.btnRootFolder.TabIndex = 4;
            this.btnRootFolder.Text = "...";
            this.btnRootFolder.UseVisualStyleBackColor = true;
            this.btnRootFolder.Click += new System.EventHandler(this.btnRootFolder_Click);
            // 
            // ucFileTileProvider
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnRootFolder);
            this.Controls.Add(this.cboImageFormat);
            this.Controls.Add(this.lblImageFormat);
            this.Controls.Add(this.txtRootFolder);
            this.Controls.Add(this.lblRootFolder);
            this.Name = "ucFileTileProvider";
            this.Size = new System.Drawing.Size(412, 187);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblRootFolder;
        private System.Windows.Forms.TextBox txtRootFolder;
        private System.Windows.Forms.Label lblImageFormat;
        private System.Windows.Forms.ComboBox cboImageFormat;
        private System.Windows.Forms.Button btnRootFolder;

        private void btnRootFolder_Click(object sender, EventArgs e)
        {
            using (var bfd = new FolderBrowserDialog())
            {
                bfd.SelectedPath = string.IsNullOrEmpty(txtRootFolder.Text)
                                       ? Path.GetPathRoot(Application.LocalUserAppDataPath)
                                       : txtRootFolder.Text;

                if (bfd.ShowDialog() == DialogResult.OK)
                    txtRootFolder.Text = bfd.SelectedPath;
            }
        }
    }
}
