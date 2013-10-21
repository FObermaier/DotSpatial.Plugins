using System.ComponentModel;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace DotSpatial.Plugins.BruTileLayer.Configuration.Forms
{
    public partial class BruTileLayerDisplayProperties : Form
    {
        private BruTileLayer _bruTileLayer;
        private int _transparency;

        public BruTileLayerDisplayProperties()
        {
            InitializeComponent();
        }

        public BruTileLayer BruTileLayer
        {
            get { return _bruTileLayer; }
            set
            {
                _bruTileLayer = value;
                _transparency = value.Transparency;
                tbTransparency.Value = _transparency;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (BruTileLayer != null)
            if (DialogResult == DialogResult.OK)
            {
                BruTileLayer.Transparency = tbTransparency.Value;
            }
            else
            {
                BruTileLayer.Transparency = _transparency;
            }

            base.OnClosing(e);
        }
    }
}
