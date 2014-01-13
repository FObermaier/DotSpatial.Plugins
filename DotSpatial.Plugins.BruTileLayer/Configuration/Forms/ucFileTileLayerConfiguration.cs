using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace DotSpatial.Plugins.BruTileLayer.Configuration.Forms
{
    public partial class ucFileTileLayerConfiguration : UserControl, IConfigurationEditor
    {
        public ucFileTileLayerConfiguration()
        {
            InitializeComponent();
            BruTileName = "File Tile Provider";

            foreach (var ici in System.Drawing.Imaging.ImageCodecInfo.GetImageDecoders())
            {
                var format = ici.FormatDescription.ToLowerInvariant();
                if (format == "emf") continue;

                var i = cboImageFormat.Items.Add(new KeyValuePair<string, string>(ici.CodecName, format));
            }
            cboImageFormat.DisplayMember = "Key";
            cboImageFormat.ValueMember = "Value";
            cboImageFormat.SelectedIndex = 0;
        }


        public string BruTileName { get; private set; }
        public IConfiguration Create()
        {
            var path = txtRootFolder.Text;
            var format = (string)cboImageFormat.SelectedValue;
            if (format.StartsWith("."))
                format = format.Substring(1);

            return new FileTileLayerConfiguration(Path.GetDirectoryName(path),path, format, 100, 200);
        }

        public void SaveSettings() { }

    }
}
