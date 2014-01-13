using System;
using System.Windows.Forms;

namespace DotSpatial.Plugins.BruTileLayer.Configuration.Forms
{
    public partial class ucBingLayerConfiguration : UserControl, IConfigurationEditor
    {
        public ucBingLayerConfiguration()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            foreach (var bmt in Enum.GetNames(typeof (BruTile.Web.BingMapType)))
                cboBingMapsType.Items.Add(bmt);
            cboBingMapsType.SelectedIndex = 0;
        }

        public string BruTileName { get { return "Bing Maps"; } }

        public IConfiguration Create()
        {
            var bmt = (BruTile.Web.BingMapType) Enum.Parse(typeof (BruTile.Web.BingMapType), cboBingMapsType.Text);
            var pcr = System.IO.Path.Combine(
                BruTileLayerPlugin.Settings.PermaCacheRoot,
                cboBingMapsType.Text);
            return new BingLayerConfiguration(pcr, bmt, txtBingMapsToken.Text);
        }

        public void SaveSettings()
        {
        }
    }
}
