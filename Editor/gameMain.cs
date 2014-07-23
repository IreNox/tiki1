using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using TikiEngine.Editor.Modes;
using FarseerPhysics.Dynamics;

namespace TikiEngine.Editor
{
    internal class gameMain : Game
    {
        #region Vars
        private formMain _formMain;
        private System.Windows.Forms.Control _window;

        private GraphicsDeviceManager _deviceManager;

        private EditorMode _currentMode;
        private Dictionary<EditorMode, World> _modes = new Dictionary<EditorMode, World>();

        private SpriteBatch _spriteBatch;
        private SpriteBatch _parallaxBatch;
        private PolygonBatch _polygonBatch;

        public event EventHandler ModeChanged;
        #endregion

        #region Init
        public gameMain()
        {
            _formMain = new formMain();
            _formMain.Show();
            Program.FormMain = _formMain;

            _deviceManager = new GraphicsDeviceManager(this);
            _deviceManager.PreferredBackBufferWidth = _formMain.panelViewport.Width;
            _deviceManager.PreferredBackBufferHeight = _formMain.panelViewport.Height;
            _deviceManager.PreparingDeviceSettings += (sender, e) =>
            {
                e.GraphicsDeviceInformation.PresentationParameters.IsFullScreen = false;
                e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = _formMain.panelViewport.Handle;
                e.GraphicsDeviceInformation.GraphicsProfile = GraphicsProfile.HiDef;
            };
            _deviceManager.DeviceCreated += (sender, e) =>
            {
                if (Program.GameMain == null) return;

                GI.Init(this);

                _spriteBatch = GI.SpriteBatch;
                _parallaxBatch = GI.SpriteBatchParallax;
                _polygonBatch = GI.PolygonBatch;

                Program.FormMain.Init();
            };
            _deviceManager.ApplyChanges();

            _window = System.Windows.Forms.Control.FromHandle(this.Window.Handle);
            _window.VisibleChanged += (sender, e) =>
            {
                if (_window.Visible) _window.Visible = false;
            };

            Mouse.WindowHandle = _formMain.panelViewport.Handle;

            this.IsMouseVisible = true;
            this.Content.RootDirectory = "Content";
        }
        #endregion

        #region Member
        public void Reset()
        {
            _deviceManager.PreferredBackBufferWidth = _formMain.panelViewport.Width;
            _deviceManager.PreferredBackBufferHeight = _formMain.panelViewport.Height;
            _deviceManager.ApplyChanges();

            GI.Camera.ResetScreen();
        }
        #endregion

        #region Member - Modes
        public void AddMode<T>()
            where T : EditorMode, new()
        {
            T mode = new T();
            GI.World = new World(new Vector2(0, 9.81f));

            _modes.Add(mode, GI.World);

            mode.Init();
        }

        public T GetMode<T>()
            where T : EditorMode
        {
            return _modes.Keys.OfType<T>().FirstOrDefault();
        }

        public void SetMode<T>()
            where T : EditorMode
        {
            _currentMode = this.GetMode<T>();

            if (_currentMode != null)
            {
                GI.World = _modes[_currentMode];

                _currentMode.Activate();
            }

            if (this.ModeChanged != null) this.ModeChanged(this, EventArgs.Empty);
        }
        #endregion

        #region Member - Xna - Draw
        protected override void Draw(GameTime gameTime)
        {
            if (_currentMode == null) return;

            this.GraphicsDevice.Clear(_currentMode.BackgroundColor);

            if (_currentMode.UseCamera)
            {
                _spriteBatch.Begin(0, null, null, null, null, null, GI.Camera.ViewMatrix2D);
            }
            else
            {
                _spriteBatch.Begin();
            }
            _parallaxBatch.Begin();
            _polygonBatch.Begin();

            if (_currentMode != null) _currentMode.Draw(gameTime);

            _polygonBatch.End();
            _parallaxBatch.End();

            this.GraphicsDevice.SetRenderTarget(null);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
        #endregion

        #region Member - Xna - Update
        protected override void Update(GameTime gameTime)
        {
            if (_currentMode == null) return;

            if (GI.Control.MouseDown(TikiEngine.MouseButtons.Right))
            {
                GI.Camera.CurrentPosition += -GI.Control.MouseDistanceDisplay();
            }

            //float zoom = (((float)GI.Control.MouseCurrentState.ScrollWheelValue / 1000) + 1) * 0.4f;
            //if (GI.Camera.Zoom != zoom && zoom > 0) GI.Camera.Zoom = zoom;

            _currentMode.Update(gameTime);
            GI.Control.Update(gameTime);
            GI.Camera.Update(gameTime);

            base.Draw(gameTime);
        }
        #endregion

        #region Propertys
        public EditorMode CurrentMode
        {
            get { return _currentMode; }
            set { _currentMode = value; }
        }

        public Dictionary<EditorMode, World> EditorModes
        {
            get { return _modes; }
        }
        #endregion
    }
}
