using System;
using System.ComponentModel;
using System.Net;
using System.Security;
using System.Text;
using System.Windows.Forms;

namespace DotSpatial.Plugins.BruTileLayer.Configuration.Forms
{
    public partial class WmsConnection : Form
    {
        public WmsConnection()
        {
            InitializeComponent();

        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);

            if (DialogResult == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(txtName.Text))
                {
                    wmsConnectionErrorProvider.SetError(txtName, "No name assigned");
                    e.Cancel = true;
                }

                if (string.IsNullOrEmpty(txtUrl.Text))
                {
                    wmsConnectionErrorProvider.SetError(txtUrl, "No Url assigned");
                    e.Cancel = true;
                }
            }
        }

        public WmsConnectionInfo WmsConnectionInfo
        {
            get
            {
                return new WmsConnectionInfo
                {
                    Name = txtName.Text,
                    Url = txtUrl.Text,
                    Username = txtUsername.Text,
                    Password = txtPassword.Text,
                    Referrer = txtReferrer.Text,
                };
            }
            set
            {
                if (value == null)
                    return;
                txtName.Text = value.Name;
                txtUrl.Text = value.Url;
                txtUsername.Text = value.Username;
                txtPassword.Text = value.Password;
                txtReferrer.Text = value.Referrer;
            }
        }
    }

    public class WmsConnectionInfo  
    {
        public string Name { get; set; }
        public string Url { get; set; }
        
        public NetworkCredential Credential { get{ return new NetworkCredential(Username, Password);} } 

        public string Username { get; set; }
        public string Password { get; set; }
        public string Referrer { get; set; }

        public override string ToString()
        {
            return Name;
            SecureString ss;
        }

        public static WmsConnectionInfo Parse(string wmsConnectionInfoLine)
        {
            var parts = wmsConnectionInfoLine.Split('|');
            if (parts.Length == 1)
            {
                var uri = new Uri(parts[0]);
                return new WmsConnectionInfo
                {Name = uri.Host, Url = parts[0]};
            }


            return new WmsConnectionInfo
            {
                Url = parts[1],
                Name = parts[0],
                Username = parts[2],
                Password = parts[3],
                Referrer = parts[4]
            };
        }

        public string ToConnectionInfoLine()
        {
            var sb = new StringBuilder();
            sb.Append(Name);
            sb.AppendFormat("|{0}", Url);
            sb.AppendFormat("|{0}", Username);
            sb.AppendFormat("|{0}", Password);
            sb.AppendFormat("|{0}", Referrer);
            return sb.ToString();

        }
    }
}
