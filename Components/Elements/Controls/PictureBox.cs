using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TikiEngine.Elements.Graphics;

namespace TikiEngine.Elements.Controls
{
    public class PictureBox : NameObjectGraphics
    {
        #region Vars
        private float _opacity = 1.0f;
        private Color _color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        private Rectangle _rect;
        private Texture2D _texture;
        #endregion

        #region Init
        public PictureBox()
        {
        }

        public PictureBox(Texture2D texture, Rectangle rect)
            : this()
        {
            _rect = rect;
            _texture = texture;
        }
        #endregion
        
        #region Member
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(
                _texture,
                _rect,
                _color
            );
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Dispose()
        {
            _texture.Dispose();
        }

        protected override void ApplyChanges()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Properties
        public float Opacity
        {
            get { return _opacity; }
            set
            {
                _opacity = value;
                _color = new Color(_opacity, _opacity, _opacity, _opacity);
            }
        }

        public Rectangle Rectangle
        {
            get { return _rect; }
            set { _rect = value; }
        }

        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        public override bool Ready
        {
            get { return _texture != null; }
        }
        #endregion
    }
}
