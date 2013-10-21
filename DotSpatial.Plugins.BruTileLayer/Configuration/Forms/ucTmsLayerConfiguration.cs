using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;
using BruTile.Web.TmsService;

namespace DotSpatial.Plugins.BruTileLayer.Configuration.Forms
{
    public partial class ucTmsLayerConfiguration : UserControl, IConfigurationEditor
    {
        private string _servicesDirectory;
        private bool _initialized=true;
        //private string file;

        public ucTmsLayerConfiguration()
        {
            InitializeComponent();
        }

        private TileMapItem SelectedService { get; set; }
        private TileMapService SelectedTileMapService { get; set; }
        private XDocument TmsTileMapServiceXml { get; set; }
        private bool Inverted { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            InitForm();
            base.OnLoad(e);
        }

        private void InitForm()
        {
            // Read the files in de services directory
            _servicesDirectory = Path.Combine(Application.LocalUserAppDataPath, "BruTileLayer");
            var files=new List<String>();
            var di = new DirectoryInfo(_servicesDirectory);
            if (!di.Exists) di.Create();
            foreach (var xmlFile in di.GetFiles("*.xml"))
            {
                files.Add(Path.GetFileNameWithoutExtension(xmlFile.FullName));
            }

            cboProvider.DataSource = files;
            cboProvider.Enabled = files.Count > 0;
            if (cboProvider.Enabled)
                cboProvider.SelectedIndex = 0;
        }

        private void WriteProviderXml(string providerName, string url)
        {
            var providerFile = Path.Combine(_servicesDirectory, providerName + ".xml");
            if (!File.Exists(providerFile))
            {
                var xml = @"<?xml version='1.0' encoding='utf-8' ?><Services>";
                xml += String.Format(@"<TileMapService title='{0}' version='1.0.0' href='{1}'/>",
                                     providerName, url);
                xml += "</Services>";

                TextWriter tw = new StreamWriter(providerFile);
                tw.WriteLine(xml);
                tw.Close();
            }
            else
            {
                MessageBox.Show("Provider " + Name + " does already exist.");
            }
        }

        public string BruTileName
        {
            get { return "Tiled Map Service Layer (TMS)"; }
        }

        public IConfiguration Create()
        {
            var tms = SelectedService;
            return new TmsLayerConfiguration(
                Path.Combine(new BruTileLayerSettings().PermaCacheRoot, cboProvider.Text, tms.Title),
                tms.Title, tms.Href, Inverted, false);
        }
    }
}
