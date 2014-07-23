#if DESIGNER
using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace TikiEngine.Levels.Designer
{
    public partial class formElements : Form
    {
        #region Vars
        private bool _attached = false;
        #endregion

        #region Init
        public formElements(bool attached)
        {
            _attached = attached;

            InitializeComponent();
        }

        private void formElements_Load(object sender, EventArgs e)
        {
            if (listElements.Items.Count != 0) return;

            Type gei = (_attached ? typeof(GameElementAttached) : typeof(GameElementIsland));
            Assembly asm = Assembly.GetExecutingAssembly();

            var q = asm.GetTypes().Where(t => t.IsSubclassOf(gei));

            foreach (Type t in q)
            {
                listElements.Items.Add(t);
            }
            listElements.DisplayMember = "Name";
        }
        #endregion

        #region Member - EventHandler
        private void listElements_DoubleClick(object sender, EventArgs e)
        {
            buttonOk_Click(null, null);
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = (this.CurrentType != null ? DialogResult.OK : DialogResult.Cancel );
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        #endregion

        #region Properties
        public Type CurrentType
        {
            get { return listElements.SelectedItem as Type; }
        }
        #endregion
    }
}
#endif