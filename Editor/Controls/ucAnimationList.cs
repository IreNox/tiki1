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
using TikiEngine.Elements.Graphics;

namespace TikiEngine.Editor.Controls
{
    public partial class ucAnimationList : UserControl
    {
        #region Vars
        private modeAnimation _modeAnimation;
        #endregion

        #region Init
        public ucAnimationList()
        {
            InitializeComponent();

            _modeAnimation = Program.GameMain.GetMode<modeAnimation>();

            listAnimations.DisplayMember = "Name";
        }
        #endregion

        #region Member - EventHandler
        private void buttonNew_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                this,
                "New?",
                "New",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                _modeAnimation.CurrentAnimation = new Animation();
                Program.FormMain.SelectTabPage<ucAnimationPropertys>();
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (_modeAnimation.CurrentAnimation != null)
            {
                var result = MessageBox.Show(
                    this,
                    "Remove?",
                    "Editor",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes)
                {
                    DataManager.RemoveObject(
                        _modeAnimation.CurrentAnimation
                    );
                }
            }
            else
            {
                MessageBox.Show(this, "No Element selected.", "Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void listAnimations_SelectedIndexChanged(object sender, EventArgs e)
        {
            _modeAnimation.CurrentAnimation = (Animation)listAnimations.SelectedItem;
        }
        #endregion

        #region Propertys
        public object DataSource
        {
            get { return listAnimations.DataSource; }
            set { listAnimations.DataSource = value; }
        }
        #endregion
    }
}
