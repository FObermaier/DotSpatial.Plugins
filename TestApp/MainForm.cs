using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Data;
using DotSpatial.Data.Forms;
using DotSpatial.Projections;
using DotSpatial.Projections.AuthorityCodes;

namespace TestApp
{
    public partial class MainForm : Form
    {
        [Export("Shell", typeof(ContainerControl))]
        private static ContainerControl _shell;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            
            LogManager.DefaultLogManager.AddLogger(new TestLogger());

            _shell = this;

            try
            {
                appManager.LoadExtensions();
            }
            finally
            {
                map.FunctionMode = FunctionMode.Pan;
                map.Projection = KnownCoordinateSystems.Projected.World.Polyconicworld;
                _infos = new ProjectionInfo[]
                {
                    KnownCoordinateSystems.Projected.Europe.EuropeLambertConformalConic,
                    KnownCoordinateSystems.Projected.World.Sinusoidalworld,
                    AuthorityCodeHandler.Instance["EPSG:3857"],
                    KnownCoordinateSystems.Projected.World.Polyconicworld,
                    ProjectionInfo.FromEpsgCode(28992)
                };
                
                map.MouseClick += map_MouseClick;
            }
        }

        void map_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                
                _index++;
                map.Projection = _infos[_index % _infos.Length];
                foreach (var layer in map.Layers)
                {
                    layer.Reproject(map.Projection);
                }

                map.ZoomToMaxExtent();
                map.Invalidate();
                
            }
        }

        private readonly ProjectionInfo[] _infos;
        private int _index = 0;

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.Space)
            {
                _index++;
                map.Projection = _infos[_index++];
                map.ZoomToMaxExtent();
            }
        }

        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            if (e.KeyCode == Keys.Space) e.IsInputKey = true;
        }

        private class TestLogger : ILogger
        {
            public void Progress(string key, int percent, string message)
            {
            }

            public void Exception(Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }

            public void PublicMethodEntered(string methodName, IEnumerable<string> parameters)
            {
            }

            public void PublicMethodLeft(string methodName)
            {
            }

            public void Status(string message)
            {
            }

            public void MessageBoxShown(string messageText, DialogResult result)
            {
                Console.WriteLine(messageText);
            }

            public void InputBoxShown(string messageText, DialogResult result, string value)
            {
            }

            public string Description { get; private set; }
            public int Key { get; set; }
        }
    }
}
