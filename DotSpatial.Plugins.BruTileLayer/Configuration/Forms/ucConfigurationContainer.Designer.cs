using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DotSpatial.Plugins.BruTileLayer.Configuration.Forms
{
    partial class ucConfigurationContainer
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

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.cboConfigurations = new System.Windows.Forms.ComboBox();
            this.gbConfigurationItem = new System.Windows.Forms.GroupBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cboConfigurations
            // 
            this.cboConfigurations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboConfigurations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboConfigurations.FormattingEnabled = true;
            this.cboConfigurations.Location = new System.Drawing.Point(3, 3);
            this.cboConfigurations.Name = "cboConfigurations";
            this.cboConfigurations.Size = new System.Drawing.Size(291, 21);
            this.cboConfigurations.TabIndex = 0;
            this.cboConfigurations.SelectedIndexChanged += new System.EventHandler(this.cboConfigurations_SelectedIndexChanged);
            // 
            // gbConfigurationItem
            // 
            this.gbConfigurationItem.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbConfigurationItem.Location = new System.Drawing.Point(3, 30);
            this.gbConfigurationItem.Name = "gbConfigurationItem";
            this.gbConfigurationItem.Size = new System.Drawing.Size(291, 200);
            this.gbConfigurationItem.TabIndex = 1;
            this.gbConfigurationItem.TabStop = false;
            this.gbConfigurationItem.Text = "Properties";
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(219, 236);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // ucConfigurationContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.gbConfigurationItem);
            this.Controls.Add(this.cboConfigurations);
            this.Name = "ucConfigurationContainer";
            this.Size = new System.Drawing.Size(297, 262);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cboConfigurations;
        private System.Windows.Forms.GroupBox gbConfigurationItem;
        private System.Windows.Forms.Button btnAdd;

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (AddBruTileLayer == null)
                return;
            if (_currentEditor == null)
                return;

            AddBruTileLayer(this, new AddBruTileLayerEventArgs(_currentEditor.Create()));

        }

        private void cboConfigurations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboConfigurations.SelectedIndex < 0)
                return;
            
            //if (_currentEditor != null)
            //    ((UserControl)_currentEditor).Hide();

            var kvp = (KeyValuePair<string, UserControl>)cboConfigurations.Items[cboConfigurations.SelectedIndex];
            kvp.Value.BringToFront();
            _currentEditor = (IConfigurationEditor)kvp.Value;
        }
    }
}
