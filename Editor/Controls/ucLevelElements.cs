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
    internal partial class ucLevelElements : ucLevelAllObjects
    {
        #region Init
        public ucLevelElements()
        {
            InitializeComponent();
        }
        #endregion

        #region Member - Protected
        protected override void LoadObjects()
        {
            allObjects = modeMap.CurrentLevel.Elements.ToArray();

            this.RefreshObjects();
        }

        protected override void ItemClick()
        {
            modeMap.SelectedObject = currentObject;
        }

        protected override void ItemDoubleClick()
        {
            GI.Camera.CurrentPosition = currentObject.CurrentPosition;
        }

        protected override void ItemRemove()
        {
            modeMap.CurrentLevel.Elements.Remove(currentObject);
        }
        #endregion
    }
}
