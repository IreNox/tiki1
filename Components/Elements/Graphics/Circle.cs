using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace TikiEngine.Elements.Graphics
{
    [Serializable]
    public class Circle : NameObjectGraphics
    {
        #region Vars
        private List<CircleElement> _listElements = new List<CircleElement>();
        #endregion

        #region Init
        public Circle()
        {
        }

        public Circle(params CircleElement[] elements)
        {
            _init(elements);
        }

        public Circle(IEnumerable<CircleElement> elements)
        {
            _init(elements);
        }

        public Circle(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { 
        }

        private void _init(IEnumerable<CircleElement> elements)
        {
            _listElements = new List<CircleElement>(elements);

            this.CurrentPosition = positionCurrent;
        }
        #endregion

        #region Member
        public override void Dispose()
        {
            _listElements.ForEach(
                e => e.Dispose()
            );
        }
        #endregion

        #region Member - XNA
        public override void Draw(GameTime gameTime)
        {
            foreach (var e in _listElements)
            {
                e.Draw(gameTime);
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var e in _listElements)
            {
                e.Update(gameTime);
            }
        }
        #endregion

        #region Member - Protected
        protected override void ApplyChanges()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Properties
        public CircleElement this[int index]
        {
            get { return _listElements[index]; }
            set { _listElements[index] = value; }
        }

        public List<CircleElement> Elements
        {
            get { return _listElements; }
        }

        public override bool Ready
        {
            get { return _listElements.Count != 0; }
        }

        public override Vector2 CurrentPosition
        {
            get { return base.CurrentPosition; }
            set
            {
                base.CurrentPosition = value;

                foreach (CircleElement e in _listElements)
                {
                    e.CurrentPosition = value;
                }
            }
        }

        public float Scale
        {
            set
            {
                foreach (CircleElement e in _listElements)
                {
                    e.Scale = value;
                }
            }
        }

        public override SpriteBatchType  SpriteBatchType
        {
            get 
	        { 
		         return base.SpriteBatchType;
	        }
	        set 
	        { 
		        base.SpriteBatchType = value;

                foreach (var e in _listElements)
                {
                    e.SpriteBatchType = value;
                }
            }
        }
        #endregion

        #region Class - CircleElement
        public class CircleElement : NameObjectGraphics, IDisposable
        {
            #region Vars
            private float _angleMin;
            private float _angleMax;

            private Sprite _sprite;
            private float _angleSpeed;
            #endregion

            #region Init
            public CircleElement(string texture, float angleSpeed)
            {
                _sprite = new Sprite() { 
                    TextureFile = texture
                };

                _angleSpeed = angleSpeed;
            }

            public CircleElement(string texture, float angleSpeed, float angleMin, float angleMax)
                : this(texture, angleSpeed)
            {
                _angleMin = angleMin;
                _angleMax = angleMax;
            }
            #endregion

            #region Member
            protected override void ApplyChanges()
            {
                throw new NotImplementedException();
            }

            public override void Draw(GameTime gameTime)
            {
                _sprite.Draw(gameTime);
            }

            public override void Update(GameTime gameTime)
            {
                if (_angleMin != 0 && _angleMax != 0)
                {
                    float t = (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds % MathHelper.TwoPi);

                    _sprite.Angle = _angleMin + ((_angleMax - _angleMin) * t);
                }
                else
                {
                    _sprite.Angle += _angleSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }

            public override void Dispose()
            {
                _sprite.Dispose();
            }
            #endregion

            #region Properties
            public float Angle
            {
                get { return _sprite.Angle; }
                set { _sprite.Angle = value; }
            }

            public float Scale
            {
                get { return _sprite.Scale; }
                set { _sprite.Scale = value; }
            }

            public float Opacity
            {
                get { return _sprite.Opacity; }
                set { _sprite.Opacity = value; }
            }

            public float AngleMin
            {
                get { return _angleMin; }
                set { _angleMin = value; }
            }

            public float AngleMax
            {
                get { return _angleMax; }
                set { _angleMax = value; }
            }

            public override bool Ready
            {
                get { return true; }
            }

            public override Vector2 CurrentPosition
            {
                get { return _sprite.CurrentPosition; }
                set { _sprite.CurrentPosition = value; }
            }

            public override SpriteBatchType SpriteBatchType
            {
                get { return base.SpriteBatchType; }
                set
                {
                    base.SpriteBatchType = value;
                    if (_sprite != null) _sprite.SpriteBatchType = value;
                }
            }
            #endregion
        }
        #endregion
    }
}
