using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using BruTile.Web;
using BruTile;
using BruTile.Wmsc;

namespace DotSpatial.Plugins.BruTileLayer.Configuration.Forms
{
    public partial class ucWmscLayerConfiguration : UserControl, IConfigurationEditor
    {
        private IList<ITileSource> _tileSources;
        private ITileSource _selectedTileSource;

        public ucWmscLayerConfiguration()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            ReadSettings();
            base.OnLoad(e);
        }

        private void InitForm()
        {
            // Read the files in de services directory
            ReadSettings();
        }

        private void ReadSettings()
        {
            tbWmsCUrl.Items.Clear();
            var settings = Path.Combine(Application.LocalUserAppDataPath, "BruTileLayer", "wms-c.config");
            if (!File.Exists(settings)) return;

            using (var streamReader = new StreamReader(File.OpenRead(settings)))
            {
                while (streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    if (string.IsNullOrEmpty(line)) continue;
                    if (line.Length < 7) continue;
                    if (!line.StartsWith("http")) continue;
                    tbWmsCUrl.Items.Add(line);
                }
            }
            if (tbWmsCUrl.Items.Count > 0)
                tbWmsCUrl.SelectedIndex = 0;
        }

        private void WriteSettings()
        {
            var settings = Path.Combine(Application.LocalUserAppDataPath, "BruTileLayer", "wms-c.config");
            if (File.Exists(settings)) File.Delete(settings);

            using (var streamWriter = new StreamWriter(File.OpenRead(settings)))
            {
                streamWriter.WriteLine("# WMS-C Urls");
                var selectedItem = tbWmsCUrl.Text;
                if (!string.IsNullOrEmpty(selectedItem)) streamWriter.WriteLine(selectedItem);
                
                foreach (var item in tbWmsCUrl.Items)
                {
                    var url = item.ToString();
                    if (url != selectedItem)
                        streamWriter.WriteLine(url);
                }
            }
        }

        public string BruTileName { get { return "WMS-C Service Layer"; } }

        public IConfiguration Create()
        {
            var wmsc = (WmscTileSource) _selectedTileSource;
            var wmscp = (WebTileProvider) wmsc.Provider;
            var wmscr = ReflectionHelper.ReflectRequest<WmscRequest>(wmscp);
            var uri = ReflectionHelper.ReflectBaseUri(wmscr);
            var host = uri.Host.Replace(".", "_");
            
            return new WmscLayerConfiguration(
                Path.Combine(BruTileLayerPlugin.Settings.PermaCacheRoot, host),  "Wmsc", wmsc);
        }

        public void SaveSettings()
        {
            WriteSettings();
        }

    }

}