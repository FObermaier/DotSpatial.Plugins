using System;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;

namespace DotSpatial.Plugins.Database
{
    public class DatabasePlugin : Extension
    {
        private const string DbpRoot = "DbpRoot";

        public DatabasePlugin()
        {
            
        }

        public override void Activate()
        {
            AddMenus(App.HeaderControl);
            base.Activate();
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        private void AddMenus(IHeaderControl headerControl)
        {
            if (headerControl == null)
                return;

            var sai = new SimpleActionItem("PostGis", OnInvokeAddLayerDialog);
            sai.GroupCaption = "Add Layer";
            sai.Key = DbpRoot + "PostGis"; 
            headerControl.Add(sai);

            sai = new SimpleActionItem("SpatiaLite", OnInvokeAddLayerDialog);
            sai.GroupCaption = "Add Layer";
            sai.Key = DbpRoot + "SpatiaLite";
            headerControl.Add(sai);
        }

        private void OnInvokeAddLayerDialog(object sender, EventArgs e)
        {
            var sai = sender as SimpleActionItem;
            if (sai == null) return;
            
            switch (sai.Key)
            {
                case DbpRoot + "PostGis":
                    var pgv = new PostGisVectorProvider();
                    var ds = pgv.Open("Server=ivv-sqlt3;Database=\"ivv-projekte\";integrated security=true;",
                        "MLV3369", "vwzellen3", "zellnr", "area");
                    App.Map.Layers.Add(ds);
                    //App.Map.ZoomToMaxExtent();

                    break;
            }
            App.Map.ZoomToMaxExtent();
        }
    }
}