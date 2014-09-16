using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;
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
            
            LogManager.DefaultLogManager.AddLogger(new TestLogger(consoleControl1));

            _shell = this;
            appManager.Map = map;
            
            appManager.DockManager = new SpatialDockManager();
            var hc = new MenuBarHeaderControl();
            hc.Initialize(new ToolStripPanel(), msTest);
            appManager.HeaderControl = hc;
            var sss = new SpatialStatusStrip();
            
            appManager.ProgressHandler = sss;

            try
            {
                appManager.LoadExtensions();
            }
            finally
            {
                map.FunctionMode = FunctionMode.Pan;
                _infos = new ProjectionInfo[]
                {
                    KnownCoordinateSystems.Projected.World.WebMercator,
                    KnownCoordinateSystems.Projected.Europe.EuropeLambertConformalConic,
                    KnownCoordinateSystems.Projected.World.Sinusoidalworld,
                    KnownCoordinateSystems.Projected.World.Polyconicworld,
                    ProjectionInfo.FromEpsgCode(28992)
                };
                map.Projection = _infos[0];
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
        private int _index;

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
            private readonly ConsoleControl.ConsoleControl _ctrl;

            public TestLogger(ConsoleControl.ConsoleControl consoleControl)
            {
                _ctrl = consoleControl;
            }

            public void Progress(string key, int percent, string message)
            {
                _ctrl.WriteOutput(".", DefaultForeColor);
            }

            public void Exception(Exception ex)
            {
                _ctrl.WriteOutput("\n"+ex.Message, Color.Red);
                _ctrl.WriteOutput(ex.StackTrace, Color.Red);
                throw ex;
            }

            public void PublicMethodEntered(string methodName, IEnumerable<string> parameters)
            {
                _ctrl.WriteOutput(string .Format("Method '{0}' entered\n"), DefaultForeColor);
            }

            public void PublicMethodLeft(string methodName)
            {
                _ctrl.WriteOutput(string.Format("Method '{0}' left\n"), DefaultForeColor);
            }

            public void Status(string message)
            {
                _ctrl.WriteOutput(message, Color.Orange);
            }

            public void MessageBoxShown(string messageText, DialogResult result)
            {
                _ctrl.WriteOutput(messageText + "\n", Color.Gold);
            }

            public void InputBoxShown(string messageText, DialogResult result, string value)
            {
            }

            public string Description { get { return "TestAppLogger"; } }
            public int Key
            {
                get; set; }
        }
    }
}
