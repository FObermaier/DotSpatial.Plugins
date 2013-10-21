// -----------------------------------------------------------------------
// <copyright file="SimpleHeaderControl.cs" company="DotSpatial Team">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel.Composition;
using System.Windows.Forms;
using DotSpatial.Controls.Header;

namespace TestApp
{
    /// <summary>
    /// Creates a ToolStripContainer that hosts a MenuBarHeaderControl.
    /// </summary>
    [Export(typeof(IHeaderControl))]
    public class SimpleHeaderControl : MenuBarHeaderControl, IPartImportsSatisfiedNotification
    {
        private ToolStripContainer _toolStripContainer1;

        [Import("Shell", typeof(ContainerControl))]
        private ContainerControl Shell { get; set; }

        #region IPartImportsSatisfiedNotification Members

        /// <summary>
        /// Called when a part's imports have been satisfied and it is safe to use. (Shell will have a value)
        /// </summary>
        public void OnImportsSatisfied()
        {
            _toolStripContainer1 = new ToolStripContainer();
            _toolStripContainer1.ContentPanel.SuspendLayout();
            _toolStripContainer1.SuspendLayout();

            _toolStripContainer1.Dock = DockStyle.Fill;
            _toolStripContainer1.Name = "toolStripContainer1";

            // place all of the controls that were on the form originally inside of our content panel.
            while (Shell.Controls.Count > 0)
            {
                foreach (Control control in Shell.Controls)
                {
                    this._toolStripContainer1.ContentPanel.Controls.Add(control);
                }
            }

            Shell.Controls.Add(_toolStripContainer1);

            _toolStripContainer1.ContentPanel.ResumeLayout(false);
            _toolStripContainer1.ResumeLayout(false);
            _toolStripContainer1.PerformLayout();

            Initialize(_toolStripContainer1);
        }

        #endregion
    }
}