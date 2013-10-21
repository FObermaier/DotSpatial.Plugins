using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml;
using BruTile.Web.TmsService;

namespace DotSpatial.Plugins.BruTileLayer.Configuration.Forms
{
    public partial class AddProviderForm : Form
    {
        public string ProviderName { get; set; }

        public string ProvidedServiceURL { get; set; }

        public AddProviderForm()
        {
            ProvidedServiceURL = string.Empty;
            InitializeComponent();

            InitForm();
        }

        private void InitForm()
        {
            var asr = new AppSettingsReader();
            string sampleProviders = null;
            try
            {
                sampleProviders = (string)asr.GetValue("sampleProviders", typeof (string));
            }
            catch (Exception)
            {
                sampleProviders = "https://dl.dropboxusercontent.com/u/9984329/ArcBruTile/arcbrutile_sample_providers.txt";
            }
            
            var providers = GetTileMapServices(sampleProviders);
            if (providers != null)
            {
                lbProviders.DataSource = providers;
                lbProviders.DisplayMember = "Name";
                //lbProviders.ValueMember = "Url";
            }
        }

        private static List<TileMapServiceDefinition> GetTileMapServices(string url)
        {
            try
            {

                var result = new List<TileMapServiceDefinition>();
                var request = (HttpWebRequest) WebRequest.Create(url);
                request.UserAgent =
                    "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.1.14) Gecko/20080404 Firefox/2.0.0.14";

                using (var response = (HttpWebResponse) request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            using (var streamReader = new StreamReader(stream))
                            {
                                while (!streamReader.EndOfStream)
                                {
                                    var line = streamReader.ReadLine();
                                    if (string.IsNullOrEmpty(line)) continue;
                                    var items = line.Split(',');
                                    result.Add(new TileMapServiceDefinition
                                    {
                                        Name = items[0],
                                        Url = items[1],
                                        Version = items[2]
                                    });
                                }
                            }
                        }
                    }
                }
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal class TileMapServiceDefinition
        {
            public string Name;
            public string Url;
            public string Version;

            public override string ToString()
            {
                return Name;
            }
        }

        private static TileMapService GetTileMapService(string url)
        {
            var request = (HttpWebRequest) WebRequest.Create(url);
            request.UserAgent =
                "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.1.14) Gecko/20080404 Firefox/2.0.0.14";

            TileMapService result;
            using (var response = (HttpWebResponse) request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    result = TileMapService.CreateFromResource(stream);
                }
            }
            return result;

        }
        private bool CheckUrl(string url)
        {
            if (UrlIsValid(url))
            {
                try
                {
                    GetTileMapService(url);
                    return true;
                }
                catch (WebException)
                {
                    errorProvider1.SetError(tbTmsUrl, "Could not download document. Please specify valid url");
                }
                catch (XmlException)
                {
                    errorProvider1.SetError(tbTmsUrl, "Could not download XML document. Please specify valid url");
                }
            }
            return false;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private static bool UrlIsValid(string url)
        {
            Uri result;
            return (Uri.TryCreate(url, UriKind.Absolute, out result));
        }

        private void lbProviders_SelectedIndexChanged(object sender, EventArgs e)
        {
            var d = (TileMapServiceDefinition) lbProviders.SelectedValue;
            tbName.Text = d.Name;
            tbTmsUrl.Text = d.Url;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var psi = new ProcessStartInfo {UseShellExecute = true};
            psi.FileName = "http://arcbrutile.codeplex.com";
            Process.Start(psi);
        }
    }
}
