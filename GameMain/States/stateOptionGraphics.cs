using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TikiEngine.Elements.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TikiEngine.States
{
    internal class stateOptionGraphics : GameState
    {
        #region Vars
        private RadioCollection _radioRes = new RadioCollection();
        private RadioCollection _radioColor = new RadioCollection();

        private CheckBox _checkWindowed;
        private Dictionary<RadioButton, DisplayMode> _res = new Dictionary<RadioButton, DisplayMode>();
        private Dictionary<RadioButton, SurfaceFormat> _color = new Dictionary<RadioButton, SurfaceFormat>();

        private DisplayModeCollection _allModes = GI.Device.Adapter.SupportedDisplayModes;

        private Dictionary<SurfaceFormat, string> _colors = new Dictionary<SurfaceFormat, string>() { 
            { SurfaceFormat.Color, "32 Bit" },
            { SurfaceFormat.Bgr565, "16 Bit" }
        };
        #endregion

        #region Init
        public stateOptionGraphics()
        {
            this.BackgroundStretch = false;
            this.LoadBackground("menu/menu_background");

            this.SetComponent(
                new Label() { 
                    FontFile = "fontIF",
                    Text = "Resolution:",
                    StartPosition = new Vector2(1, 0.5f)
                }
            );

            this.SetComponent(
                new Label()
                {
                    FontFile = "fontIF",
                    Text = "Color depth:",
                    StartPosition = new Vector2(1, 3f)
                }
            );

            _checkWindowed = new CheckBox()
            {
                Text = "Windowed",
                FontFile = "fontIF",
                CurrentPosition = new Vector2(5, 3.5f),
                Checked = !GI.Device.PresentationParameters.IsFullScreen
            };
            this.SetComponent(_checkWindowed);

            foreach (var g in _allModes.GroupBy(m => m.Height.ToString() + m.Width.ToString()))
            {
                DisplayMode dm = g.First();

                RadioButton rb = new RadioButton(_radioRes);
                rb.Text = String.Format(
                    "{0} x {1}",
                    dm.Width,
                    dm.Height
                );
                rb.FontFile = "fontIF";
                rb.Checked = (GI.Device.PresentationParameters.IsFullScreen ? dm.Width == GI.Device.DisplayMode.Width && dm.Height == GI.Device.DisplayMode.Height : dm.Width == GI.Device.Viewport.Width && dm.Height == GI.Device.Viewport.Height);
                _res[rb] = dm;
            }

            foreach (var g in _allModes.GroupBy(m => m.Format).Where(m => m.All(b => _colors.ContainsKey(b.Format))))
            {
                SurfaceFormat sf = g.First().Format;

                RadioButton rb = new RadioButton(_radioColor);
                rb.Text = _colors[sf];
                rb.FontFile = "fontIF";
                rb.Checked = (sf == GI.Device.DisplayMode.Format);
                _color[rb] = sf;
            }

            Button button = new Button();
            button.Text = "Back";
            button.FontFile = "fontIF";
            button.CurrentPosition = new Vector2(3, 5.5f);
            button.MouseClick += new EventHandler(buttonBack_MouseClick);
            this.SetComponent("button1", button);

            button = new Button();
            button.Text = "Apply";
            button.FontFile = "fontIF";
            button.CurrentPosition = new Vector2(3, 4.5f);
            button.MouseClick += new EventHandler(buttonApply_MouseClick);
            this.SetComponent("button2", button);

            this.ResetScreen();
        }
        #endregion

        #region Private Member
        private void _setPos(Vector2 posStart, RadioCollection collection)
        {
            Vector2 pos = posStart;

            foreach (RadioButton rb in collection)
            {
                rb.StartPosition = pos;

                pos.X += 2;
                if (pos.X + 3 > GI.Camera.ScreenSize.X / ConvertUnits.DisplayUnitsToSimUnitsRatio)
                {
                    pos.X = posStart.X;
                    pos.Y += 0.5f;
                }
            }
        }
        #endregion

        #region Member
        public override void ResetScreen()
        {
            _setPos(
                new Vector2(1, 1f),
                _radioRes
            );

            _setPos(
                new Vector2(1, 3.5f),
                _radioColor
            );

            this.GetComponent<Button>("button1").CurrentPosition = new Vector2(
                3f,
                ConvertUnits.ToSimUnits(GI.Device.Viewport.Height) - 0.5f
            );

            this.GetComponent<Button>("button2").CurrentPosition = new Vector2(
                3f,
                ConvertUnits.ToSimUnits(GI.Device.Viewport.Height) - 1.5f
            );
        }
        #endregion

        #region Member - EventHandler
        private void buttonApply_MouseClick(object sender, EventArgs e)
        {
            DisplayMode mode = _res[_radioRes.SelectedButton];
            SurfaceFormat format = _color[_radioColor.SelectedButton];

            DisplayMode currentMode = _allModes.FirstOrDefault(
                m => m.Width == mode.Width && m.Height == mode.Height && m.Format == format
            );

            if (currentMode != null)
            {
                Program.Config.ScreenWidth = currentMode.Width;
                Program.Config.ScreenHeight = currentMode.Height;
                Program.Config.ScreenWindowed = _checkWindowed.Checked;
                Program.Config.Apply();
            }
        }

        private void buttonBack_MouseClick(object sender, EventArgs e)
        {
            game.ChangeGameState<stateMenu>();
        }
        #endregion

        #region Member - Xna - Draw
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _radioRes.Draw(gameTime);
            _radioColor.Draw(gameTime);
        }
        #endregion

        #region Member - Xna - Update
        public override void Update(GameTime gameTime)
        {
            _radioRes.Update(gameTime);
            _radioColor.Update(gameTime);

            base.Update(gameTime);
        }
        #endregion
    }
}
