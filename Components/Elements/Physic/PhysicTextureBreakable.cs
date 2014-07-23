using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using TikiEngine.Elements.Graphics;

namespace TikiEngine.Elements.Physic
{
    [Serializable]
    public class PhysicTextureBreakable : NameObjectPhysicPolygon
    {
        #region Vars
        private bool _broken = false;
        private bool _drawSprite = false;

        private float _strength = 20;

        private BreakableBody _breakableBody;
        #endregion

        #region Init
        public PhysicTextureBreakable()
        {
        }

        public PhysicTextureBreakable(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Member
        public override void Reset()
        {
            if (_broken)
            {
                this.RefreshBody = true;
                this.CurrentPosition = this.StartPosition;
            }

            base.Reset();
        }
        #endregion

        #region Member - Xna - Draw
        public override void Draw(GameTime gameTime)
        {
            if (_drawSprite && !_breakableBody.Broken)
            {
                spriteBatch.Draw(
                    this.Texture,
                    ConvertUnits.ToDisplayUnits(this.CurrentPosition),
                    null,
                    Color.White,
                    body.Rotation,
                    this.TextureSize / 2,
                    1.0f,
                    SpriteEffects.None,
                    this.LayerDepth
                );

                foreach (AttachedElement aa in this.AttachedAssets)
                {
                    aa.Draw(gameTime);
                }
            }
            else
            {
                base.Draw(gameTime);
            }
        }
        #endregion

        #region Member - Xna - Update
        public override void Update(GameTime gameTime)
        {
            if (_breakableBody != null && _broken != _breakableBody.Broken)
            {
                this.ReselectParts = _broken = true;
            }

            base.Update(gameTime);
        }
        #endregion

        #region Member - Protected
        protected override Body CreateBodyByList(List<Vertices> list)
        {
            if (_breakableBody != null)
            {
                world.BreakableBodyList.Remove(_breakableBody);
                _breakableBody = null;
            }

            _broken = false;

            _breakableBody = new BreakableBody(list, world, density, this);
            _breakableBody.Strength = _strength;
            world.AddBreakableBody(_breakableBody);

            _breakableBody.MainBody.UserData = this;
            _breakableBody.MainBody.BodyType = bodyType;
            _breakableBody.MainBody.Position = positionCurrent;

            return _breakableBody.MainBody;
        }
        #endregion

        #region Properties
        public bool DrawSprite
        {
            get { return _drawSprite; }
            set { _drawSprite = value; }
        }

        public float Strength
        {
            get { return _strength; }
            set
            {
                _strength = value;
                if (_breakableBody != null) _breakableBody.Strength = _strength;
            }
        }

        public BreakableBody BreakableBody
        {
            get { return _breakableBody; }
        }

        public bool Broken
        {
            get { return _broken; }
        }
        #endregion
    }
}
