using System;
using System.Windows.Forms;

namespace DotSpatial.Plugins.BruTileLayer.Configuration.Forms
{
    partial class ucGlobalSettings
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
            this.gbPermaCache = new System.Windows.Forms.GroupBox();
            this.lblExpire = new System.Windows.Forms.Label();
            this.nudExpire = new System.Windows.Forms.NumericUpDown();
            this.cboImageFormat = new System.Windows.Forms.ComboBox();
            this.lblFormat = new System.Windows.Forms.Label();
            this.btnPickFolder = new System.Windows.Forms.Button();
            this.txtPermaCacheRoot = new System.Windows.Forms.TextBox();
            this.lblPermaCacheRoot = new System.Windows.Forms.Label();
            this.cboPermaCacheType = new System.Windows.Forms.ComboBox();
            this.lblPermaCacheType = new System.Windows.Forms.Label();
            this.gbVolatileCache = new System.Windows.Forms.GroupBox();
            this.lblMaximum = new System.Windows.Forms.Label();
            this.lblMinimum = new System.Windows.Forms.Label();
            this.nudMaximum = new System.Windows.Forms.NumericUpDown();
            this.nudMinimum = new System.Windows.Forms.NumericUpDown();
            this.gbTileFetcher = new System.Windows.Forms.GroupBox();
            this.chkAsyncMode = new System.Windows.Forms.CheckBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.gbPermaCache.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudExpire)).BeginInit();
            this.gbVolatileCache.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaximum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinimum)).BeginInit();
            this.gbTileFetcher.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbPermaCache
            // 
            this.gbPermaCache.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbPermaCache.Controls.Add(this.lblExpire);
            this.gbPermaCache.Controls.Add(this.nudExpire);
            this.gbPermaCache.Controls.Add(this.cboImageFormat);
            this.gbPermaCache.Controls.Add(this.lblFormat);
            this.gbPermaCache.Controls.Add(this.btnPickFolder);
            this.gbPermaCache.Controls.Add(this.txtPermaCacheRoot);
            this.gbPermaCache.Controls.Add(this.lblPermaCacheRoot);
            this.gbPermaCache.Controls.Add(this.cboPermaCacheType);
            this.gbPermaCache.Controls.Add(this.lblPermaCacheType);
            this.gbPermaCache.Location = new System.Drawing.Point(3, 53);
            this.gbPermaCache.Name = "gbPermaCache";
            this.gbPermaCache.Size = new System.Drawing.Size(467, 127);
            this.gbPermaCache.TabIndex = 5;
            this.gbPermaCache.TabStop = false;
            this.gbPermaCache.Text = "Perma cache";
            // 
            // lblExpire
            // 
            this.lblExpire.AutoSize = true;
            this.lblExpire.Location = new System.Drawing.Point(14, 103);
            this.lblExpire.Name = "lblExpire";
            this.lblExpire.Size = new System.Drawing.Size(169, 13);
            this.lblExpire.TabIndex = 15;
            this.lblExpire.Text = "Number of days before tiles expire:";
            // 
            // nudExpire
            // 
            this.nudExpire.Location = new System.Drawing.Point(207, 101);
            this.nudExpire.Maximum = new decimal(new int[] {
            365,
            0,
            0,
            0});
            this.nudExpire.Name = "nudExpire";
            this.nudExpire.Size = new System.Drawing.Size(55, 20);
            this.nudExpire.TabIndex = 14;
            this.nudExpire.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudExpire.Value = new decimal(new int[] {
            14,
            0,
            0,
            0});
            // 
            // cboImageFormat
            // 
            this.cboImageFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboImageFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboImageFormat.FormattingEnabled = true;
            this.cboImageFormat.Location = new System.Drawing.Point(73, 74);
            this.cboImageFormat.Name = "cboImageFormat";
            this.cboImageFormat.Size = new System.Drawing.Size(388, 21);
            this.cboImageFormat.TabIndex = 13;
            // 
            // lblFormat
            // 
            this.lblFormat.AutoSize = true;
            this.lblFormat.Location = new System.Drawing.Point(12, 77);
            this.lblFormat.Name = "lblFormat";
            this.lblFormat.Size = new System.Drawing.Size(39, 13);
            this.lblFormat.TabIndex = 12;
            this.lblFormat.Text = "Format";
            // 
            // btnPickFolder
            // 
            this.btnPickFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPickFolder.Location = new System.Drawing.Point(436, 45);
            this.btnPickFolder.Name = "btnPickFolder";
            this.btnPickFolder.Size = new System.Drawing.Size(25, 23);
            this.btnPickFolder.TabIndex = 9;
            this.btnPickFolder.Text = "...";
            this.btnPickFolder.UseVisualStyleBackColor = true;
            this.btnPickFolder.Click += new System.EventHandler(this.btnPickFolder_Click);
            // 
            // txtPermaCacheRoot
            // 
            this.txtPermaCacheRoot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPermaCacheRoot.Enabled = false;
            this.txtPermaCacheRoot.Location = new System.Drawing.Point(73, 48);
            this.txtPermaCacheRoot.Name = "txtPermaCacheRoot";
            this.txtPermaCacheRoot.Size = new System.Drawing.Size(357, 20);
            this.txtPermaCacheRoot.TabIndex = 8;
            // 
            // lblPermaCacheRoot
            // 
            this.lblPermaCacheRoot.AutoSize = true;
            this.lblPermaCacheRoot.Location = new System.Drawing.Point(12, 51);
            this.lblPermaCacheRoot.Name = "lblPermaCacheRoot";
            this.lblPermaCacheRoot.Size = new System.Drawing.Size(30, 13);
            this.lblPermaCacheRoot.TabIndex = 7;
            this.lblPermaCacheRoot.Text = "Root";
            // 
            // cboPermaCacheType
            // 
            this.cboPermaCacheType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboPermaCacheType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPermaCacheType.FormattingEnabled = true;
            this.cboPermaCacheType.Location = new System.Drawing.Point(73, 19);
            this.cboPermaCacheType.Name = "cboPermaCacheType";
            this.cboPermaCacheType.Size = new System.Drawing.Size(388, 21);
            this.cboPermaCacheType.TabIndex = 6;
            // 
            // lblPermaCacheType
            // 
            this.lblPermaCacheType.AutoSize = true;
            this.lblPermaCacheType.Location = new System.Drawing.Point(12, 22);
            this.lblPermaCacheType.Name = "lblPermaCacheType";
            this.lblPermaCacheType.Size = new System.Drawing.Size(31, 13);
            this.lblPermaCacheType.TabIndex = 5;
            this.lblPermaCacheType.Text = "Type";
            // 
            // gbVolatileCache
            // 
            this.gbVolatileCache.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbVolatileCache.Controls.Add(this.lblMaximum);
            this.gbVolatileCache.Controls.Add(this.lblMinimum);
            this.gbVolatileCache.Controls.Add(this.nudMaximum);
            this.gbVolatileCache.Controls.Add(this.nudMinimum);
            this.gbVolatileCache.Location = new System.Drawing.Point(3, 0);
            this.gbVolatileCache.Name = "gbVolatileCache";
            this.gbVolatileCache.Size = new System.Drawing.Size(467, 47);
            this.gbVolatileCache.TabIndex = 6;
            this.gbVolatileCache.TabStop = false;
            this.gbVolatileCache.Text = "Memory cache";
            // 
            // lblMaximum
            // 
            this.lblMaximum.AutoSize = true;
            this.lblMaximum.Location = new System.Drawing.Point(170, 21);
            this.lblMaximum.Name = "lblMaximum";
            this.lblMaximum.Size = new System.Drawing.Size(51, 13);
            this.lblMaximum.TabIndex = 7;
            this.lblMaximum.Text = "Maximum";
            // 
            // lblMinimum
            // 
            this.lblMinimum.AutoSize = true;
            this.lblMinimum.Location = new System.Drawing.Point(12, 21);
            this.lblMinimum.Name = "lblMinimum";
            this.lblMinimum.Size = new System.Drawing.Size(48, 13);
            this.lblMinimum.TabIndex = 6;
            this.lblMinimum.Text = "Minimum";
            // 
            // nudMaximum
            // 
            this.nudMaximum.Increment = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.nudMaximum.Location = new System.Drawing.Point(224, 19);
            this.nudMaximum.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.nudMaximum.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudMaximum.Name = "nudMaximum";
            this.nudMaximum.Size = new System.Drawing.Size(75, 20);
            this.nudMaximum.TabIndex = 1;
            this.nudMaximum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudMaximum.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // nudMinimum
            // 
            this.nudMinimum.Increment = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.nudMinimum.Location = new System.Drawing.Point(73, 19);
            this.nudMinimum.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.nudMinimum.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudMinimum.Name = "nudMinimum";
            this.nudMinimum.Size = new System.Drawing.Size(75, 20);
            this.nudMinimum.TabIndex = 0;
            this.nudMinimum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudMinimum.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudMinimum.ValueChanged += new System.EventHandler(this.nudMinimum_ValueChanged);
            // 
            // gbTileFetcher
            // 
            this.gbTileFetcher.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbTileFetcher.Controls.Add(this.chkAsyncMode);
            this.gbTileFetcher.Location = new System.Drawing.Point(3, 186);
            this.gbTileFetcher.Name = "gbTileFetcher";
            this.gbTileFetcher.Size = new System.Drawing.Size(467, 43);
            this.gbTileFetcher.TabIndex = 7;
            this.gbTileFetcher.TabStop = false;
            this.gbTileFetcher.Text = "Tile fetcher";
            // 
            // chkAsyncMode
            // 
            this.chkAsyncMode.AutoSize = true;
            this.chkAsyncMode.Location = new System.Drawing.Point(15, 19);
            this.chkAsyncMode.Name = "chkAsyncMode";
            this.chkAsyncMode.Size = new System.Drawing.Size(168, 17);
            this.chkAsyncMode.TabIndex = 0;
            this.chkAsyncMode.Text = "Use tile fetcher in async mode";
            this.chkAsyncMode.UseVisualStyleBackColor = true;
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Location = new System.Drawing.Point(395, 235);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 8;
            this.btnApply.Text = "&Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // ucGlobalSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.gbTileFetcher);
            this.Controls.Add(this.gbVolatileCache);
            this.Controls.Add(this.gbPermaCache);
            this.Name = "ucGlobalSettings";
            this.Size = new System.Drawing.Size(477, 262);
            this.gbPermaCache.ResumeLayout(false);
            this.gbPermaCache.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudExpire)).EndInit();
            this.gbVolatileCache.ResumeLayout(false);
            this.gbVolatileCache.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaximum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinimum)).EndInit();
            this.gbTileFetcher.ResumeLayout(false);
            this.gbTileFetcher.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbPermaCache;
        private System.Windows.Forms.Button btnPickFolder;
        private System.Windows.Forms.TextBox txtPermaCacheRoot;
        private System.Windows.Forms.Label lblPermaCacheRoot;
        private System.Windows.Forms.ComboBox cboPermaCacheType;
        private System.Windows.Forms.Label lblPermaCacheType;
        private System.Windows.Forms.GroupBox gbVolatileCache;
        private System.Windows.Forms.Label lblMaximum;
        private System.Windows.Forms.Label lblMinimum;
        private System.Windows.Forms.NumericUpDown nudMaximum;
        private System.Windows.Forms.NumericUpDown nudMinimum;
        private System.Windows.Forms.GroupBox gbTileFetcher;
        private System.Windows.Forms.CheckBox chkAsyncMode;
        private System.Windows.Forms.Button btnApply;

        private void btnPickFolder_Click(object sender, System.EventArgs e)
        {
            using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
            {
                fbd.SelectedPath = txtPermaCacheRoot.Text;
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtPermaCacheRoot.Text = fbd.SelectedPath;
                }
            }
        }

        private void btnApply_Click(object sender, System.EventArgs e)
        {
            var settings = BruTileLayerPlugin.Settings;

            settings.UseAsyncMode = chkAsyncMode.Checked;

            settings.PermaCacheType = (PermaCacheType)cboPermaCacheType.SelectedIndex;
            settings.PermaCacheRoot = txtPermaCacheRoot.Text;
            settings.PermaCacheExpireInDays = (int) nudMinimum.Value;

            settings.MemoryCacheMinimum = (int)nudMinimum.Value;
            settings.MemoryCacheMaximum = (int)nudMaximum.Value;

        }

        private void nudMinimum_ValueChanged(object sender, System.EventArgs e)
        {
            nudMaximum.Minimum = nudMinimum.Value + 25;
        }

        private Label lblExpire;
        private NumericUpDown nudExpire;
        private ComboBox cboImageFormat;
        private Label lblFormat;
    }
}
