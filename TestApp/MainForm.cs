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
            
            if (DesignMode) return;
            
            LogManager.DefaultLogManager.AddLogger(new TestLogger(consoleControl1));

            _shell = this;
            appManager.Map = map;

            appManager.DockManager = new SpatialDockManager();
            var hc = new MenuBarHeaderControl();
            hc.Initialize(new ToolStripPanel(), msTest);
            appManager.HeaderControl = hc;
            var sss = new SpatialStatusStrip();
                        
            appManager.ProgressHandler = sss;
            appManager.ShowExtensionsDialogMode = ShowExtensionsDialogMode.Default;

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
                if (map.Layers.Count == 0)
                    return;

                _index++;
                Reproject(_infos[_index%_infos.Length]);
            }
        }

static Extent Reproject(Extent extent, ProjectionInfo source, ProjectionInfo target, int depth = 0)
{
    var xy = ToSequence(extent);
    DotSpatial.Projections.Reproject.ReprojectPoints(xy, null, source, target, 0, xy.Length / 2);
    var res = ToExtent(xy);

    return res;
}

static double[] ToSequence(Extent extent)
{
    const int horizontal = 72;
    const int vertical = 36;
    var res = new double[horizontal * vertical * 2];

    var dx = extent.Width / (horizontal - 1);
    var dy = extent.Height / (vertical - 1);

    var minY = extent.MinY;
    var k = 0;
    for (var i = 0; i < vertical; i++)
    {
        var minX = extent.MinX;
        for (var j = 0; j < horizontal; j++)
        {
            res[k++] = minX;
            res[k++] = minY;
            minX += dx;
        }
        minY += dy;
    }

    return res;
}

private static Extent ToExtent(double[] xyOrdinates)
{
    double minX = double.MaxValue, maxX = double.MinValue;
    double minY = double.MaxValue, maxY = double.MinValue;

    var i = 0;
    while (i < xyOrdinates.Length)
    {
        if (!double.IsNaN(xyOrdinates[i]) &&
            (double.MinValue < xyOrdinates[i] && xyOrdinates[i] < double.MaxValue))
        {
            if (minX > xyOrdinates[i]) minX = xyOrdinates[i];
            if (maxX < xyOrdinates[i]) maxX = xyOrdinates[i];
        }
        i += 1;
        if (!double.IsNaN(xyOrdinates[i]) &&
            (double.MinValue < xyOrdinates[i] && xyOrdinates[i] < double.MaxValue))
        {
            if (minY > xyOrdinates[i]) minY = xyOrdinates[i];
            if (maxY < xyOrdinates[i]) maxY = xyOrdinates[i];
        }
        i += 1;
    }
    return new Extent(minX, minY, maxX, maxY);
}



        private readonly ProjectionInfo[] _infos;
        private int _index;

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.Space)
            {
                _index++;
                Reproject(_infos[_index%_infos.Length]);
            }
        }

        private void Reproject(DotSpatial.Projections.ProjectionInfo proj)
        {
            LogManager.DefaultLogManager.LogMessage(string.Format("Reprojecting from '{0}' to '{1};'", map.Projection.Name, proj.Name), DialogResult.OK);
            
            var extents = map.ViewExtents;
            var oldProjection = map.Projection;
            map.Projection = proj;
            var newExtents = Reproject(extents, oldProjection, map.Projection);

            foreach (var layer in map.Layers)
            {
                layer.Reproject(map.Projection);
            }

            map.ViewExtents = newExtents;
            map.Invalidate();
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

        private void map_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawString(map.Projection.Name, SystemFonts.CaptionFont, SystemBrushes.WindowText, new PointF());
        }

        private void consoleControl1_OnConsoleOutput(object sender, ConsoleControl.ConsoleEventArgs args)
        {
            ((ConsoleControl.ConsoleControl)sender).InternalRichTextBox.ScrollToCaret();
        }

        private void mnuFileLoadMap_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog
            {
                Filter = appManager.SerializationManager.OpenDialogFilterText,
                FilterIndex = 0,
                FileName = appManager.SerializationManager.CurrentProjectFile

                //Title = appManager.SerializationManager.

            })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    appManager.SerializationManager.OpenProject(ofd.FileName);
                }
            }
        }

        private void mnuFileSaveMap_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog()
            {
                Filter = appManager.SerializationManager.SaveDialogFilterText,
                FilterIndex = 0,
                FileName = appManager.SerializationManager.CurrentProjectFile

                //Title = appManager.SerializationManager.

            })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    appManager.SerializationManager.SaveProject(sfd.FileName);
                }
            }
        }
    }
}
