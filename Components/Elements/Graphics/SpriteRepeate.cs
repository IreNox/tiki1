using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TikiEngine.Elements.Graphics
{
    [Flags]
    public enum Direction
    {
        Vertical,
        Horizontally
    }

    public class SpriteRepeate : NameObjectTextured
    {
        #region Vars
        private Direction _direction;
        #endregion

        #region Init
        public SpriteRepeate()
        { 
        }

        public SpriteRepeate(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { 
        }
        #endregion

        #region Member - Protected
        protected override void ApplyChanges()
        {
        }
        #endregion

        #region Member - Xna - Draw
        public override void Draw(GameTime gameTime)
        {
            float x = 0;
            float y = 0;

            if ((_direction & Direction.Horizontally) == Direction.Horizontally)
            {
                x = GI.Camera.CurrentPosition.X - (GI.Camera.CurrentPosition.X % this.TextureSize.X);
            }
            else
            {
                x = this.CurrentPosition.X;
            }

            if ((_direction & Direction.Vertical) == Direction.Vertical)
            {
                y = GI.Camera.CurrentPosition.Y - (GI.Camera.CurrentPosition.Y % this.TextureSize.Y);
            }
            else
            {
                y = this.CurrentPosition.Y;
            }

            spriteBatch.Draw(
                this.Texture,
                new Vector2(x, y),
                null,
                Color.White,
                0,
                this.TextureSize / 2,
                1.0f,
                SpriteEffects.None,
                this.LayerDepth
            );
        }
        #endregion

        #region Member - Xna - Update
        public override void Update(GameTime gameTime)
        {
        }
        #endregion

        #region Properies
        public Direction Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        public override bool Ready
        {
            get { return this.Texture != null; }
        }
        #endregion
    }
}
