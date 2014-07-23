using System;
using System.Collections.Generic;
using System.Linq;
using TikiEngine.Elements;
using TikiEngine.Elements.Controls;
using TikiEngine.Elements.Graphics;
using Microsoft.Xna.Framework;

namespace TikiEngine.States
{
    internal class stateCredits : GameState
    {
        #region Vars
        private Label _label;

        private double _time;
        #endregion

        #region Init
        public stateCredits()
        {
            this.BackgroundStretch = false;
            this.LoadBackground("menu/menu_background");

            _label = new Label()
            {
                FontFile = "fontIF",
                CurrentPosition = new Vector2(2.5f, 14),
                SpriteBatchType = SpriteBatchType.Interface
            };

            _label.Text = 
                "Coreteam Tikitek\n" +
                "   Teamlead\n" +
	            "       Adrian Lück\n" +
                "   Programmer\n" +
	            "       Tim Boden\n" +
                "       Adrian Lück\n" +
                "   Artist\n" +
	            "        Rolf Bertz\n" +
                "        Patrick Metz\n" +
                "        Stephanie Wagner\n" +
	            "    Game Design\n" +
		        "        Pascal Jacob\n" +
                "\n" +
                "Extern\n" +
	            "    Sound Design\n" +
		        "        Luis Schöffend\n" +
		        "        Brad Fish\n" +
	            "    Game Analyst\n" +
		        "        Felix Lukas\n" +
	            "    Level Design\n" +
		        "        Marco Busse\n" +
	            "    Flying Support\n" +
		        "        Daniel Köhnlein\n" +
	            "    Quality Assurance\n" +
		        "        GA Frankfurt\n" +
                "\n" +
                "Special Thanks\n" +
                "    Edeka\n" +
                "    Farseers Physics Team";
            this.SetComponent(_label);
        }
        #endregion

        #region Member
        public override void OnActivate()
        {
            _time = 0;

            GI.Camera.RealZoom = 1.0f;
            GI.Camera.TrackingBody = null;
            GI.Camera.CurrentPosition = Vector2.Zero;
            GI.Mouse.CurrentMouse = MouseCursor.None;
        }

        public override void Update(GameTime gameTime)
        {
            _time += gameTime.ElapsedGameTime.TotalSeconds;

            _label.CurrentPosition = new Vector2(2.5f, 14) - (new Vector2(0, 0.5f) * (float)_time);

            if (_time > 20)
            {
                game.ChangeGameState<stateOptionControl>();
            }

            base.Update(gameTime);
        }
        #endregion
    }
}
