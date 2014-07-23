using System;
using System.Runtime.Serialization;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TikiEngine.Elements.Graphics;
using TikiEngine.Components;

namespace TikiEngine.Elements.Physic
{
    [Serializable]
    public class PhysicBox : NameObjectPhysic, IAttachable
    {
        #region Vars
        private Vector2 _size = new Vector2(1);
        private Vector2 _origin = new Vector2(0.5f);
        private Vector2 _offset = Vector2.Zero;

        private RotatedRectangle rotatedRectangle;

        private Animation _animation;
        #endregion

        #region Init
        public PhysicBox()
        {
            this.ApplyChanges();
        }

        public PhysicBox(Vector2 size, Vector2 position, float density, string texture)
            : base(null, texture)
        {
            _size = size;

            this.ApplyChanges();

            body.Position = position;
            body.BodyType = BodyType.Dynamic;
        }    
    
        public PhysicBox(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Member - Xna - Draw
        public override void Draw(GameTime gameTime)
        {
            if (body == null) return;

            if (_animation == null)
            {
                spriteBatch.Draw(
                    this.Texture,
                    new Rectangle(
                        (int)ConvertUnits.ToDisplayUnits(body.Position).X,
                        (int)ConvertUnits.ToDisplayUnits(body.Position).Y,
                        (int)(_size.X * 100f),
                        (int)(_size.Y * 100f)
                    ),
                    null,
                    Color.White,
                    body.Rotation,
                    _origin,
                    SpriteEffects.None,
                    this.LayerDepth
                );
            }
            else
            { 
                _animation.Angle = body.Rotation;
                _animation.CurrentPosition = body.Position;
                _animation.Draw(gameTime);
            }

#if DEBUG
            if (this.rotatedRectangle != null)
            {
                this.rotatedRectangle.Draw(gameTime);
            }
#endif

            base.Draw(gameTime);
        }
        #endregion

        #region Member - Xna - Update
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_animation != null)
            {
                _animation.Update(gameTime);
            }

            if (this.rotatedRectangle != null)
            {
                this.rotatedRectangle.ChangePosition(ConvertUnits.ToDisplayUnits(this.CurrentPosition - Size / 2));
                this.rotatedRectangle.Rotation = this.Angle;
            }
        }
        #endregion

        #region Member - Protected
        protected override void CreateBody()
        {
            body = BodyFactory.CreateRectangle(world, _size.X, _size.Y, density, positionCurrent);
            body.BodyType = bodyType;
            body.UserData = this;

            this.rotatedRectangle = new RotatedRectangle(
                new Rectangle(0, 0, (int)ConvertUnits.ToDisplayUnits(_size.X), (int)ConvertUnits.ToDisplayUnits(_size.Y)));
            this.rotatedRectangle.Color = Color.Blue;

            //_origin = ConvertUnits.ToDisplayUnits(_size);
        }

        protected override void SetTexture(Texture2D texture)
        {
            _origin = new Vector2(
                texture.Width / 2f,
                texture.Height / 2f
            );

            base.SetTexture(texture);
        }
        #endregion

        #region Properties
        public override Vector2 CurrentPosition
        {
            get { return base.CurrentPosition; }
            set
            {
                if (body == null)
                {
                    base.CurrentPosition = value;
                }
                else
                {
                    base.CurrentPosition = value + Vector2.Transform(
                        _offset,
                        Matrix.CreateRotationZ(body.Rotation)
                    );
                }
            }
        }

        public RotatedRectangle Bounds
        {
            set { this.rotatedRectangle = value; }
            get { return this.rotatedRectangle; }
        }

        public override bool Ready
        {
            get { return _size.X > 0 && _size.Y > 0 && this.Texture != null; }
        }

        public Vector2 Size
        {
            get { return _size; }
            set
            {
                _size = value;
                this.RefreshBody = true;

                this.ApplyChanges();
            }
        }

        public Animation Animation
        {
            get { return _animation; }
            set 
            { 
                _animation = value;
                _animation.IsAlive = true;
            }
        }

        public float Angle
        {
            get { return body.Rotation; }
            set
            {
                if (body != null && body.IsReady()) body.Rotation = value;
            }
        }

        public Vector2 Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }
        #endregion
    }
}
