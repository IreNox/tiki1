using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using TikiEngine.Elements.Controls;
using Microsoft.Xna.Framework;

namespace TikiEngine.States
{
    internal class stateOptionAudio : GameState
    {
        #region Vars
        private Label _labelMusic;
        private Label _labelSound;
        private Label _labelVoice;

        private ScrollBar _scrollMusic;
        private ScrollBar _scrollSound;
        private ScrollBar _scrollVoice;
        #endregion

        #region Init
        public stateOptionAudio()
        {
            this.BackgroundStretch = false;
            this.LoadBackground("menu/menu_background");

            _labelMusic = new Label("fontIF")
            {
                Text = "Music:",
                StartPosition = new Vector2(1, 0.5f)
            };
            this.SetComponent(_labelMusic);

            _labelSound = new Label("fontIF")
            {
                Text = "Sound:",
                StartPosition = new Vector2(1, 1.5f)
            };
            this.SetComponent(_labelSound);

            _labelVoice = new Label("fontIF")
            {
                Text = "Voice:",
                StartPosition = new Vector2(1, 2.5f)
            };
            this.SetComponent(_labelVoice);

            _scrollMusic = new ScrollBar()
            {
                Width = 5,
                Value = GI.Sound.MusicVolume,
                CurrentPosition = new Vector2(4, 1f)
            };
            this.SetComponent(_scrollMusic);

            _scrollSound = new ScrollBar()
            {
                Width = 5,
                Value = GI.Sound.SoundVolume,
                CurrentPosition = new Vector2(4, 2f)
            };
            this.SetComponent(_scrollSound);

            _scrollVoice = new ScrollBar()
            {
                Width = 5,
                Value = GI.Voice.Volume,
                CurrentPosition = new Vector2(4, 3f)
            };
            this.SetComponent(_scrollVoice);

            Button button = new Button();
            button.Text = "Back";
            button.FontFile = "fontIF";
            button.MouseClick += new EventHandler(buttonBack_MouseClick);
            this.SetComponent("button", button);

            this.ResetScreen();
        }
        #endregion

        #region Member
        public override void ResetScreen()
        {
            this.GetComponent<Button>("button").CurrentPosition = new Vector2(
                3f,
                ConvertUnits.ToSimUnits(GI.Device.Viewport.Height) - 0.5f
            );
        }
        #endregion

        #region Member - Xna - Update
        public override void Update(GameTime gameTime)
        {
            Program.Config.AudioMusic = _scrollMusic.Value;
            Program.Config.AudioSound = _scrollSound.Value;
            Program.Config.AudioVoice = _scrollVoice.Value;
            Program.Config.Apply();

            base.Update(gameTime);
        }
        #endregion

        #region Member - EventHandler
        private void buttonBack_MouseClick(object sender, EventArgs e)
        {
            game.ChangeGameState<stateMenu>();
        }
        #endregion

    }
}
