using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TikiEngine.Elements.Graphics;

namespace TikiEngine.Elements.Controls
{
    public class ScrollBar : NameObjectGraphics
    {
        #region Vars
        private Sprite _texLeft;
        private Sprite _texRight;
        private Sprite _texPointer;

        private Vector4 _recMiddle;
        private Texture2D _texMiddle;

        private float _value;
        private float _valueMin = 0.0f;
        private float _valueMax = 1.0f;

        private float _width = 2.0f;

        private bool _selected;
        #endregion

        #region Init
        public ScrollBar()
        {
            _texLeft = new Sprite()
            {
                TextureFile = "controls/scroll_left",
                LayerDepth = 0.2f
            };
            _texRight = new Sprite()
            {
                TextureFile = "controls/scroll_right",
                LayerDepth = 0.2f
            };
            _texPointer = new Sprite()
            {
                TextureFile = "controls/scroll_pointer",
                LayerDepth = 1.0f
            };

            _texMiddle = GI.Content.Load<Texture2D>("controls/scroll_middle");

            this.ApplyChanges();
        }
        #endregion

        #region Private Member
        private void _setRect()
        {
            Vector2 halfSize = ConvertUnits.ToSimUnits(_texPointer.TextureSize / 2);

            _recMiddle = new Vector4(
                (positionCurrent.X + _valueToPos()) - halfSize.X,
                positionCurrent.Y - halfSize.Y,
                halfSize.X * 2,
                halfSize.Y * 2
            );
        }

        private float _valueToPos()
        {
            float value = ((_value - _valueMin) / (_valueMax - _valueMin)) * _width;
            
            return value - (_width / 2);
        }

        private void _posToValue()
        {
            Vector2 mouse = GI.Control.MouseSimVector();
            float pos = mouse.X - (this.CurrentPosition.X - (_width / 2));

            this.Value = ((pos / _width) * (_valueMax - _valueMin)) + _valueMin;
        }
        #endregion

        #region Member
        protected override void ApplyChanges()
        {
            float halfWidth = (_width / 2) + ConvertUnits.ToSimUnits(_texLeft.TextureSize.X / 2);

            _texLeft.CurrentPosition = this.CurrentPosition - new Vector2(halfWidth, 0);
            _texRight.CurrentPosition = this.CurrentPosition + new Vector2(halfWidth, 0);

            _setRect();
        }

        public override void Draw(GameTime gameTime)
        {
            _texLeft.Draw(gameTime);
            _texRight.Draw(gameTime);

            spriteBatch.Draw(
                _texMiddle,
                ConvertUnits.ToDisplayUnits(this.CurrentPosition),
                null,
                Color.White,
                0.0f,
                new Vector2(0.5f, (float)_texMiddle.Height / 2),
                new Vector2(
                    ConvertUnits.ToDisplayUnits(_width),
                    1
                ),
                SpriteEffects.None,
                layerDepth
            );

            _texPointer.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (_selected)
            {
                if (GI.Control.MouseDown(MouseButtons.Left))
                {
                    _posToValue();
                }
                else
                {
                    _selected = false;
                }
            }
            else if (_recMiddle.Contains(GI.Control.MouseSimVector()))
            {
                _selected = true;
            }

            if (_value > _valueMax) this.Value = _valueMax;
            if (_value < _valueMin) this.Value = _valueMin;

            _texPointer.CurrentPosition = this.CurrentPosition + new Vector2(_valueToPos(), 0);
        }

        public override void Dispose()
        {
        }
        #endregion

        #region Properties
        public float Value
        {
            get { return _value; }
            set
            {
                _value = value;
                _setRect();
            }
        }

        public float ValueMin
        {
            get { return _valueMin; }
            set { _valueMin = value; }
        }

        public float ValueMax
        {
            get { return _valueMax; }
            set { _valueMax = value; }
        }
        
        public float Width
        {
            get { return _width; }
            set
            {
                _width = value;
                this.ApplyChanges();
            }
        }

        public override Vector2 CurrentPosition
        {
            get { return base.CurrentPosition; }
            set
            {
                base.CurrentPosition = value;
                this.ApplyChanges();
            }
        }

        public override bool Ready
        {
            get { return true; }
        }
        #endregion
    }
}
