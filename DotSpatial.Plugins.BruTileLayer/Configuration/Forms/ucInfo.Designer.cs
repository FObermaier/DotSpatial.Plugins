using System.Diagnostics;
using System.Windows.Forms;

namespace DotSpatial.Plugins.BruTileLayer.Configuration.Forms
{
    partial class ucInfo
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
            this.lnkBruTile = new System.Windows.Forms.LinkLabel();
            this.lblName = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblVersionValue = new System.Windows.Forms.Label();
            this.lblCompany = new System.Windows.Forms.Label();
            this.lblCompanyValue = new System.Windows.Forms.Label();
            this.lblCopyrightValue = new System.Windows.Forms.Label();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescriptionValue = new System.Windows.Forms.TextBox();
            this.lblBruTileVersion = new System.Windows.Forms.Label();
            this.lblBruTileVersionValue = new System.Windows.Forms.Label();
            this.picBruTile = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picBruTile)).BeginInit();
            this.SuspendLayout();
            // 
            // lnkBruTile
            // 
            this.lnkBruTile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkBruTile.AutoSize = true;
            this.lnkBruTile.LinkArea = new System.Windows.Forms.LinkArea(22, 30);
            this.lnkBruTile.Location = new System.Drawing.Point(162, 142);
            this.lnkBruTile.Name = "lnkBruTile";
            this.lnkBruTile.Size = new System.Drawing.Size(153, 17);
            this.lnkBruTile.TabIndex = 2;
            this.lnkBruTile.TabStop = true;
            this.lnkBruTile.Text = "Visit BruTile site on CodePlex";
            this.lnkBruTile.UseCompatibleTextRendering = true;
            this.lnkBruTile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkBruTile_LinkClicked);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(109, 3);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(44, 13);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "Produkt";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(109, 16);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(45, 13);
            this.lblVersion.TabIndex = 4;
            this.lblVersion.Text = "Version:";
            // 
            // lblVersionValue
            // 
            this.lblVersionValue.AutoSize = true;
            this.lblVersionValue.Location = new System.Drawing.Point(160, 16);
            this.lblVersionValue.Name = "lblVersionValue";
            this.lblVersionValue.Size = new System.Drawing.Size(40, 13);
            this.lblVersionValue.TabIndex = 5;
            this.lblVersionValue.Text = "1.2.3.4";
            // 
            // lblCompany
            // 
            this.lblCompany.AutoSize = true;
            this.lblCompany.Location = new System.Drawing.Point(109, 29);
            this.lblCompany.Name = "lblCompany";
            this.lblCompany.Size = new System.Drawing.Size(54, 13);
            this.lblCompany.TabIndex = 6;
            this.lblCompany.Text = "Company:";
            // 
            // lblCompanyValue
            // 
            this.lblCompanyValue.AutoSize = true;
            this.lblCompanyValue.Location = new System.Drawing.Point(160, 29);
            this.lblCompanyValue.Name = "lblCompanyValue";
            this.lblCompanyValue.Size = new System.Drawing.Size(86, 13);
            this.lblCompanyValue.TabIndex = 7;
            this.lblCompanyValue.Text = "DotSpatial-Team";
            // 
            // lblCopyrightValue
            // 
            this.lblCopyrightValue.AutoSize = true;
            this.lblCopyrightValue.Location = new System.Drawing.Point(128, 55);
            this.lblCopyrightValue.Name = "lblCopyrightValue";
            this.lblCopyrightValue.Size = new System.Drawing.Size(161, 13);
            this.lblCopyrightValue.TabIndex = 9;
            this.lblCopyrightValue.Text = "(c) Felix Obermaier for DotSpatial";
            // 
            // lblCopyright
            // 
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.Location = new System.Drawing.Point(109, 42);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(54, 13);
            this.lblCopyright.TabIndex = 8;
            this.lblCopyright.Text = "Copyright:";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(109, 68);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(63, 13);
            this.lblDescription.TabIndex = 10;
            this.lblDescription.Text = "Description:";
            // 
            // txtDescriptionValue
            // 
            this.txtDescriptionValue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescriptionValue.BackColor = System.Drawing.SystemColors.Window;
            this.txtDescriptionValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDescriptionValue.Enabled = false;
            this.txtDescriptionValue.Location = new System.Drawing.Point(112, 82);
            this.txtDescriptionValue.Multiline = true;
            this.txtDescriptionValue.Name = "txtDescriptionValue";
            this.txtDescriptionValue.Size = new System.Drawing.Size(203, 57);
            this.txtDescriptionValue.TabIndex = 11;
            this.txtDescriptionValue.Text = "Sample description";
            // 
            // lblBruTileVersion
            // 
            this.lblBruTileVersion.AutoSize = true;
            this.lblBruTileVersion.Location = new System.Drawing.Point(3, 105);
            this.lblBruTileVersion.Name = "lblBruTileVersion";
            this.lblBruTileVersion.Size = new System.Drawing.Size(81, 13);
            this.lblBruTileVersion.TabIndex = 12;
            this.lblBruTileVersion.Text = "BruTile Version:";
            // 
            // lblBruTileVersionValue
            // 
            this.lblBruTileVersionValue.AutoSize = true;
            this.lblBruTileVersionValue.Location = new System.Drawing.Point(72, 118);
            this.lblBruTileVersionValue.Name = "lblBruTileVersionValue";
            this.lblBruTileVersionValue.Size = new System.Drawing.Size(31, 13);
            this.lblBruTileVersionValue.TabIndex = 13;
            this.lblBruTileVersionValue.Text = "0.6.4";
            this.lblBruTileVersionValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // picBruTile
            // 
            this.picBruTile.Image = global::DotSpatial.Plugins.BruTileLayer.Properties.Resources.BruTileLogoBig;
            this.picBruTile.Location = new System.Drawing.Point(3, 3);
            this.picBruTile.Name = "picBruTile";
            this.picBruTile.Size = new System.Drawing.Size(100, 99);
            this.picBruTile.TabIndex = 1;
            this.picBruTile.TabStop = false;
            // 
            // ucInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblBruTileVersionValue);
            this.Controls.Add(this.lblBruTileVersion);
            this.Controls.Add(this.txtDescriptionValue);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblCopyrightValue);
            this.Controls.Add(this.lblCopyright);
            this.Controls.Add(this.lblCompanyValue);
            this.Controls.Add(this.lblCompany);
            this.Controls.Add(this.lblVersionValue);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lnkBruTile);
            this.Controls.Add(this.picBruTile);
            this.Name = "ucInfo";
            this.Size = new System.Drawing.Size(318, 163);
            ((System.ComponentModel.ISupportInitialize)(this.picBruTile)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picBruTile;
        private System.Windows.Forms.LinkLabel lnkBruTile;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblVersionValue;
        private System.Windows.Forms.Label lblCompany;
        private System.Windows.Forms.Label lblCompanyValue;
        private System.Windows.Forms.Label lblCopyrightValue;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescriptionValue;
        private System.Windows.Forms.Label lblBruTileVersion;
        private System.Windows.Forms.Label lblBruTileVersionValue;

        private void lnkBruTile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            var psi = new ProcessStartInfo("http://brutile.codeplex.com/");
            Process.Start(psi);
        }
    }
}
