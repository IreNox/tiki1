using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TikiEngine.Elements.Graphics;
using Microsoft.Xna.Framework;
using TikiEngine.Elements.Controls;

namespace TikiEngine.States
{
    internal class stateOptionControl : GameState
    {
        #region Vars
        private Animation _ani = new Animation("Elements/debug_circlewings", 1, 12, 0, 11, 100, 3, true, true);
        #endregion

        #region Init
        public stateOptionControl()
        {
            this.BackgroundStretch = false;
            this.LoadBackground("menu/menu_background");

            this.SetComponent(_ani);

            this.SetComponent(
                new Label()
                {
                    FontFile = "fontIF",
                    Text = "Thank you for help",
                    CurrentPosition = new Vector2(4, 4)
                }
            );
        }
        #endregion

        #region Member - Xna - Update
        public override void Update(GameTime gameTime)
        {
            _ani.CurrentPosition = new Vector2(
                (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds) * 3,
                (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds) * 3
            ) + new Vector2(MathHelper.Pi * 1.5f);

            base.Update(gameTime);
        }
        #endregion
    }
}

