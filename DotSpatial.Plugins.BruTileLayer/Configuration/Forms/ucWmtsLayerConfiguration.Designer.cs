namespace DotSpatial.Plugins.BruTileLayer.Configuration.Forms
{
    partial class ucWmtsLayerConfiguration
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
            this.cboWmts = new System.Windows.Forms.ComboBox();
            this.lblUrl = new System.Windows.Forms.Label();
            this.tvwWmtsLayers = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // cboWmts
            // 
            this.cboWmts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboWmts.FormattingEnabled = true;
            this.cboWmts.Location = new System.Drawing.Point(76, 10);
            this.cboWmts.Name = "cboWmts";
            this.cboWmts.Size = new System.Drawing.Size(385, 21);
            this.cboWmts.TabIndex = 0;
            this.cboWmts.SelectedIndexChanged += new System.EventHandler(this.cboWmts_SelectedIndexChanged);
            this.cboWmts.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboWmts_KeyDown);
            this.cboWmts.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.cboWmts_PreviewKeyDown);
            // 
            // lblUrl
            // 
            this.lblUrl.AutoSize = true;
            this.lblUrl.Location = new System.Drawing.Point(7, 13);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(59, 13);
            this.lblUrl.TabIndex = 1;
            this.lblUrl.Text = "Wmts-URL";
            // 
            // tvwWmtsLayers
            // 
            this.tvwWmtsLayers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvwWmtsLayers.Location = new System.Drawing.Point(10, 39);
            this.tvwWmtsLayers.Margin = new System.Windows.Forms.Padding(5);
            this.tvwWmtsLayers.Name = "tvwWmtsLayers";
            this.tvwWmtsLayers.Size = new System.Drawing.Size(451, 158);
            this.tvwWmtsLayers.TabIndex = 2;
            // 
            // ucWmtsLayerConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tvwWmtsLayers);
            this.Controls.Add(this.lblUrl);
            this.Controls.Add(this.cboWmts);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "ucWmtsLayerConfiguration";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(471, 207);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboWmts;
        private System.Windows.Forms.Label lblUrl;
        private System.Windows.Forms.TreeView tvwWmtsLayers;
    }
}
