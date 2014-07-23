using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TikiEngine.Elements.Physic
{
    [Serializable]
    public class PhysicCircle : NameObjectPhysic
    {
        #region Vars
        private float _radius = 1f;
        private Vector2 _origin = new Vector2(0.5f);
        #endregion

        #region Init
        public PhysicCircle()
            : base()
        {
            this.ApplyChanges();
        }

        public PhysicCircle(float radius, float density, String texture)
            : base(texture)
        {
            _radius = radius;
            _origin = this.TextureSize / 2;

            this.ApplyChanges();
        }

        public PhysicCircle(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { 
        }
        #endregion

        #region Member - XNA
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(
                this.Texture,
                new Rectangle(
                    (int)ConvertUnits.ToDisplayUnits(body.Position.X),
                    (int)ConvertUnits.ToDisplayUnits(body.Position.Y),
                    (int)ConvertUnits.ToDisplayUnits(_radius * 2),
                    (int)ConvertUnits.ToDisplayUnits(_radius * 2)
                    ),
                null,
                Color.White,
                this.Body.Rotation,
                _origin,
                SpriteEffects.None,
                0f
            );

            base.Draw(gameTime);
        }
        #endregion

        #region Member - Protected
        protected override void CreateBody()
        {
            body = BodyFactory.CreateCircle(GI.World, _radius, this.Density);
        }
        #endregion

        #region Properties
        public override bool Ready
        {
            get { return true; }
        }
        #endregion
    }
}
