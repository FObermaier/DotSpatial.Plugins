namespace DotSpatial.Plugins.BruTileLayer.Configuration.Forms
{
    partial class BruTileLayerDisplayProperties
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
            this.gbTransparency = new System.Windows.Forms.GroupBox();
            this.tbTransparency = new System.Windows.Forms.TrackBar();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.gbTransparency.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbTransparency)).BeginInit();
            this.SuspendLayout();
            // 
            // gbTransparency
            // 
            this.gbTransparency.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbTransparency.Controls.Add(this.tbTransparency);
            this.gbTransparency.Location = new System.Drawing.Point(12, 12);
            this.gbTransparency.Name = "gbTransparency";
            this.gbTransparency.Size = new System.Drawing.Size(354, 79);
            this.gbTransparency.TabIndex = 0;
            this.gbTransparency.TabStop = false;
            this.gbTransparency.Text = "Transparency";
            // 
            // tbTransparency
            // 
            this.tbTransparency.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTransparency.Location = new System.Drawing.Point(6, 19);
            this.tbTransparency.Maximum = 100;
            this.tbTransparency.Name = "tbTransparency";
            this.tbTransparency.Size = new System.Drawing.Size(342, 45);
            this.tbTransparency.TabIndex = 0;
            this.tbTransparency.TickFrequency = 5;
            this.tbTransparency.Scroll += new System.EventHandler(this.tbTransparency_Scroll);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(210, 100);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(291, 100);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // BruTileLayerDisplayProperties
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(378, 135);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gbTransparency);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(394, 173);
            this.Name = "BruTileLayerDisplayProperties";
            this.ShowInTaskbar = false;
            this.Text = "BruTileLayerDisplayProperties";
            this.gbTransparency.ResumeLayout(false);
            this.gbTransparency.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbTransparency)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbTransparency;
        private System.Windows.Forms.TrackBar tbTransparency;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;

        private void tbTransparency_Scroll(object sender, System.EventArgs e)
        {
            BruTileLayer.Transparency = tbTransparency.Value;
        }
    }
}