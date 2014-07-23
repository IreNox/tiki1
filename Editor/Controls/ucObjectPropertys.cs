using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TikiEngine.Elements;
using TikiEngine.Editor.Modes;

namespace TikiEngine.Editor.Controls
{
    public partial class ucObjectPropertys : UserControl
    {
        #region Vars
        private bool _loading = false;

        private modeObject _modeObject;
        #endregion

        #region Init
        public ucObjectPropertys()
        {
            InitializeComponent();

            _modeObject = Program.GameMain.GetMode<modeObject>();
            _modeObject.ObjectChanged += delegate(object sender, EventArgs e)
            {
                _objectToControls();
            };

            Type typeNO = typeof(NameObject);

            comboType.DataSource = typeNO.Assembly.GetExportedTypes().Where(t => !t.IsAbstract && t.GetConstructor(Type.EmptyTypes) != null && t.IsSubclassOf(typeNO)).ToArray();
            comboType.DisplayMember = "Name";
        }
        #endregion

        #region Private Member
        private void _objectToControls()
        {
            _loading = true;

            comboType.SelectedItem = _modeObject.CurrentObjectType;
            propertyGrid.SelectedObject = _modeObject.CurrentObject;

            _loading = false;
        }
        #endregion

        #region Member - EventHandler
        private void comboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loading) return;

            Type type = (Type)comboType.SelectedItem;

            _modeObject.CreateBaseObject(type);

            _objectToControls();
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            propertyGrid.Refresh();
        }
        #endregion
    }
}
