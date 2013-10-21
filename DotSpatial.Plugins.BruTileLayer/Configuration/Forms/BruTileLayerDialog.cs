using System;
using System.ComponentModel;
using System.Windows.Forms;
using DotSpatial.Controls;

namespace DotSpatial.Plugins.BruTileLayer.Configuration.Forms
{
    public partial class BruTileLayerDialog : Form
    {
        public BruTileLayerDialog()
        {
            InitializeComponent();
            LoadSettings();

            ucConfigurationContainer1.AddBruTileLayer += HandleAddBrutileLayer;
        }

        private void HandleAddBrutileLayer(object sender, AddBruTileLayerEventArgs e)
        {
            if (Map == null)
                return;

            //DotSpatial.Projections.KnownCoordinateSystems.Projected.w
            Map.Layers.Insert(0, new BruTileLayer(e.BruTileLayerConfiguration));
        }

        public IMap Map { get; set; }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                SaveSettings();
                ucConfigurationContainer1.SaveSetting();
            }
            base.OnClosing(e);
        }

        private void LoadSettings()
        {
            //throw new NotImplementedException();
        }

        private void SaveSettings()
        {
            
        }
    }
}
