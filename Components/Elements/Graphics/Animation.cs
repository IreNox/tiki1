using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;
using System.Runtime.Serialization;
using TikiEngine.Elements.Physic;

namespace TikiEngine.Elements.Graphics
{
    [Serializable]
    public class Animation : NameObjectTextured, IAttachable
    {
        #region Vars
        protected int _rows;
        protected int _columns;

        protected Vector2 _size;
        protected Vector2 _origin;
        protected Vector2 _offset;

        protected double _time;
        protected long _timePerFrame;
        protected float _animationSpeed;
        protected float _scale = 1;

        protected int _stopFrame;
        protected int _startFrame;
        protected int _currentFrame;

        protected bool bounds = false;
        protected bool _isLoop;
        protected bool _isAlive;
        protected SpriteEffects spriteEffect = SpriteEffects.None;

        protected float _angle;
        #endregion

        #region Init
        public Animation()
        { 
        }

        public Animation(string texture, int rows, int columns, int startframe, int stopframe, float animationSpeed, bool loop)
            : this(texture, rows, columns, startframe, stopframe, animationSpeed, 1.0f, 0.0f, loop, false)
        {
        }

        public Animation(string texture, int rows, int columns, int startframe, int stopframe, float animationSpeed, float scale, bool loop, bool isAlive)
            :this(texture, rows, columns, startframe, stopframe, animationSpeed, scale, 0.0f, loop, isAlive)
        {
        }

        public Animation(string texture, int rows, int columns, int startframe, int stopframe, float animationSpeed,float scale, bool loop)
            :this(texture, rows, columns, startframe, stopframe, animationSpeed, scale, 0.0f, loop, false)
        {
        }

        public Animation(string texture, int rows, int columns, int startframe, int stopframe, float animationSpeed, float scale, float angle, bool loop, bool isAlive)
            : base(texture)
        {
            _rows = rows;
            _columns = columns;
            _startFrame = startframe;
            _stopFrame = stopframe;
            _animationSpeed = animationSpeed;
            _currentFrame = startframe;
            _time = 0;
            _isLoop = loop;
            _scale = scale;

            this.IsAlive = isAlive;

            _setSize();
            _setSpeed();
        }

        public Animation(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { 
        }
        #endregion

        #region Private Member
        private void _setSize()
        {
            if (this.Texture != null && _columns != 0 && _rows != 0)
            {
                _size = new Vector2(
                    this.Texture.Width / _columns,
                    this.Texture.Height / _rows
                );

                _origin = _size / 2;
            }
        }

        private void _setSpeed()
        {
            _timePerFrame = (long)_animationSpeed / (_stopFrame - _startFrame);
        }
        #endregion

        #region Member - Xna - Draw
        public override void Draw(GameTime gameTime)
        {
            Point size = _size.ToPoint();

            spriteBatch.Draw(
                this.Texture,
                ConvertUnits.ToDisplayUnits(this.CurrentPosition),
                new Rectangle(
                    _currentFrame % _columns * size.X,
                    _currentFrame / _columns * size.Y,
                    size.X,
                    size.Y
                ),
                Color.White,
                _angle,
                _origin - ConvertUnits.ToDisplayUnits(_offset),
                _scale,
                this.spriteEffect,
                this.LayerDepth
            );
        }
        #endregion

        #region Member - XNA
        public override void Update(GameTime gameTime)
        {


            if(!this.IsAlive)
                return;

            this._time += gameTime.ElapsedGameTime.Milliseconds;

            if (this._time > this._timePerFrame)
            {

                if (CurrentFrame + 1 <= StopFrame)
                {
                    CurrentFrame++;
                }
                else
                {
                    if (Loop)
                    {
                        CurrentFrame = StartFrame;
                    }
                    else
                    {
                        IsAlive = false;
                        CurrentFrame = StopFrame;
                    }
                }
                //_currentFrame++;

                //if (this._currentFrame > _stopFrame)
                //{
                //    if (this.Loop)
                //    {
                //        _currentFrame = _startFrame;
                //    }
                //    else
                //    {
                //        _isAlive = false;
                //        _currentFrame = _stopFrame;
                //    }

                //}
                this._time %= this._timePerFrame;
            }
            
        }
        #endregion

        #region Member - Control
        public float Stop()
        {
            float tmp = (float)_time;
            _isAlive = false;
            _time = 0;
            _currentFrame = StopFrame;
            return tmp;
        }
        #endregion

        #region Member - Protected
        protected override void ApplyChanges()
        {
        }

        protected override void SetTexture(Texture2D texture)
        {
            _setSize();

            base.SetTexture(texture);
        }
        #endregion

        #region Properties
        public bool Bounds
        {
            get { return this.bounds; }
            set { this.bounds = value; }
        }
        public float Scale
        {
            get { return this._scale; }
            set { this._scale = value; }
        }

        public int StartFrame
        {
            get { return _startFrame; }
            set { _startFrame = value; }
        }

        public SpriteEffects SpriteEffect
        {
            get { return this.spriteEffect; }
            set { this.spriteEffect = value; }
        }

        public int StopFrame
        {
            get { return _stopFrame; }
            set { _stopFrame = value; }
        }

        public int FrameCount
        {
            get { return _stopFrame - _startFrame; }
        }

        public int Rows
        {
            get { return _rows; }
            set
            {
                _rows = value;
                _setSize();
            }
        }

        public int Columns
        {
            get { return _columns; }
            set
            {
                _columns = value;
                _setSize();
            }
        }

        [NonSerializedTiki]
        public bool IsAlive
        {
            get { return this._isAlive; }
            set 
            { 
                _isAlive = value;
                CurrentFrame = StartFrame;
            }
        }

        [NonSerializedTiki]
        public int CurrentFrame
        {
            get { return this._currentFrame; }
            set
            {
                _currentFrame = value;

                if (value >= _stopFrame) _currentFrame = _stopFrame;
                if (value <= _startFrame) _currentFrame = _startFrame;
            }
        }

        public float AnimationSpeed
        {
            get { return _animationSpeed; }
            set
            {
                _animationSpeed = value;
                if (_animationSpeed <= 0) _animationSpeed = 1;
                _setSpeed();
            }
        }
        public float TimePerFrame
        {
            set 
            {
                this._animationSpeed = (_stopFrame - _startFrame ) * value;
                this._timePerFrame = (long)value;
            }
        }
        public float Time
        {
            get { return (float)this._time; }
            set 
            { 
                this._time = value;
                this._isAlive = true;
            }
        }

        public bool Loop
        {
            get { return _isLoop; }
            set { _isLoop = value; }
        }

        public override bool Ready
        {
            get
            {
                return this.Texture != null &&
                        _rows > 0 &&
                        _columns > 0 &&
                        _stopFrame > 0 &&
                        _size.X > 0 &&
                        _size.Y > 0;
            }
        }

        public float Angle
        {
            get { return _angle; }
            set { _angle = value; }
        }

        public Vector2 Size
        {
            get { return _size; }
        }

        public Vector2 Origin
        {
            get { return _origin; }
        }

        public Vector2 Offset
        {
            get { return _offset; }
            set { _offset = value; }

        }
        #endregion
    }
}
