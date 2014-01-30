using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotSpatial.Plugins.BruTileLayer;

namespace TestAppWithoutExtension
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var l = BruTileLayer.CreateOsmMapnicLayer();
            l.Reproject(map.Projection);
            map.Layers.Add(l);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var f = new DotSpatial.Plugins.BruTileLayer.Configuration.Forms.BruTileLayerDialog();
            f.Map = map;
            f.ShowDialog();
            f.Dispose();
        }
    }
}
