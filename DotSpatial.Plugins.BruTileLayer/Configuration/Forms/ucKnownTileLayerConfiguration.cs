using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Plugins.BruTileLayer.Configuration.Forms
{
    public partial class ucKnownTileLayerConfiguration : UserControl, IConfigurationEditor
    {
        public ucKnownTileLayerConfiguration()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            foreach (var bmt in Enum.GetNames(typeof (BruTile.Web.KnownTileServers)))
            {
                if (bmt == "Custom") continue;
                cboKnownOsmMapTypes.Items.Add(bmt);
            }
            cboKnownOsmMapTypes.SelectedIndex = 0;
        }

        public string BruTileName { get { return "OpenStreetMap based Maps"; } }

        public IConfiguration Create()
        {
            var bmt = (BruTile.Web.KnownTileServers)Enum.Parse(typeof(BruTile.Web.KnownTileServers), cboKnownOsmMapTypes.Text);
            return new KnownTileLayerConfiguration(System.IO.Path.Combine(new BruTileLayerSettings().PermaCacheRoot,cboKnownOsmMapTypes.Text),
                bmt, txtOsmMapTypeToken.Text);
        }

        public void SaveSettings() {}
    }
}
