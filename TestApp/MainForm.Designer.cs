using DotSpatial.Controls;

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
            this.appManager = new DotSpatial.Controls.AppManager();
            this.map = new DotSpatial.Controls.Map();
            this.ttHelp = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // appManager
            // 
            this.appManager.CompositionContainer = null;
            this.appManager.DockManager = null;
            this.appManager.HeaderControl = null;
            this.appManager.Map = this.map;
            this.appManager.ProgressHandler = null;
            this.appManager.ShowExtensionsDialog = DotSpatial.Controls.ShowExtensionsDialog.None;
            // 
            // map1
            // 
            this.map.AllowDrop = true;
            this.map.BackColor = System.Drawing.Color.White;
            this.map.CollectAfterDraw = false;
            this.map.CollisionDetection = true;
            this.map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map.ExtendBuffer = false;
            this.map.FunctionMode = DotSpatial.Controls.FunctionMode.None;
            this.map.IsBusy = false;
            this.map.Location = new System.Drawing.Point(0, 0);
            this.map.Name = "map";
            this.map.ProgressHandler = null;
            this.map.ProjectionModeDefine = DotSpatial.Controls.ActionMode.PromptOnce;
            this.map.ProjectionModeReproject = DotSpatial.Controls.ActionMode.PromptOnce;
            this.map.RedrawLayersWhileResizing = false;
            this.map.SelectionEnabled = true;
            this.map.Size = new System.Drawing.Size(596, 506);
            this.map.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 356);
            this.Controls.Add(this.map);
            this.Name = "MainForm";
            this.Text = "DotSpatial.Plugins.TestApp";
            this.ResumeLayout(false);

        }

        #endregion

        private AppManager appManager;
        private Map map;
        private System.Windows.Forms.ToolTip ttHelp;
    }
}

