using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TikiEngine.Elements.Physic;

namespace TikiEngine.Elements.Graphics
{
    [Serializable]
    public class Sprite : NameObjectTextured, IAttachable
    {
        #region Vars
        private float _angle = 0;
        private float _scale = 1;
        private float _opacity = 1;

        private float speed;

        private Color _opacityColor = Color.White;

        private Vector2 _origin;
        private Vector2 _offset;
        #endregion

        #region Init
        public Sprite()
        { 
        }

        public Sprite(string textureFile)
        {
            this.TextureFile = textureFile;
        }

        public Sprite(string textureFile, float angle, float scale)
            : this(textureFile)
        {
            this.Angle = angle;
            this.Scale = scale;
        }

        public Sprite(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { 
        }
        #endregion

        #region Member
        protected override void ApplyChanges()
        {
            throw new NotImplementedException();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(
                this.Texture,
                ConvertUnits.ToDisplayUnits(this.CurrentPosition),
                null,
                _opacityColor,
                _angle,
                _origin - ConvertUnits.ToDisplayUnits(_offset),
                _scale,
                SpriteEffects.None,
                this.LayerDepth
            );
        }

        public void Draw(GameTime gameTime, NameObjectPhysic nop)
        {
            spriteBatch.Draw(
                this.Texture,
                ConvertUnits.ToDisplayUnits(nop.Body.Position),
                null,
                _opacityColor,
                nop.Body.Rotation,
                _origin - ConvertUnits.ToDisplayUnits(Offset),
                _scale,
                SpriteEffects.None,
                this.LayerDepth
            );
        }

        public override void Update(GameTime gameTime)
        {
        }
        #endregion

        #region Member - Protected
        protected override void SetTexture(Texture2D texture)
        {
            base.SetTexture(texture);

            _origin = this.TextureSize / 2;
        }
        #endregion

        #region Properties
        public float Speed
        {
            get { return this.speed; }
            set { this.speed = value; }
        }

        public float Angle
        {
            get { return _angle; }
            set { _angle = value; }
        }

        public float Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        public Vector2 Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }

        public Vector2 Offset
        {
            get { return this._offset; }
            set { this._offset = value; }
        }

        public float Opacity
        {
            get { return _opacity; }
            set
            {
                _opacity = value;
                _opacityColor = new Color(value, value, value, value);
            }
        }

        public Color Color
        {
            get { return _opacityColor; }
            set { _opacityColor = value; }
        }

        public override bool Ready
        {
            get { return this.Texture != null; }
        }
        #endregion
    }
}
