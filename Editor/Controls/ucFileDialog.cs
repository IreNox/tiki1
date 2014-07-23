using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TikiEngine.Elements;

namespace TikiEngine.Editor.Controls
{
    public enum FileDialogMode
    {
        Open,
        Save,
        Remove
    }

    public partial class ucFileDialog : UserControl
    {
        #region Vars
        private Type _objectType;

        private NameObject _selectedObject;

        private Action _delegateCancel;
        private Action<NameObject> _delegateAction;
        #endregion

        #region Init
        public ucFileDialog()
        {
            InitializeComponent();
        }

        public ucFileDialog(FileDialogMode mode, Action<NameObject> onClickAction, Action onClickCancel)
            : this()
        {
            this.SetMode(mode);

            _delegateCancel = onClickCancel;
            _delegateAction = onClickAction;
        }
        #endregion

        #region Member
        public void SetObjectType(Type type)
        {
            _objectType = type;

            listFiles.DataSource = DataManager.LoadObjectTypeNonGeneric(_objectType);
            listFiles.DisplayMember = "Name";
        }

        public void SetMode(FileDialogMode mode)
        {
            switch (mode)
            {
                case FileDialogMode.Open:
                    buttonAction.Text = "Open";
                    break;
                case FileDialogMode.Save:
                    buttonAction.Text = "Save";
                    break;
                case FileDialogMode.Remove:
                    buttonAction.Text = "Remove";
                    break;
            }
        }
        #endregion

        #region Member - EventHandler
        private void buttonAction_Click(object sender, EventArgs e)
        {
            switch (buttonAction.Text)
            { 
                case "Open":
                    if (_selectedObject == null)
                    { 
                        MessageBox.Show(
                            this,
                            "No file selected.",
                            "Editor",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );

                    }
                    break;
                case "Save":
                    if (_selectedObject != null) _selectedObject.Name = textName.Text;
                    break;
            }

            if (_delegateAction != null) _delegateAction(_selectedObject);
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            DataManager.RemoveObject<NameObject>(_selectedObject);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (_delegateCancel != null) _delegateCancel();
        }

        private void listFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            textName.Text = ((NameObject)listFiles.SelectedItem).Name;

            if (buttonAction.Text == "Open")
            {
                _selectedObject = ((NameObject)listFiles.SelectedItem);
            }
        }
        #endregion

        #region Propertys
        public NameObject SelectedObject
        {
            get { return _selectedObject; }
            set
            {
                _selectedObject = value;
                listFiles.SelectedItem = value;

                textName.Text = value.Name;
            }
        }
        #endregion
    }
}
