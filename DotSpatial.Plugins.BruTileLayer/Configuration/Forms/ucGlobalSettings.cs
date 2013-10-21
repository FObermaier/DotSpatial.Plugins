using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DotSpatial.Plugins.BruTileLayer.Configuration.Forms
{
    public partial class ucGlobalSettings : UserControl
    {
        public ucGlobalSettings()
        {
            InitializeComponent();
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            var settings = BruTileLayerPlugin.Settings;
            
            FillPermaCacheSettings(settings);


            nudMinimum.Value = settings.MemoryCacheMinimum;
            nudMaximum.Value = settings.MemoryCacheMaximum;

            chkAsyncMode.Checked = settings.UseAsyncMode;
        }

        private void FillPermaCacheSettings(BruTileLayerSettings settings)
        {
            cboPermaCacheType.Items.Add(PermaCacheType.FileCache);
            cboPermaCacheType.Items.Add(PermaCacheType.DbCache);
            cboPermaCacheType.Items.Add(PermaCacheType.Noop);
            cboPermaCacheType.SelectedIndex = (int)settings.PermaCacheType;

            txtPermaCacheRoot.Text = settings.PermaCacheRoot;

            var si = 0;
            foreach (var ici in System.Drawing.Imaging.ImageCodecInfo.GetImageDecoders())
            {
                var format = ici.FormatDescription.ToLowerInvariant();
                if (format == "emf") continue;
                
                var i = cboImageFormat.Items.Add(new KeyValuePair<string, string>(ici.CodecName, format));
                if (format == settings.PermaCacheFormat)
                    si = i;
            }
            cboImageFormat.DisplayMember = "Key";
            cboImageFormat.ValueMember = "Value";

            cboImageFormat.SelectedIndex = si;
            
            nudExpire.Value = settings.PermaCacheExpireInDays;

        }

    }
}
