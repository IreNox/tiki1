using System;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TikiEngine.Elements;

namespace TikiEngine.Editor.Modes
{
    abstract class EditorMode
    {
        #region Vars
        private formMain _formMain;

        private Dictionary<Control, TabPage> _tabPages;

        public event EventHandler RefreshGUI;
        public event EventHandler ObjectChanged;
        #endregion

        #region Init
        public EditorMode()
        {
            _formMain = Program.FormMain;
            _tabPages = new Dictionary<Control, TabPage>();

            this.BackgroundColor = Color.Black;
        }
        #endregion

        #region Member - Tabs
        public void AddTabPage<TControl>(string text)
            where TControl : Control, new()
        { 
            Control control = new TControl();
            control.Dock = DockStyle.Fill;

            TabPage page = new TabPage(text);
            page.Controls.Add(control);
            page.UseVisualStyleBackColor = true;

            _tabPages.Add(control, page);
        }

        public TControl GetTabControl<TControl>()
            where TControl : Control
        {
            return _tabPages.Keys.OfType<TControl>().FirstOrDefault();
        }

        public TabPage GetTabPageByControl<TControl>()
            where TControl : Control
        {
            return _tabPages[_tabPages.Keys.OfType<TControl>().FirstOrDefault()];
        }
        #endregion

        #region Member - Abstract
        public abstract void Init();

        public abstract void Activate();

        public abstract void New();
        public abstract void Open(string name);
        public abstract void Save();
        public abstract void SaveAs(string name);

        public abstract void ShowPreferences();

        public abstract void Draw(GameTime gameTime);
        public abstract void Update(GameTime gameTime);
        #endregion

        #region Member - Protected
        protected void RaiseRefreshGUI()
        {
            this.RaiseRefreshGUI(EventArgs.Empty);
        }

        protected void RaiseRefreshGUI(EventArgs e)
        {
            if (this.RefreshGUI != null) this.RefreshGUI(this, e);
        }

        protected void RaiseObjectChanged()
        {
            this.RaiseObjectChanged(EventArgs.Empty);
        }

        protected void RaiseObjectChanged(EventArgs e)
        {
            if (this.ObjectChanged != null) this.ObjectChanged(this, e);
        }
        #endregion

        #region Propertys
        [Browsable(false)]
        public bool UseCamera { get; set; }
        [Browsable(false)]
        public bool UsePreferences { get; set; }
        public Color BackgroundColor { get; set; }

        [Browsable(false)]
        public abstract Type ObjectType { get; }
        [Browsable(false)]
        public abstract string ObjectName { get; set; }
        [Browsable(false)]
        public abstract NameObject CurrentObject { get; }

        [Browsable(false)]
        public TabPage[] TabPages
        {
            get { return _tabPages.Values.ToArray(); }
        }
        #endregion
    }
}
