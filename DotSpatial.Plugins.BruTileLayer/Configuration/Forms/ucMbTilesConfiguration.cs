using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace DotSpatial.Plugins.BruTileLayer.Configuration.Forms
{
    public partial class ucMbTilesConfiguration : UserControl, IConfigurationEditor
    {
        public ucMbTilesConfiguration()
        {
            InitializeComponent();
            BruTileName = "MbTiles Provider";
        }


        public string BruTileName { get; private set; }
        public IConfiguration Create()
        {
            var path = txtMbTilesFile.Text;
            return new MbTilesConfiguration(path);
        }
    }
}
