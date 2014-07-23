#if DESIGNER
using System;
using System.Drawing;
using System.Windows.Forms;
using TikiEngine.Elements;
using System.IO;
using System.Text;

namespace TikiEngine.Levels.Designer
{
    internal partial class ucDesign : UserControl
    {
        #region Vars
        private Image _image;

        private LevelDesigner _level;

        private formElements _formElementsIslands = new formElements(false);
        private formElements _formElementsAttached = new formElements(true);
        #endregion

        #region Init
        public ucDesign()
        {
            InitializeComponent();
        }
        #endregion

        #region Member
        public void LoadObject(NameObject obj)
        {
            tabsProperties.Enabled = true;
            tabsProperties.SelectedTab = tabProperties;

            if (obj is GameElementIsland)
            {
                GameElementIsland island = (GameElementIsland)obj;

                if (!tabsProperties.TabPages.Contains(tabAttached))
                {
                    tabsProperties.TabPages.Add(tabAttached);
                }
                listAttached.DataSource = island.Attached;
                listAttached.DisplayMember = "ToString";
            }
            else
            {
                tabsProperties.TabPages.Remove(tabAttached);
            }
            
            labelType.Text = obj.GetType().Name;
            labelName.Text = obj.Name;
            checkReady.Checked = obj.Ready;
            propertiesControl.CurrentObject = obj;
        }

        public void SetImage(Image value)
        { 
            _image = value;

            if (this.InvokeRequired)
            {
                this.Invoke(new Action<Image>(this.SetImage), value);
                return;
            }

            picElement.BackgroundImage = value;
        }
        #endregion

        #region Member - EventHandler - Island
        private void buttonNew_Click(object sender, EventArgs e)
        {
            if (_formElementsIslands.ShowDialog(this) == DialogResult.OK)
            { 
                GameElementIsland island = (GameElementIsland)Activator.CreateInstance(
                    _formElementsIslands.CurrentType
                );

                _level.Islands.Add(island);
                _level.CurrentElement = island;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)        
        {
            propertiesControl.Save();
            _level.DrawElement();

            DataManager.SetObject(
                new LevelSave(_level)
            );

            StringBuilder sb = new StringBuilder();

            foreach (GameElement island in _level.Islands)
            {
                sb.Append(
                    island.GenerateCode()
                );
            }

            File.WriteAllText(
                "Data\\" + this.Name + ".txt",
                sb.ToString()
            );
        }
        #endregion

        #region Member - EventHander - Attached
        private void buttonAttachedAdd_Click(object sender, EventArgs e)
        {
            GameElementIsland island = _level.CurrentElement as GameElementIsland;
            
            if (island != null && _formElementsAttached.ShowDialog(this) == DialogResult.OK)
            {
                GameElementAttached attached = (GameElementAttached)Activator.CreateInstance(
                    _formElementsAttached.CurrentType,
                    island
                );

                island.Attached.Add(attached);
                _level.CurrentElement = attached;
            }
        }

        private void buttonAttachedSelect_Click(object sender, EventArgs e)
        {
            if (listAttached.SelectedItem != null)
            {
                _level.CurrentElement = (GameElementAttached)listAttached.SelectedItem;
            }
        }

        private void buttonAttachedRemove_Click(object sender, EventArgs e)
        {
            GameElementIsland island = _level.CurrentElement as GameElementIsland;

            if (island != null && listAttached.SelectedItem != null)
            {
                island.Attached.Remove(
                    (GameElementAttached)listAttached.SelectedItem
                );
            }
        }
        #endregion

        #region Properties
        public LevelDesigner Level
        {
            get { return _level; }
            set { _level = value; }
        }

        public Image Image
        {
            get { return _image; }
        }
        #endregion
    }
}
#endif