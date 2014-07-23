using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TikiEngine.Elements.Graphics;
using TikiEngine.Elements.Physic;

namespace TikiEngine
{
    public enum MouseCursor
    {
        None,
        Build,
        Default,
        Destroy
    }

    public class MouseComponent : DrawableGameComponent
    {
        #region Vars
        private MouseCursor _currentState;
        private MouseStateData _currentData;

        private Vector2 _circleOffset = new Vector2(0.24f, 0.24f);

        private Dictionary<MouseCursor, MouseStateData> _states = new Dictionary<MouseCursor, MouseStateData>();
        #endregion

        #region Init
        public MouseComponent()
            : base(GI.Game)
        {
            this.Initialize();
        }

        public override void Initialize()
        {
            _states[MouseCursor.None] = null;
            _addState(MouseCursor.Build);
            _addState(MouseCursor.Default);
            _addState(MouseCursor.Destroy);

            base.Initialize();
        }

        private void _addState(MouseCursor state)
        {
            string name = state.ToString().ToLower();

            _states.Add(
                state,
                new MouseStateData(
                    new Sprite(@"mouse/" + name + "_p")
                    {
                        SpriteBatchType = SpriteBatchType.Interface,
                        Origin = Vector2.Zero,
                        Scale = 0.6f
                    },
                    new Circle(
                        new Circle.CircleElement("mouse/" + name + "_s_0", 0.002f),
                        new Circle.CircleElement("mouse/" + name + "_s_1", -0.001f)
                    )
                    {
                        SpriteBatchType = SpriteBatchType.Interface,
                        Scale = 0.6f
                    }
                )
            );
        }
        #endregion

        #region Private Member
        private void _setPosition()
        {
            Vector2 pos = GI.Control.MouseSimVectorNoCamera();

            if (_currentData != null)
            {
                _currentData.Sprite.CurrentPosition = pos;
                _currentData.Circle.CurrentPosition = pos + _circleOffset;
            }
        }
        #endregion

        #region Member - Xna - Draw
        public override void Draw(GameTime gameTime)
        {
            if (_currentData != null)
            {
                _currentData.Sprite.Draw(gameTime);
                _currentData.Circle.Draw(gameTime);
            }

            base.Draw(gameTime);
        }
        #endregion

        #region Member - Xna - Update
        public override void Update(GameTime gameTime)
        {
            _setPosition();

            if (_currentData != null)
            {
                _currentData.Circle.Update(gameTime);
            }

            base.Update(gameTime);
        }
        #endregion

        #region Properties
        public MouseCursor CurrentMouse
        {
            get { return _currentState; }
            set
            {
                if (value == _currentState) return;

                _currentState = value;

                _currentData = _states[value];
                _setPosition();
            }
        }
        #endregion

        #region Class - MouseState
        private class MouseStateData
        {
            #region Vars
            public Sprite Sprite;
            public Circle Circle;
            #endregion

            #region Init
            public MouseStateData(Sprite sprite, Circle circle)
            {
                this.Sprite = sprite;
                this.Circle = circle;
            }
            #endregion
        }
        #endregion
    }
}
