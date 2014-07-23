using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TikiEngine.Elements.Graphics;

namespace TikiEngine.Elements
{
    [Serializable]
    public class Grid : NameObjectGraphics
    {
        #region Vars
        private int _size = 1;
        private int _cellSize = 50;

        private Color _color = Color.Red;
        [NonSerialized]
        private Texture2D _texture;
        #endregion

        #region Init
        public Grid()
        {
            _init();
        }

        public Grid(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _init();
        }

        private void _init()
        {
            _texture = new Texture2D(
                GI.Device,
                1,
                1
            );
            _texture.SetData(
                new Color[] {
                    Color.White
                }
            );
        }
        #endregion

        #region Member
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Vector2 pos = GI.Camera.CurrentPositionNagativ;

            int x = (int)(pos.X - (pos.X % _cellSize));
            int y = (int)(pos.Y - (pos.Y % _cellSize));
            int width = GI.Device.PresentationParameters.BackBufferWidth;
            int height = GI.Device.PresentationParameters.BackBufferHeight;

            for (; y < pos.Y + height; y += _cellSize)
            {
                for (; x < pos.X + width; x += _cellSize)
                {
                    spriteBatch.Draw(
                        _texture,
                        new Rectangle(
                            x,
                            (int)pos.Y,
                            _size,
                            height
                        ),
                        _color
                    );
                }

                spriteBatch.Draw(
                    _texture,
                    new Rectangle(
                        (int)pos.X,
                        y,
                        width,
                        _size
                    ),
                    _color
                );
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
        }

        public override void Dispose()
        {
            _texture.Dispose();
        }
        #endregion

        #region Member - Protected
        protected override void ApplyChanges()
        {
        }
        #endregion

        #region Properties
        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public int CellSize
        {
            get { return _cellSize; }
            set { _cellSize = value; }
        }

        public override bool Ready
        {
            get { return true; }
        }
        #endregion
    }
}
