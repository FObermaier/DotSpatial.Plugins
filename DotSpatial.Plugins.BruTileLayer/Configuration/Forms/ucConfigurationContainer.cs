using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace DotSpatial.Plugins.BruTileLayer.Configuration.Forms
{
    public partial class ucConfigurationContainer : UserControl
    {
        private static int _selectedIndex = 0;

        private static readonly List<Type> ConfigurationEditors = new List<Type>();
        private IConfigurationEditor _currentEditor;

        public EventHandler<AddBruTileLayerEventArgs> AddBruTileLayer { get; set; }

        public ucConfigurationContainer()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (ConfigurationEditors.Count == 0)
                AddByReflection();
            AddFromCache();

            cboConfigurations.DisplayMember = "Key";
            cboConfigurations.ValueMember = "Value";
            if (cboConfigurations.Items.Count > 0)
                cboConfigurations.SelectedIndex = _selectedIndex;
            else
                btnAdd.Enabled = false;
        }

        public void SaveSetting()
        {
            _selectedIndex = cboConfigurations.SelectedIndex;
        }

        public void AddFromCache()
        {
            foreach (var configurationEditor in ConfigurationEditors)
            {
                var config = (UserControl)Activator.CreateInstance(configurationEditor);
                gbConfigurationItem.Controls.Add(config);
                config.Dock = DockStyle.Fill;
                var configEditor = (IConfigurationEditor)config;
                cboConfigurations.Items.Add(new KeyValuePair<string, UserControl>(configEditor.BruTileName, config));
            }
        }

        public void AddByReflection()
        {
            var typeUserControl = typeof (UserControl);
            var typeIConfigurationEditor = typeof (IConfigurationEditor);
            
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (typeIConfigurationEditor.IsAssignableFrom(type) &&
                    typeUserControl.IsAssignableFrom(type))
                {
                    ConfigurationEditors.Add(type);
                }
            }
        }
    }

    public class AddBruTileLayerEventArgs : EventArgs
    {
        public IConfiguration BruTileLayerConfiguration { get; private set; }

        internal AddBruTileLayerEventArgs(IConfiguration config)
        {
            BruTileLayerConfiguration = config;
        }
    }
}
