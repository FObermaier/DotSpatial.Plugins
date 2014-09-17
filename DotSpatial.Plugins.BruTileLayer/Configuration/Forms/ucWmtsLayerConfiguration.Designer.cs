using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

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
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.lvwWmtsLayers = new System.Windows.Forms.ListView();
            this.chLayer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chFormat = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chStyle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chTileSet = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chCRS = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chAbstract = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // cboWmts
            // 
            this.cboWmts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboWmts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboWmts.FormattingEnabled = true;
            this.cboWmts.Location = new System.Drawing.Point(10, 10);
            this.cboWmts.Name = "cboWmts";
            this.cboWmts.Size = new System.Drawing.Size(451, 21);
            this.cboWmts.TabIndex = 0;
            this.cboWmts.SelectedIndexChanged += new System.EventHandler(this.cboWmts_SelectedIndexChanged);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(10, 37);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 3;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(89, 37);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 4;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(170, 37);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 5;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // lvwWmtsLayers
            // 
            this.lvwWmtsLayers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwWmtsLayers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chLayer,
            this.chFormat,
            this.chStyle,
            this.chTitle,
            this.chAbstract,
            this.chTileSet,
            this.chCRS});
            this.lvwWmtsLayers.FullRowSelect = true;
            this.lvwWmtsLayers.GridLines = true;
            this.lvwWmtsLayers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvwWmtsLayers.Location = new System.Drawing.Point(10, 66);
            this.lvwWmtsLayers.MultiSelect = false;
            this.lvwWmtsLayers.Name = "lvwWmtsLayers";
            this.lvwWmtsLayers.Size = new System.Drawing.Size(451, 206);
            this.lvwWmtsLayers.TabIndex = 7;
            this.lvwWmtsLayers.UseCompatibleStateImageBehavior = false;
            this.lvwWmtsLayers.View = System.Windows.Forms.View.Details;
            // 
            // chLayer
            // 
            this.chLayer.Text = "Layer";
            // 
            // chFormat
            // 
            this.chFormat.Text = "Format";
            // 
            // chStyle
            // 
            this.chStyle.Text = "Style";
            // 
            // chTitle
            // 
            this.chTitle.Text = "Title";
            // 
            // chTileSet
            // 
            this.chTileSet.Text = "TileSet";
            // 
            // chCRS
            // 
            this.chCRS.Text = "CRS";
            // 
            // chAbstract
            // 
            this.chAbstract.Text = "Abstract";
            this.chAbstract.Width = 120;
            // 
            // ucWmtsLayerConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lvwWmtsLayers);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.cboWmts);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "ucWmtsLayerConfiguration";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(471, 280);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cboWmts;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnEdit;

        private void btnNew_Click(object sender, EventArgs e)
        {
            var newWmts = false;
            using (var dlg = new WmsConnection())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    cboWmts.Items.Add(dlg.WmsConnectionInfo);
                    cboWmts.SelectedIndex = cboWmts.Items.Count - 1;
                    newWmts = true;
                }
            }
            if (newWmts) btnConnect.PerformClick();
        }

        //private void cboWmts_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        //{
        //    if (string.IsNullOrEmpty(cboWmts.Text))
        //        return;

        //    if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
        //    {
        //        e.IsInputKey = true;
        //    }
        //}

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (cboWmts.SelectedIndex < 0)
            {
                MessageBox.Show("You need to define/select a Wm(t)s connection first", "Can't conntect",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var clickConnect = false;
            using (var dlg = new WmsConnection())
            {
                dlg.WmsConnectionInfo = (WmsConnectionInfo) cboWmts.SelectedItem;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    cboWmts.Items[cboWmts.SelectedIndex] = dlg.WmsConnectionInfo;
                    clickConnect = true;
                }
            }

            if (clickConnect) btnConnect.PerformClick();
        }

        private ListView lvwWmtsLayers;
        private ColumnHeader chLayer;
        private ColumnHeader chFormat;
        private ColumnHeader chStyle;
        private ColumnHeader chTitle;
        private ColumnHeader chTileSet;
        private ColumnHeader chCRS;
        private ColumnHeader chAbstract;
    }
}
