using System;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;

namespace DotSpatial.Plugins.SharpMapLayer
{
    public class SharpMapLayerPlugin : Extension
    {
        private const string SmlRoot = "SmlRoot";

        public SharpMapLayerPlugin()
        {
            
        }
        public override void Activate()
        {
            AddMenus(App.HeaderControl);
            base.Activate();
            OnInvokeSharpMapLayerDialog(null, EventArgs.Empty);
        }

        private void AddMenus(IHeaderControl headerControl)
        {
            headerControl.Add(new SimpleActionItem("Add SharpMap Layer ...", OnInvokeSharpMapLayerDialog));
        }

        private void OnInvokeSharpMapLayerDialog(object sender, EventArgs e)
        {
            var map = new SharpMap.Map();
            var ts = new BruTile.Web.OsmTileSource();
            var tl = new SharpMap.Layers.TileLayer(ts, "OSM");
            map.Layers.Add(tl);

            var sml = new SharpMapLayer(map);
            App.Map.Layers.Add(sml);
        }

        public override void Deactivate()
        {
            base.Deactivate();
            RemoveMenus(App.HeaderControl);
        }

        private static void RemoveMenus(IHeaderControl headerControl)
        {
            headerControl.Remove(SmlRoot);
        }
    }
}