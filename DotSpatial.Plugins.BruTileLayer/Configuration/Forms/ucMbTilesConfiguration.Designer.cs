using System;
using System.IO;
using System.Windows.Forms;

namespace DotSpatial.Plugins.BruTileLayer.Configuration.Forms
{
    partial class ucMbTilesConfiguration
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
            this.txtMbTilesFile = new System.Windows.Forms.TextBox();
            this.btnMbTilesFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblRootFolder
            // 
            this.lblRootFolder.AutoSize = true;
            this.lblRootFolder.Location = new System.Drawing.Point(3, 0);
            this.lblRootFolder.Name = "lblRootFolder";
            this.lblRootFolder.Size = new System.Drawing.Size(60, 13);
            this.lblRootFolder.TabIndex = 0;
            this.lblRootFolder.Text = "MbTiles file";
            // 
            // txtMbTilesFile
            // 
            this.txtMbTilesFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMbTilesFile.Location = new System.Drawing.Point(6, 16);
            this.txtMbTilesFile.Name = "txtMbTilesFile";
            this.txtMbTilesFile.Size = new System.Drawing.Size(373, 20);
            this.txtMbTilesFile.TabIndex = 1;
            this.txtMbTilesFile.Click += new System.EventHandler(this.btnMbTilesFile_Click);
            // 
            // btnMbTilesFile
            // 
            this.btnMbTilesFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMbTilesFile.Location = new System.Drawing.Point(385, 14);
            this.btnMbTilesFile.Name = "btnMbTilesFile";
            this.btnMbTilesFile.Size = new System.Drawing.Size(24, 23);
            this.btnMbTilesFile.TabIndex = 4;
            this.btnMbTilesFile.Text = "...";
            this.btnMbTilesFile.UseVisualStyleBackColor = true;
            this.btnMbTilesFile.Click += new System.EventHandler(this.btnMbTilesFile_Click);
            // 
            // ucMbTilesConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnMbTilesFile);
            this.Controls.Add(this.txtMbTilesFile);
            this.Controls.Add(this.lblRootFolder);
            this.Name = "ucMbTilesConfiguration";
            this.Size = new System.Drawing.Size(412, 187);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblRootFolder;
        private System.Windows.Forms.TextBox txtMbTilesFile;
        private System.Windows.Forms.Button btnMbTilesFile;

        private void btnMbTilesFile_Click(object sender, EventArgs e)
        {
            using (var bfd = new OpenFileDialog())
            {
                bfd.FileName = string.IsNullOrEmpty(txtMbTilesFile.Text)
                                       ? Path.GetPathRoot(Application.LocalUserAppDataPath)
                                       : txtMbTilesFile.Text;
                bfd.Filter = "MbTiles file (*.mbtiles)|*.mbtiles|All files (*.*)|*.*";
                bfd.FilterIndex = 0;
                bfd.CheckFileExists = true;

                if (bfd.ShowDialog() == DialogResult.OK)
                    txtMbTilesFile.Text = bfd.FileName;
            }
        }
    }
}
