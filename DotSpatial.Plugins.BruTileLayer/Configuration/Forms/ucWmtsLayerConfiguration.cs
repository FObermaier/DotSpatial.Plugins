using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml.Linq;
using BruTile;
using BruTile.Wmts;

namespace DotSpatial.Plugins.BruTileLayer.Configuration.Forms
{
    public partial class ucWmtsLayerConfiguration : UserControl, IConfigurationEditor
    {
        public ucWmtsLayerConfiguration()
        {
            InitializeComponent();
        }

        public string BruTileName { get { return "Web Map Tile Service (WMTS)"; } }

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
                    if (line.StartsWith("#")) continue;
                    cboWmts.Items.Add(WmsConnectionInfo.Parse(line));
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
                var selectedItem = cboWmts.SelectedItem;
                if (selectedItem != null) streamWriter.WriteLine(((WmsConnectionInfo)selectedItem).ToConnectionInfoLine());

                foreach (var item in cboWmts.Items)
                {
                    if (item != selectedItem)
                        streamWriter.WriteLine(((WmsConnectionInfo)item).ToConnectionInfoLine());
                }
            }
        }

        public IConfiguration Create()
        {
            if (lvwWmtsLayers.SelectedItems.Count == 0)
                return null;

            var tmp = lvwWmtsLayers.SelectedItems[0].Tag;
            var tileSource = (ITileSource) tmp;
            var tileSchema = (WmtsTileSchema) tileSource.Schema;
            var host = cboWmts.Text;
            var format = tileSchema.Format.Split('/')[1];
            var layerStyle = tileSchema.Identifier;
            if (!string.IsNullOrEmpty(tileSchema.Style)) layerStyle += "_" + tileSchema.Style;

            
            foreach (var c in Path.GetInvalidFileNameChars())
                host = host.Replace(c, '$');
            foreach (var c in Path.GetInvalidFileNameChars())
                layerStyle = layerStyle.Replace(c, '$');

            var path = Path.Combine(BruTileLayerPlugin.Settings.PermaCacheRoot, "Wmts", host, layerStyle, format);
            return new WmtsLayerConfiguration(path, tileSource.Name, tileSource);

        }

        private void cboWmts_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cboWmts.SelectedIndex == -1)
            //    return;

            //var uri = new Uri(cboWmts.Text);
            //var tileSources = WmtsLayerConfiguration.GetTileSources(new Uri(cboWmts.Text));
            //if (tileSources == null)
            //{
            //    if (MessageBox.Show("No tile sources found for the specified URL. Shall URL be removed?", 
            //        "Get tile sources failed", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //        cboWmts.Items.RemoveAt(cboWmts.SelectedIndex);
            //    return;
            //}

            //FillTree(uri, tileSources);
        }

        private void FillTree(WmsConnectionInfo connectionInfo)
        {
            
            lvwWmtsLayers.Items.Clear();
            
            //Trying to get more information about layers to display

            XDocument doc;
            var req = (HttpWebRequest)WebRequest.Create(connectionInfo.Url);
            if (!string.IsNullOrEmpty(connectionInfo.Username) )
                req.Credentials = new NetworkCredential(connectionInfo.Username, connectionInfo.Password);

            if (!string.IsNullOrEmpty(connectionInfo.Referrer))
                req.Referer = connectionInfo.Referrer;

            var tileSources = new List<ITileSource>();
            using (var resp = req.GetResponse())
            {
                using (var s = resp.GetResponseStream())
                {
                    tileSources.AddRange(WmtsParser.Parse(s));
                }
            }

            foreach (var tileSource in tileSources)
            {
                var tmp = (WmtsTileSchema)tileSource.Schema;

                var n = lvwWmtsLayers.Items.Add(tmp.Identifier);
                
                n.SubItems.Add(tmp.Format);
                n.SubItems.Add(tmp.Style);
                n.SubItems.Add(tileSource.Name);
                n.SubItems.Add(tmp.Abstract);
                n.SubItems.Add(tmp.Name);
                n.SubItems.Add(tmp.Srs);

                n.Tag = tileSource;
            }
        }

        private void cboWmts_KeyDown(object sender, KeyEventArgs e)
        {
            //Uri uri;
            //if (!Uri.TryCreate(cboWmts.Text, UriKind.Absolute, out uri))
            //    return;

            //var tileSources = WmtsLayerConfiguration.GetTileSources(uri);
            //if (tileSources != null)
            //{
            //    cboWmts.Items.Add(uri.ToString());
            //    FillTree(uri, tileSources);
            //}
        }

        public void SaveSettings()
        {
            WriteSettings();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (cboWmts.SelectedIndex < 0)
            {
                MessageBox.Show("You need to define/select a Wm(t)s connection first", "Can't conntect",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var wmsConnectionInfo = (WmsConnectionInfo) cboWmts.SelectedItem;
            FillTree(wmsConnectionInfo);

        }
    }
}
