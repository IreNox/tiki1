using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using TikiEngine.Editor.Modes;
using TikiEngine.Editor.Controls;
using TikiEngine.Elements;
using Microsoft.Xna.Framework;

namespace TikiEngine.Editor
{
    partial class formMain : Form
    {
        #region Vars
        private gameMain _game;
        #endregion

        #region Init
        public formMain()
        {
            InitializeComponent();
        }

        internal void Init()
        {
            _game = Program.GameMain;

            _game.ModeChanged += Device_ModeChanged;

            _game.AddMode<modeMap>();
            _game.AddMode<modeObject>();
            _game.AddMode<modeAnimation>();
            _game.AddMode<modeBreakable>();
            _game.AddMode<modeBreakableTest>();

            _game.SetMode<modeMap>();
        }
        #endregion

        #region Member
        public void SelectTabPage<TControl>()
            where TControl : Control
        {
            tabControl.SelectedTab = _game.CurrentMode.GetTabPageByControl<TControl>();
        }

        public void SelectMainControl(Control control)
        {
            var pc = splitContainer1.Panel1.Controls;

            pc.Clear();
            pc.Add(control);
            control.Dock = DockStyle.Fill;
        }
        #endregion

        #region Member - File
        public void New()
        {
            DialogResult result = MessageBox.Show(
                this,
                "New. Save?",
                "Editor",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Warning
            );

            switch (result)
            {
                case DialogResult.Yes:
                    _game.CurrentMode.Save();
                    break;
                case DialogResult.Cancel:
                    return;
            }

            _game.CurrentMode.New();
        }

        public void Open()
        {
            ucFileDialog fd = new ucFileDialog(
                FileDialogMode.Open,
                delegate(NameObject obj)
                {
                    _game.CurrentMode.Open(obj.Name);

                    this.SelectMainControl(panelViewport);
                },
                delegate()
                {
                    this.SelectMainControl(panelViewport);
                }
            );
            fd.SetObjectType(
                _game.CurrentMode.ObjectType
            );

            this.SelectMainControl(fd);
        }

        public void Save()
        {
            if (String.IsNullOrEmpty(_game.CurrentMode.ObjectName))
            {
                this.SaveAs();
            }
            else
            {
                _game.CurrentMode.Save();
            }
        }

        public void SaveAs()
        {
            ucFileDialog fd = new ucFileDialog(
                FileDialogMode.Save,
                delegate(NameObject obj)
                {
                    _game.CurrentMode.SaveAs(obj.Name);

                    this.SelectMainControl(panelViewport);
                },
                delegate()
                {
                    this.SelectMainControl(panelViewport);
                }
            );
            fd.SetObjectType(
                _game.CurrentMode.ObjectType
            );
            fd.SelectedObject = _game.CurrentMode.CurrentObject;

            this.SelectMainControl(fd);
        }
        #endregion

        #region Member - EventHandler
        private void panelViewport_SizeChanged(object sender, EventArgs e)
        {
            Program.GameMain.Reset();
        }

        private void formMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.GameMain.Exit();
        }

        private void Device_ModeChanged(object sender, EventArgs e)
        {
            tabControl.TabPages.Clear();
            tabControl.TabPages.AddRange(
                _game.CurrentMode.TabPages
            );

            menuPreferences.Enabled = _game.CurrentMode.UsePreferences;
        }
        #endregion

        #region Member - EventHandler - Click
        private void menuLevelEditor_Click(object sender, EventArgs e)
        {
            _game.SetMode<modeMap>();
        }

        private void menuAnimationEditor_Click(object sender, EventArgs e)
        {
            _game.SetMode<modeAnimation>();
        }

        private void menuObjectEditor_Click(object sender, EventArgs e)
        {
            _game.SetMode<modeObject>();
        }

        private void menuBreakableEditor_Click(object sender, EventArgs e)
        {
            _game.SetMode<modeBreakable>();
        }
        #endregion

        #region Member - EventHandler - File
        private void menuLevelNew_Click(object sender, EventArgs e)
        {
            this.New();
        }

        private void menuLevelOpen_Click(object sender, EventArgs e)
        {
            this.Open();
        }

        private void menuLevelSave_Click(object sender, EventArgs e)
        {
            this.Save();
        }

        private void menuSaveAs_Click(object sender, EventArgs e)
        {
            this.SaveAs();
        }

        private void menuLevelPropertys_Click(object sender, EventArgs e)
        {
            _game.CurrentMode.ShowPreferences();
        }

        private void menuCameraReset_Click(object sender, EventArgs e)
        {
            GI.Camera.CurrentPosition = Vector2.Zero;
        }
        #endregion
    }
}
