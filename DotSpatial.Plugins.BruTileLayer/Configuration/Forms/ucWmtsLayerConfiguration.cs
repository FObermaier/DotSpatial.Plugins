using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;
using BruTile;
using BruTile.Web;
using BruTile.Wmts;

namespace DotSpatial.Plugins.BruTileLayer.Configuration.Forms
{
    public partial class ucWmtsLayerConfiguration : UserControl, IConfigurationEditor
    {
        public ucWmtsLayerConfiguration()
        {
            InitializeComponent();
        }

        public string BruTileName { get { return "Web Map Tile Service"; } }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ReadSettings();
        }

        private void ReadSettings()
        {
            cboWmts.Items.Clear();
            var settings = Path.Combine(Application.LocalUserAppDataPath, "BruTileLayer", "wmts.config");
            if (!File.Exists(settings)) return;

            using (var streamReader = new StreamReader(File.OpenRead(settings)))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    if (string.IsNullOrEmpty(line)) continue;
                    if (line.Length < 7) continue;
                    if (!line.StartsWith("http")) continue;
                    cboWmts.Items.Add(line);
                }
            }
            if (cboWmts.Items.Count > 0)
                cboWmts.SelectedIndex = 0;
        }

        private void WriteSettings()
        {
            var settings = Path.Combine(Application.LocalUserAppDataPath, "BruTileLayer", "wmts.config");
            if (File.Exists(settings)) File.Delete(settings);

            using (var streamWriter = new StreamWriter(File.OpenWrite(settings)))
            {
                streamWriter.WriteLine("# Web Map Tile Service Urls");
                var selectedItem = cboWmts.Text;
                if (!string.IsNullOrEmpty(selectedItem)) streamWriter.WriteLine(selectedItem);

                foreach (var item in cboWmts.Items)
                {
                    var url = item.ToString();
                    if (url != selectedItem)
                        streamWriter.WriteLine(url);
                }
            }
        }

        public IConfiguration Create()
        {
            if (tvwWmtsLayers.SelectedNode == null)
                return null;

            var tileSource = (Uri) tvwWmtsLayers.SelectedNode.Tag;
            var host = tileSource.Host.Replace(".", "_");
            return new WmtsLayerConfiguration(System.IO.Path.Combine(BruTileLayerPlugin.Settings.PermaCacheRoot, "Wmts", host, tvwWmtsLayers.SelectedNode.Text),
                tvwWmtsLayers.SelectedNode.Text, tileSource);

        }

        private void cboWmts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboWmts.SelectedIndex == -1)
                return;

            var uri = new Uri(cboWmts.Text);
            var tileSources = WmtsLayerConfiguration.GetTileSources(new Uri(cboWmts.Text));
            if (tileSources == null)
            {
                if (MessageBox.Show("No tile sources found for the specified URL. Shall URL be removed?", 
                    "Get tile sources failed", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    cboWmts.Items.RemoveAt(cboWmts.SelectedIndex);
                return;
            }

            FillTree(uri, tileSources);
        }

        private void FillTree(Uri uri, IEnumerable<ITileSource> tileSources)
        {
            tvwWmtsLayers.Nodes.Clear();
            /* //Trying to get more information about layers to display
            XDocument doc;
            var req = WebRequest.Create(uri);
            using (var resp = req.GetResponse())
            {
                using (var s = resp.GetResponseStream())
                {
                    doc = XDocument.Load(s);
                }
            }

            foreach (var xElement in doc.Root.Descendants(XName.Get("Layer", "http://www.opengis.net/wmts/1.0")))
            {
               // xEleme
            }
            */
            foreach (var tileSource in tileSources)
            {
                var n = tvwWmtsLayers.Nodes.Add(tileSource.Title);
                n.Tag = uri;
            }
        }

        private void cboWmts_KeyDown(object sender, KeyEventArgs e)
        {
            Uri uri;
            if (!Uri.TryCreate(cboWmts.Text, UriKind.Absolute, out uri))
                return;

            var tileSources = WmtsLayerConfiguration.GetTileSources(uri);
            if (tileSources != null)
            {
                cboWmts.Items.Add(uri.ToString());
                FillTree(uri, tileSources);
            }
        }

        private void cboWmts_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (string.IsNullOrEmpty(cboWmts.Text))
                return;

            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                e.IsInputKey = true;
            }
        }

        public void SaveSettings()
        {
            WriteSettings();
        }

    }
}
