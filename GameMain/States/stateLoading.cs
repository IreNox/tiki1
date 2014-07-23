using System;
using System.Linq;
using System.Collections.Generic;
using TikiEngine.Elements.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TikiEngine.States
{
    internal class stateLoading : GameState
    {
        #region Vars
        private Circle _circle;

        private Action _doAction = null;

        private long loadingTime = 0;

        #endregion

        #region Init
        public stateLoading()
        {
            this.BackgroundImage = GI.Content.Load<Texture2D>("menu/menu_background");
            this.BackgroundStretch = false;

            _circle = new Circle(
                new Circle.CircleElement("circle/circle_b_0", -0.001f),
                new Circle.CircleElement("circle/circle_b_1", 0.0005f),
                new Circle.CircleElement("circle/circle_b_2", -0.0015f),
                new Circle.CircleElement("base/loading_text", 0, 0.001f, 0.5f)
            );

            this.ResetScreen();

            this.SetComponent(_circle);
        }
        #endregion

        #region Member
        public override void OnActivate()
        {
            GI.Camera.RealZoom = 1.0f;
            GI.Camera.TrackingBody = null;
            GI.Camera.CurrentPosition = Vector2.Zero;
            GI.Mouse.CurrentMouse = MouseCursor.None;
        }

        public override void ResetScreen()
        {
            _circle.CurrentPosition = ConvertUnits.ToSimUnits(GI.Camera.ScreenCenter);
        }
        #endregion

        #region Member - Xna
        public override void Update(GameTime gameTime)
        {
            this.loadingTime += gameTime.ElapsedGameTime.Milliseconds;
            base.Update(gameTime);

            if (_doAction != null)
            {
                _doAction();
                Console.WriteLine("LadeZeit : " + this.loadingTime);
                this.loadingTime = 0;
                _doAction = null;
            }
        }
        #endregion

        #region Properties
        public Action DoAction
        {
            get { return _doAction; }
            set { _doAction = value; }
        }
        #endregion
    }
}
