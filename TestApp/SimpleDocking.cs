// -----------------------------------------------------------------------
// <copyright file="SimpleDocking.cs" company="DotSpatial Team">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DotSpatial.Controls.Docking;
using System.ComponentModel.Composition;
using System.Drawing;

namespace TestApp
{
    /// <summary>
    ///
    /// </summary>
    public class SimpleDocking : IDockManager
    {

        [Import("Shell")]
        private ContainerControl Shell { get; set; }
        private readonly List<Form> _forms = new List<Form>();

        #region IDockManager Members

        /// <summary>
        /// Removes the specified panel.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Remove(string key)
        {
            foreach (Form form in _forms)
            {
                if (form.Name == key)
                {
                    form.Close();
                    _forms.Remove(form);
                    break;
                }
            }
        }

        /// <summary>
        /// Show the panel with the provided key
        /// </summary>
        /// <param name="key"></param>
        public void ShowPanel(string key)
        {
            var f = this._forms.Find(t => t.Name == key);
            if (f != null)
                f.Show();
        }

        /// <summary>
        /// Occurs when the active panel is changed.
        /// </summary>
        public event EventHandler<DockablePanelEventArgs> ActivePanelChanged;

        /// <summary>
        /// Selects the panel.
        /// </summary>
        /// <param name="key">The key.</param>
        public void SelectPanel(string key)
        {
        }

        /// <summary>
        /// Adds the specified panel.
        /// </summary>
        /// <param name="panel"></param>
        public void Add(DockablePanel panel)
        {
            Add(panel.Key, panel.Caption, panel.InnerControl, panel.Dock);
        }

        /// <summary>
        /// Resets the layout of the dock panels to a developer specified location.
        /// </summary>
        public void ResetLayout()
        {
        }

        public event EventHandler<DockablePanelEventArgs> PanelClosed;

        public event EventHandler<DockablePanelEventArgs> PanelAdded;

        public event EventHandler<DockablePanelEventArgs> PanelRemoved;

        public event EventHandler<DockablePanelEventArgs> PanelHidden;

        #endregion

        #region OnPanelRemoved

        /// <summary>
        /// Triggers the PanelRemoved event.
        /// </summary>
        public virtual void OnPanelRemoved(DockablePanelEventArgs ea)
        {
            if (PanelRemoved != null)
                PanelRemoved(null/*this*/, ea);
        }

        #endregion

        #region OnPanelAdded

        /// <summary>
        /// Triggers the PanelAdded event.
        /// </summary>
        public virtual void OnPanelAdded(DockablePanelEventArgs ea)
        {
            if (PanelAdded != null)
                PanelAdded(null/*this*/, ea);
        }

        #endregion

        #region OnPanelClosed

        /// <summary>
        /// Triggers the PanelClosed event.
        /// </summary>
        public virtual void OnPanelClosed(DockablePanelEventArgs ea)
        {
            if (PanelClosed != null)
                PanelClosed(null/*this*/, ea);
        }

        #endregion

        #region OnActivePanelChanged

        /// <summary>
        /// Triggers the ActivePanelChanged event.
        /// </summary>
        public virtual void OnActivePanelChanged(DockablePanelEventArgs ea)
        {
            if (ActivePanelChanged != null)
                ActivePanelChanged(null/*this*/, ea);
        }

        /// <summary>
        /// Triggers the <see cref="PanelHidden"/> event
        /// </summary>
        /// <param name="ea"></param>
        public virtual void OnPanelHidden(DockablePanelEventArgs ea)
        {
            if (PanelHidden != null)
                PanelHidden(this, ea);
        }

        #endregion

        /// <summary>
        /// Adds the specified panel.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="caption">The caption of the panel and any tab button.</param>
        /// <param name="panel">The panel.</param>
        /// <param name="dockStyle">The dock location.</param>
        public void Add(string key, string caption, Control panel, DockStyle dockStyle)
        {
            Form owner = Shell as Form;

            if (panel == null) return;
            panel.Dock = DockStyle.Fill;

            var form = new Form();
            form.Controls.Add(panel);
            form.Name = key;
            form.Text = panel.Name;
            form.Width = panel.Width;
            form.Height = panel.Height;
            if (panel.Name.Equals("Map"))
            {
                form.Width = 700;
                form.Height = 400;
            }
            if (owner != null)
            {
                form.Owner = owner;
                form.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            }
            form.ControlBox = false;
            form.Show();
            form.Activated += form_Activated;
            _forms.Add(form);
        }

        private void form_Activated(object sender, EventArgs e)
        {
            OnActivePanelChanged(new DockablePanelEventArgs((sender as Form).Name));
        }

        public void HidePanel(string key)
        {

        }
    }
}