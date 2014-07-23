using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TikiEngine.Editor.Controls
{
    public partial class ucMainProperties : UserControl
    {
        #region Init
        public ucMainProperties()
        {
            InitializeComponent();
        }
        #endregion

        #region Member - EventHandler
        private void propertys_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            propertys.Refresh();
        }
        #endregion

        #region Properties
        public object SelectedObject
        {
            get { return propertys.SelectedObject; }
            set { propertys.SelectedObject = value; }
        }
        #endregion
    }
}
