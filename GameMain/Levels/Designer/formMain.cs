#if DESIGNER
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TikiEngine.Levels.Designer
{
    internal partial class formMain : Form
    {
        #region Init
        public formMain()
        {
            InitializeComponent();
        }
        #endregion

        #region Member - EventHandler
        private void panelViewport_SizeChanged(object sender, EventArgs e)
        {
            Program.Game.Reset(
                panelViewport.Width,
                panelViewport.Height,
                false
            );
        }

        private void splitContainer1_Panel2_SizeChanged(object sender, EventArgs e)
        {
            Point pForm = this.PointToScreen(Point.Empty);
            Point pControl = panelViewport.PointToScreen(Point.Empty);

            if (GI.Control != null)
            {
                GI.Control.MouseOffset = new Microsoft.Xna.Framework.Point(
                    (pControl.X - pForm.X) * -1,
                    (pControl.Y - pForm.Y) * -1
                );
            }
        }

        private void formMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.Game.Exit();
        }
        #endregion

        #region Properties
        public ucDesign Designer
        {
            get { return ucDesign1; }
        }

        public bool DesignShow
        {
            get { return !splitContainer1.Panel1Collapsed; }
            set { splitContainer1.Panel1Collapsed = !value; }
        }
        #endregion
    }
}
#endif