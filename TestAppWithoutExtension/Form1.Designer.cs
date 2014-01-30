namespace TestAppWithoutExtension
{
    partial class Form1
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
            this.map = new DotSpatial.Controls.Map();
            this.legend = new DotSpatial.Controls.Legend();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
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
            this.map.Size = new System.Drawing.Size(421, 311);
            this.map.TabIndex = 1;
            // 
            // legend
            // 
            this.legend.BackColor = System.Drawing.Color.White;
            this.legend.ControlRectangle = new System.Drawing.Rectangle(0, 0, 210, 311);
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
            this.legend.Size = new System.Drawing.Size(210, 311);
            this.legend.TabIndex = 0;
            this.legend.VerticalScrollEnabled = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.legend);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.button1);
            this.splitContainer1.Panel2.Controls.Add(this.map);
            this.splitContainer1.Size = new System.Drawing.Size(635, 311);
            this.splitContainer1.SplitterDistance = 210;
            this.splitContainer1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(9, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 311);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DotSpatial.Controls.Map map;
        private DotSpatial.Controls.Legend legend;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button button1;
    }
}

