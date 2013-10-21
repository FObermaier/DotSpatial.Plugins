using System;
using System.Reflection;
using System.Windows.Forms;

namespace DotSpatial.Plugins.BruTileLayer.Configuration.Forms
{
    public partial class ucInfo : UserControl
    {
        #region Construction and Disposal
        public ucInfo()
        {
            InitializeComponent();
        }
        #endregion

        #region UserControl overrides
        protected override void OnLoad(EventArgs e)
        {           
            base.OnLoad(e);

            var asm = Assembly.GetExecutingAssembly();
            var name = asm.GetName();


            var ap = new AssemblyPropertyHelper(asm);
            lblName.Text = ap.Product;
            txtDescriptionValue.Text = ap.Description;
            lblCopyrightValue.Text = ap.Copyright;
            lblCompanyValue.Text = ap.Company;

            lblVersionValue.Text = name.Version.ToString(4);
            lblVersionValue.Text = name.Version.ToString(4);
            lblVersionValue.Text = name.Version.ToString(4);

            foreach (var asmTest in asm.GetReferencedAssemblies())
            {
                if (asmTest.Name == "BruTile")
                {
                    lblBruTileVersionValue.Text = asmTest.Version.ToString(4);
                }
            }

        }
        #endregion

        #region Nested classes

        private class AssemblyPropertyHelper
        {
            #region Private fields

            private readonly AssemblyProductAttribute _product;
            private readonly AssemblyCompanyAttribute _company;
            private readonly AssemblyCopyrightAttribute _copyright;
            private readonly AssemblyDescriptionAttribute _description;

            #endregion

            #region Construction and Disposal

            public AssemblyPropertyHelper(Assembly asm)
            {
                var attributes = asm.GetCustomAttributes(typeof (AssemblyProductAttribute), false);
                if (attributes.Length > 0)
                    _product = (AssemblyProductAttribute) attributes[0];
                attributes = asm.GetCustomAttributes(typeof (AssemblyCompanyAttribute), false);
                if (attributes.Length > 0)
                    _company = (AssemblyCompanyAttribute) attributes[0];
                attributes = asm.GetCustomAttributes(typeof (AssemblyCopyrightAttribute), false);
                if (attributes.Length > 0)
                    _copyright = (AssemblyCopyrightAttribute) attributes[0];
                attributes = asm.GetCustomAttributes(typeof (AssemblyDescriptionAttribute), false);
                if (attributes.Length > 0)
                    _description = (AssemblyDescriptionAttribute) attributes[0];
            }

            #endregion

            #region Properties

            public string Product
            {
                get { return _product == null ? string.Empty : _product.Product; }
            }

            public string Company
            {
                get { return _company == null ? string.Empty : _company.Company; }
            }

            public string Copyright
            {
                get { return _copyright == null ? string.Empty : _copyright.Copyright; }
            }

            public string Description
            {
                get { return _description == null ? "-" : _description.Description; }
            }

            #endregion
        }

        #endregion
    }
}
