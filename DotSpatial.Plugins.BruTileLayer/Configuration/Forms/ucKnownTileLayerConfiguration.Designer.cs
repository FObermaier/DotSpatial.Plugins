namespace DotSpatial.Plugins.BruTileLayer.Configuration.Forms
{
    partial class ucKnownTileLayerConfiguration
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
            this.lblKnownOsmMapTypes = new System.Windows.Forms.Label();
            this.cboKnownOsmMapTypes = new System.Windows.Forms.ComboBox();
            this.lblOsmMapTypeToken = new System.Windows.Forms.Label();
            this.txtOsmMapTypeToken = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblKnownOsmMapTypes
            // 
            this.lblKnownOsmMapTypes.AutoSize = true;
            this.lblKnownOsmMapTypes.Location = new System.Drawing.Point(5, 5);
            this.lblKnownOsmMapTypes.Name = "lblKnownOsmMapTypes";
            this.lblKnownOsmMapTypes.Size = new System.Drawing.Size(110, 13);
            this.lblKnownOsmMapTypes.TabIndex = 0;
            this.lblKnownOsmMapTypes.Text = "Known \"slippy\" maps:";
            // 
            // cboKnownOsmMapTypes
            // 
            this.cboKnownOsmMapTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboKnownOsmMapTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboKnownOsmMapTypes.FormattingEnabled = true;
            this.cboKnownOsmMapTypes.Location = new System.Drawing.Point(8, 21);
            this.cboKnownOsmMapTypes.Name = "cboKnownOsmMapTypes";
            this.cboKnownOsmMapTypes.Size = new System.Drawing.Size(414, 21);
            this.cboKnownOsmMapTypes.TabIndex = 1;
            // 
            // lblOsmMapTypeToken
            // 
            this.lblOsmMapTypeToken.AutoSize = true;
            this.lblOsmMapTypeToken.Location = new System.Drawing.Point(8, 56);
            this.lblOsmMapTypeToken.Name = "lblOsmMapTypeToken";
            this.lblOsmMapTypeToken.Size = new System.Drawing.Size(199, 13);
            this.lblOsmMapTypeToken.TabIndex = 0;
            this.lblOsmMapTypeToken.Text = "Token to access the above map source:";
            // 
            // txtOsmMapTypeToken
            // 
            this.txtOsmMapTypeToken.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOsmMapTypeToken.Location = new System.Drawing.Point(8, 72);
            this.txtOsmMapTypeToken.Name = "txtOsmMapTypeToken";
            this.txtOsmMapTypeToken.Size = new System.Drawing.Size(414, 20);
            this.txtOsmMapTypeToken.TabIndex = 2;
            // 
            // ucKnownTileLayerConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtOsmMapTypeToken);
            this.Controls.Add(this.cboKnownOsmMapTypes);
            this.Controls.Add(this.lblOsmMapTypeToken);
            this.Controls.Add(this.lblKnownOsmMapTypes);
            this.Name = "ucKnownTileLayerConfiguration";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(430, 188);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblKnownOsmMapTypes;
        private System.Windows.Forms.ComboBox cboKnownOsmMapTypes;
        private System.Windows.Forms.Label lblOsmMapTypeToken;
        private System.Windows.Forms.TextBox txtOsmMapTypeToken;
    }
}
