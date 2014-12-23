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

            foreach (var bmt in Enum.GetNames(typeof (BruTile.Predefined.KnownTileSource)))
            {
                if (bmt == "Custom") continue;
                cboKnownOsmMapTypes.Items.Add(bmt);
            }
            cboKnownOsmMapTypes.SelectedIndex = 0;
        }

        public string BruTileName { get { return "Known tile layers"; } }

        public IConfiguration Create()
        {
            var bmt = (BruTile.Predefined.KnownTileSource)Enum.Parse(typeof(BruTile.Predefined.KnownTileSource), cboKnownOsmMapTypes.Text);
            return new KnownTileLayerConfiguration(null, bmt, txtOsmMapTypeToken.Text);
        }

        public void SaveSettings() {}
    }
}
