using System.Windows.Forms;
using DotSpatial.Controls;
using cctrl = ConsoleControl.ConsoleControl;
namespace TestApp
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.appManager = new DotSpatial.Controls.AppManager();
            this.legend = new DotSpatial.Controls.Legend();
            this.ttHelp = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitHorizontal = new System.Windows.Forms.SplitContainer();
            this.map = new DotSpatial.Controls.Map();
            this.consoleControl1 = new ConsoleControl.ConsoleControl();
            this.msTest = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileLoadMap = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileSaveMap = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitHorizontal)).BeginInit();
            this.splitHorizontal.Panel1.SuspendLayout();
            this.splitHorizontal.Panel2.SuspendLayout();
            this.splitHorizontal.SuspendLayout();
            this.msTest.SuspendLayout();
            this.SuspendLayout();
            // 
            // appManager
            // 
            this.appManager.Directories = ((System.Collections.Generic.List<string>)(resources.GetObject("appManager.Directories")));
            this.appManager.DockManager = null;
            this.appManager.HeaderControl = null;
            this.appManager.Legend = this.legend;
            this.appManager.Map = null;
            this.appManager.ProgressHandler = null;
            this.appManager.ShowExtensionsDialogMode = DotSpatial.Controls.ShowExtensionsDialogMode.Default;
            // 
            // legend
            // 
            this.legend.BackColor = System.Drawing.Color.White;
            this.legend.ControlRectangle = new System.Drawing.Rectangle(0, 0, 203, 332);
            this.legend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.legend.DocumentRectangle = new System.Drawing.Rectangle(0, 0, 187, 428);
            this.legend.HorizontalScrollEnabled = true;
            this.legend.Indentation = 30;
            this.legend.IsInitialized = false;
            this.legend.Location = new System.Drawing.Point(0, 0);
            this.legend.MinimumSize = new System.Drawing.Size(5, 5);
            this.legend.Name = "legend";
            this.legend.ProgressHandler = null;
            this.legend.ResetOnResize = false;
            this.legend.SelectionFontColor = System.Drawing.Color.Black;
            this.legend.SelectionHighlight = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(238)))), ((int)(((byte)(252)))));
            this.legend.Size = new System.Drawing.Size(203, 332);
            this.legend.TabIndex = 0;
            this.legend.VerticalScrollEnabled = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.legend);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitHorizontal);
            this.splitContainer1.Size = new System.Drawing.Size(611, 332);
            this.splitContainer1.SplitterDistance = 203;
            this.splitContainer1.TabIndex = 1;
            // 
            // splitHorizontal
            // 
            this.splitHorizontal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitHorizontal.Location = new System.Drawing.Point(0, 0);
            this.splitHorizontal.Name = "splitHorizontal";
            this.splitHorizontal.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitHorizontal.Panel1
            // 
            this.splitHorizontal.Panel1.Controls.Add(this.map);
            // 
            // splitHorizontal.Panel2
            // 
            this.splitHorizontal.Panel2.Controls.Add(this.consoleControl1);
            this.splitHorizontal.Size = new System.Drawing.Size(404, 332);
            this.splitHorizontal.SplitterDistance = 236;
            this.splitHorizontal.TabIndex = 2;
            // 
            // map
            // 
            this.map.AllowDrop = true;
            this.map.BackColor = System.Drawing.Color.White;
            this.map.CollectAfterDraw = false;
            this.map.CollisionDetection = true;
            this.map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map.ExtendBuffer = false;
            this.map.FunctionMode = DotSpatial.Controls.FunctionMode.None;
            this.map.IsBusy = false;
            this.map.IsZoomedToMaxExtent = false;
            this.map.Legend = this.legend;
            this.map.Location = new System.Drawing.Point(0, 0);
            this.map.Name = "map";
            this.map.ProgressHandler = null;
            this.map.ProjectionModeDefine = DotSpatial.Controls.ActionMode.PromptOnce;
            this.map.ProjectionModeReproject = DotSpatial.Controls.ActionMode.PromptOnce;
            this.map.RedrawLayersWhileResizing = false;
            this.map.SelectionEnabled = true;
            this.map.Size = new System.Drawing.Size(404, 236);
            this.map.TabIndex = 2;
            this.map.Paint += new System.Windows.Forms.PaintEventHandler(this.map_Paint);
            // 
            // consoleControl1
            // 
            this.consoleControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.consoleControl1.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.consoleControl1.IsInputEnabled = true;
            this.consoleControl1.Location = new System.Drawing.Point(0, 0);
            this.consoleControl1.Name = "consoleControl1";
            this.consoleControl1.SendKeyboardCommandsToProcess = false;
            this.consoleControl1.ShowDiagnostics = false;
            this.consoleControl1.Size = new System.Drawing.Size(404, 92);
            this.consoleControl1.TabIndex = 0;
            this.consoleControl1.OnConsoleOutput += new ConsoleControl.ConsoleEventHanlder(this.consoleControl1_OnConsoleOutput);
            // 
            // msTest
            // 
            this.msTest.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile});
            this.msTest.Location = new System.Drawing.Point(0, 0);
            this.msTest.Name = "msTest";
            this.msTest.Size = new System.Drawing.Size(611, 24);
            this.msTest.TabIndex = 2;
            this.msTest.Text = "menuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFileLoadMap,
            this.mnuFileSaveMap});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(37, 20);
            this.mnuFile.Text = "File";
            // 
            // mnuFileLoadMap
            // 
            this.mnuFileLoadMap.Name = "mnuFileLoadMap";
            this.mnuFileLoadMap.Size = new System.Drawing.Size(152, 22);
            this.mnuFileLoadMap.Text = "Load map";
            this.mnuFileLoadMap.Click += new System.EventHandler(this.mnuFileLoadMap_Click);
            // 
            // mnuFileSaveMap
            // 
            this.mnuFileSaveMap.Name = "mnuFileSaveMap";
            this.mnuFileSaveMap.Size = new System.Drawing.Size(152, 22);
            this.mnuFileSaveMap.Text = "Save map";
            this.mnuFileSaveMap.Click += new System.EventHandler(this.mnuFileSaveMap_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 356);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.msTest);
            this.MainMenuStrip = this.msTest;
            this.Name = "MainForm";
            this.Text = "DotSpatial.Plugins.TestApp";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitHorizontal.Panel1.ResumeLayout(false);
            this.splitHorizontal.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitHorizontal)).EndInit();
            this.splitHorizontal.ResumeLayout(false);
            this.msTest.ResumeLayout(false);
            this.msTest.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AppManager appManager;
        private System.Windows.Forms.ToolTip ttHelp;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private DotSpatial.Controls.Legend legend;
        private System.Windows.Forms.SplitContainer splitHorizontal;
        private Map map;
        private cctrl consoleControl1;
        private MenuStrip msTest;
        private ToolStripMenuItem mnuFile;
        private ToolStripMenuItem mnuFileLoadMap;
        private ToolStripMenuItem mnuFileSaveMap;
    }
}

