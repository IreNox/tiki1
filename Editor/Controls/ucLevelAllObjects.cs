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
    internal partial class ucLevelAllObjects : UserControl
    {
        #region Vars
        protected modeMap modeMap;

        protected NameObject[] allObjects;
        protected NameObject currentObject;
        #endregion

        #region Init
        public ucLevelAllObjects()
        {
            InitializeComponent();

            modeMap = Program.GameMain.GetMode<modeMap>();
            modeMap.RefreshGUI += new EventHandler(ModeMap_RefreshGUI);

            listObjects.DisplayMember = "Name";
        }
        #endregion

        #region Member - Protected
        protected virtual void LoadObjects()
        {
            allObjects = DataManager.LoadObjectType<NameObject>();

            this.RefreshObjects();
        }

        protected virtual void RefreshObjects()
        {
            listObjects.DataSource = allObjects.Where(
                o => o.Name.IndexOf(textSearch.Text, StringComparison.OrdinalIgnoreCase) != -1
            ).ToList();
        }

        protected virtual void ItemClick()
        { 
        }

        protected virtual void ItemDoubleClick()
        {
            NameObject obj = DataManager.LoadObject<NameObject>(currentObject.Name, true);
            obj.StartPosition = ConvertUnits.ToSimUnits(
                GI.Camera.CurrentPositionCenter
            );

            modeMap.CurrentLevel.Elements.Add(obj);
            modeMap.GetTabControl<ucLevelElements>().LoadObjects();
        }

        protected virtual void ItemRemove()
        {
            DataManager.RemoveObject<NameObject>(currentObject);
        }
        #endregion

        #region Member - EventHandler
        private void textSearch_TextChanged(object sender, EventArgs e)
        {
            this.RefreshObjects();
        }

        private void ModeMap_RefreshGUI(object sender, EventArgs e)
        {
            this.LoadObjects();
        }

        private void listObjects_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (currentObject == null) return;

            this.ItemDoubleClick();
        }

        private void listObjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentObject = (NameObject)listObjects.SelectedItem;

            this.ItemClick();
        }

        private void listObjects_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == Keys.Delete)
            {
                DialogResult result = MessageBox.Show(
                    this,
                    "Remove?",
                    "Editor",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation
                );

                if (result == DialogResult.Yes)
                {
                    this.ItemRemove();
                }
            }
        }
        #endregion
    }
}
