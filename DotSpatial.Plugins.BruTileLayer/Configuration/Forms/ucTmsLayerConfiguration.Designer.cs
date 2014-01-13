using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml.Linq;
using BruTile.Tms;

namespace DotSpatial.Plugins.BruTileLayer.Configuration.Forms
{
    partial class ucTmsLayerConfiguration
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
            this.cboProvider = new System.Windows.Forms.ComboBox();
            this.btnAddProvider = new System.Windows.Forms.Button();
            this.btnRemoveProvider = new System.Windows.Forms.Button();
            this.gbProviders = new System.Windows.Forms.GroupBox();
            this.gbServices = new System.Windows.Forms.GroupBox();
            this.dgvServices = new System.Windows.Forms.DataGridView();
            this.gbProviders.SuspendLayout();
            this.gbServices.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvServices)).BeginInit();
            this.SuspendLayout();
            // 
            // cboProvider
            // 
            this.cboProvider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProvider.FormattingEnabled = true;
            this.cboProvider.Location = new System.Drawing.Point(13, 19);
            this.cboProvider.Name = "cboProvider";
            this.cboProvider.Size = new System.Drawing.Size(492, 21);
            this.cboProvider.TabIndex = 2;
            this.cboProvider.SelectedIndexChanged += new System.EventHandler(this.cboProvider_SelectedIndexChanged);
            // 
            // btnAddProvider
            // 
            this.btnAddProvider.Location = new System.Drawing.Point(13, 46);
            this.btnAddProvider.Name = "btnAddProvider";
            this.btnAddProvider.Size = new System.Drawing.Size(117, 23);
            this.btnAddProvider.TabIndex = 6;
            this.btnAddProvider.Text = "Add provider...";
            this.btnAddProvider.UseVisualStyleBackColor = true;
            this.btnAddProvider.Click += new System.EventHandler(this.btnAddProvider_Click);
            // 
            // btnRemoveProvider
            // 
            this.btnRemoveProvider.Enabled = false;
            this.btnRemoveProvider.Location = new System.Drawing.Point(145, 46);
            this.btnRemoveProvider.Name = "btnRemoveProvider";
            this.btnRemoveProvider.Size = new System.Drawing.Size(152, 23);
            this.btnRemoveProvider.TabIndex = 7;
            this.btnRemoveProvider.Text = "Remove selected provider";
            this.btnRemoveProvider.UseVisualStyleBackColor = true;
            this.btnRemoveProvider.Click += new System.EventHandler(this.btnRemoveProvider_Click);
            // 
            // gbProviders
            // 
            this.gbProviders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbProviders.Controls.Add(this.btnAddProvider);
            this.gbProviders.Controls.Add(this.btnRemoveProvider);
            this.gbProviders.Controls.Add(this.cboProvider);
            this.gbProviders.Location = new System.Drawing.Point(3, 3);
            this.gbProviders.Name = "gbProviders";
            this.gbProviders.Size = new System.Drawing.Size(514, 85);
            this.gbProviders.TabIndex = 8;
            this.gbProviders.TabStop = false;
            this.gbProviders.Text = "Providers";
            // 
            // gbServices
            // 
            this.gbServices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbServices.Controls.Add(this.dgvServices);
            this.gbServices.Location = new System.Drawing.Point(3, 94);
            this.gbServices.Name = "gbServices";
            this.gbServices.Size = new System.Drawing.Size(514, 211);
            this.gbServices.TabIndex = 9;
            this.gbServices.TabStop = false;
            this.gbServices.Text = "Services";
            // 
            // dgvServices
            // 
            this.dgvServices.AllowUserToAddRows = false;
            this.dgvServices.AllowUserToDeleteRows = false;
            this.dgvServices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvServices.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvServices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvServices.Location = new System.Drawing.Point(10, 18);
            this.dgvServices.MultiSelect = false;
            this.dgvServices.Name = "dgvServices";
            this.dgvServices.ReadOnly = true;
            this.dgvServices.RowHeadersVisible = false;
            this.dgvServices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvServices.ShowCellErrors = false;
            this.dgvServices.ShowCellToolTips = false;
            this.dgvServices.ShowEditingIcon = false;
            this.dgvServices.ShowRowErrors = false;
            this.dgvServices.Size = new System.Drawing.Size(495, 179);
            this.dgvServices.TabIndex = 6;
            this.dgvServices.SelectionChanged += new System.EventHandler(this.dgvServices_SelectionChanged);
            // 
            // ucTmsLayerConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbProviders);
            this.Controls.Add(this.gbServices);
            this.Name = "ucTmsLayerConfiguration";
            this.Size = new System.Drawing.Size(520, 310);
            this.gbProviders.ResumeLayout(false);
            this.gbServices.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvServices)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cboProvider;
        private System.Windows.Forms.Button btnAddProvider;
        private System.Windows.Forms.Button btnRemoveProvider;
        private System.Windows.Forms.GroupBox gbProviders;
        private System.Windows.Forms.GroupBox gbServices;
        private System.Windows.Forms.DataGridView dgvServices;

        private void cboProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            _initialized = true;
            
            var file = (String)cboProvider.SelectedItem;
            string res = _servicesDirectory + Path.DirectorySeparatorChar + file + ".xml";

            XDocument xdoc = XDocument.Load(res);
            var el = xdoc.Element("Services");
            var el1 = el.Element("TileMapService");

            
            using (var webClient = new WebClient())
            {
                var ms = new MemoryStream(webClient.DownloadData(el1.Attribute("href").Value));
                ms.Seek(0, SeekOrigin.Begin);
                SelectedTileMapService = TileMapService.CreateFromResource(ms);
                ms.Seek(0, SeekOrigin.Begin);
                TmsTileMapServiceXml = XDocument.Load(ms);
            }

            /*
            var request = (HttpWebRequest)WebRequest.Create(el1.Attribute("href").Value);
            request.UserAgent =
                "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.1.14) Gecko/20080404 Firefox/2.0.0.14";
            //request.Referer = ""
            using (var response = request.GetRequestStream())
                SelectedTileMapService = TileMapService.CreateFromResource(response);
            */
            btnRemoveProvider.Enabled = true;

            var tileMaps = new List<TileMapItem>(SelectedTileMapService.TileMaps);
            dgvServices.DataSource = tileMaps;

            //resize columns
            dgvServices.Columns[0].Width=120;
            dgvServices.ClearSelection();

            _initialized = false;
        }

        private void btnRemoveProvider_Click(object sender, EventArgs e)
        {
            var providerFile = (String)cboProvider.SelectedItem;
            var res = Path.Combine(_servicesDirectory, providerFile + ".xml");

            if(File.Exists(res))
            {
                File.Delete(res);
                InitForm();
            }
            else
            {
                MessageBox.Show("File " + providerFile + " does not exist. Cannot remove provider.", "Error");
            }
        }

        private void btnAddProvider_Click(object sender, EventArgs e)
        {
            using (var addProviderForm = new AddProviderForm())
            {
                if (addProviderForm.ShowDialog() == DialogResult.OK)
                {
                    var name = addProviderForm.ProviderName;
                    var url = addProviderForm.ProvidedServiceURL;

                    // Now write an XML file to the services...
                    WriteProviderXml(name, url);

                    // now refresh...
                    InitForm();
                }
            }
        }

        private void dgvServices_SelectionChanged(object sender, EventArgs e)
        {
            if (!_initialized)
            {
                SelectedService = (TileMapItem)dgvServices.CurrentRow.DataBoundItem;
                var tileMaps = TmsTileMapServiceXml.Root.Element("TileMaps");
                if (tileMaps == null)
                    return;
                foreach (var tileMap in tileMaps.Elements("TileMap"))
                {
                    if (SelectedService.Href == tileMap.Attribute("href").Value)
                    {
                        var att = tileMap.Attribute("type");
                        if (att != null && att.Value == "InvertedTMS")
                            Inverted = true;
                        else
                            Inverted = false;
                    }
                }
                //SelectedService.
            }
        }
    }
}