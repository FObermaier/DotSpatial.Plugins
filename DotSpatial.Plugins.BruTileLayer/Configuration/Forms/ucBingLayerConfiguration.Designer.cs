namespace DotSpatial.Plugins.BruTileLayer.Configuration.Forms
{
    partial class ucBingLayerConfiguration
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
            this.lblBingMapsType = new System.Windows.Forms.Label();
            this.cboBingMapsType = new System.Windows.Forms.ComboBox();
            this.lblBingMapsToken = new System.Windows.Forms.Label();
            this.txtBingMapsToken = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblBingMapsType
            // 
            this.lblBingMapsType.AutoSize = true;
            this.lblBingMapsType.Location = new System.Drawing.Point(0, 0);
            this.lblBingMapsType.Name = "lblBingMapsType";
            this.lblBingMapsType.Size = new System.Drawing.Size(83, 13);
            this.lblBingMapsType.TabIndex = 0;
            this.lblBingMapsType.Text = "Bing Maps type:";
            // 
            // cboBingMapsType
            // 
            this.cboBingMapsType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboBingMapsType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBingMapsType.FormattingEnabled = true;
            this.cboBingMapsType.Location = new System.Drawing.Point(3, 16);
            this.cboBingMapsType.Name = "cboBingMapsType";
            this.cboBingMapsType.Size = new System.Drawing.Size(424, 21);
            this.cboBingMapsType.TabIndex = 1;
            // 
            // lblBingMapsToken
            // 
            this.lblBingMapsToken.AutoSize = true;
            this.lblBingMapsToken.Location = new System.Drawing.Point(3, 40);
            this.lblBingMapsToken.Name = "lblBingMapsToken";
            this.lblBingMapsToken.Size = new System.Drawing.Size(146, 13);
            this.lblBingMapsToken.TabIndex = 0;
            this.lblBingMapsToken.Text = "Token for Bing Maps access:";
            // 
            // txtBingMapsToken
            // 
            this.txtBingMapsToken.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBingMapsToken.Location = new System.Drawing.Point(3, 56);
            this.txtBingMapsToken.Name = "txtBingMapsToken";
            this.txtBingMapsToken.Size = new System.Drawing.Size(424, 20);
            this.txtBingMapsToken.TabIndex = 2;
            // 
            // ucBingLayerConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtBingMapsToken);
            this.Controls.Add(this.cboBingMapsType);
            this.Controls.Add(this.lblBingMapsToken);
            this.Controls.Add(this.lblBingMapsType);
            this.Name = "ucBingLayerConfiguration";
            this.Size = new System.Drawing.Size(430, 188);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblBingMapsType;
        private System.Windows.Forms.ComboBox cboBingMapsType;
        private System.Windows.Forms.Label lblBingMapsToken;
        private System.Windows.Forms.TextBox txtBingMapsToken;
    }
}
