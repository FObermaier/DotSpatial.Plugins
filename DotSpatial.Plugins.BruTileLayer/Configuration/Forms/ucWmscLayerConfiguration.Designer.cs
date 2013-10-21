using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml.Linq;
using BruTile;
using BruTile.Extensions;
using BruTile.Web;

namespace DotSpatial.Plugins.BruTileLayer.Configuration.Forms
{
    partial class ucWmscLayerConfiguration
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbWmsCUrl = new System.Windows.Forms.ComboBox();
            this.gbLayers = new System.Windows.Forms.GroupBox();
            this.btnRetrieve = new System.Windows.Forms.Button();
            this.lbServices = new System.Windows.Forms.ListBox();
            this.gbLayers.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "WMS-C URL: ";
            // 
            // tbWmsCUrl
            // 
            this.tbWmsCUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbWmsCUrl.Location = new System.Drawing.Point(85, -3);
            this.tbWmsCUrl.Name = "tbWmsCUrl";
            this.tbWmsCUrl.Size = new System.Drawing.Size(671, 21);
            this.tbWmsCUrl.TabIndex = 14;
            this.tbWmsCUrl.Text = "http://www.idee.es/wms-c/IDEE-Base/IDEE-Base?version=1.1.1&request=GetCapabilitie" +
    "s&service=wms-c";
            // 
            // gbLayers
            // 
            this.gbLayers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbLayers.Controls.Add(this.btnRetrieve);
            this.gbLayers.Controls.Add(this.lbServices);
            this.gbLayers.Location = new System.Drawing.Point(6, 23);
            this.gbLayers.Name = "gbLayers";
            this.gbLayers.Size = new System.Drawing.Size(750, 370);
            this.gbLayers.TabIndex = 19;
            this.gbLayers.TabStop = false;
            this.gbLayers.Text = "Server Layers";
            // 
            // btnRetrieve
            // 
            this.btnRetrieve.Location = new System.Drawing.Point(6, 19);
            this.btnRetrieve.Name = "btnRetrieve";
            this.btnRetrieve.Size = new System.Drawing.Size(165, 23);
            this.btnRetrieve.TabIndex = 0;
            this.btnRetrieve.Text = "Get Layers";
            this.btnRetrieve.UseVisualStyleBackColor = true;
            this.btnRetrieve.Click += new System.EventHandler(this.btnRetrieve_Click);
            // 
            // lbServices
            // 
            this.lbServices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbServices.FormattingEnabled = true;
            this.lbServices.Location = new System.Drawing.Point(6, 52);
            this.lbServices.Name = "lbServices";
            this.lbServices.Size = new System.Drawing.Size(738, 303);
            this.lbServices.TabIndex = 3;
            this.lbServices.SelectedIndexChanged += new System.EventHandler(this.lbServices_SelectedIndexChanged);
            // 
            // ucWmscLayerConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbWmsCUrl);
            this.Controls.Add(this.gbLayers);
            this.Name = "ucWmscLayerConfiguration";
            this.Size = new System.Drawing.Size(759, 396);
            this.gbLayers.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox tbWmsCUrl;
        private System.Windows.Forms.GroupBox gbLayers;
        private System.Windows.Forms.Button btnRetrieve;
        private System.Windows.Forms.ListBox lbServices;

        private void btnRetrieve_Click(object sender, EventArgs e)
        {
            // Complete sample urrel:
            // http://labs.metacarta.com/wms-c/tilecache.py?version=1.1.1&request=GetCapabilities&service=wms-c
            // Does not work yet: http://public-wms.kaartenbalie.nl/wms/nederland
            //string url = String.Format("{0}?version={1}&request=GetCapabilities&service=wms-c", tbWmsCUrl.Text, cbbVersion.SelectedItem);
            string url = tbWmsCUrl.Text;

            try
            {
                _tileSources = new List<ITileSource>(WmscTileSource.CreateFromWmscCapabilties(ToXDocument(new Uri(url))));

                var names = new List<string>();
                foreach (var tileSource in _tileSources)
                {
                    names.Add(tileSource.Schema.Name);
                }

                lbServices.DataSource = names;
                
                tbWmsCUrl.Items.Add(url);
                tbWmsCUrl.SelectedIndex = tbWmsCUrl.Items.Count - 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        private static XDocument ToXDocument(Uri uri)
        {
            Stream stream = GetRemoteXmlStream(uri);
            var sr = new StreamReader(stream);
            var ret = XDocument.Load(sr);
            return ret;
        }

        private static Stream GetRemoteXmlStream(Uri uri)
        {
            var myRequest = (HttpWebRequest)WebRequest.Create(uri);
            var myResponse = myRequest.GetSyncResponse(30000);
            var stream = myResponse.GetResponseStream();
            return stream;
        }

        private void lbServices_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbServices.SelectedItem != null)
            {
                string name = (String)lbServices.SelectedItem;
                foreach (ITileSource tileSource in _tileSources)
                {
                    if (tileSource.Schema.Name == name)
                    {
                        _selectedTileSource = tileSource;
                    }
                }
            }
        }
    }
}