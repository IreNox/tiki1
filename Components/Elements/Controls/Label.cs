using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TikiEngine.Elements.Graphics;
using TikiEngine.Elements.Effects;

namespace TikiEngine.Elements.Controls
{
    #region Enum - Orientation
    public enum Orientation
    {
        LeftTop,
        LeftCenter,
        Center,
    }
    #endregion

    public class Label : NameObjectGraphics
    {
        #region Vars
        private string _text = "";        
        private Vector2 _size = new Vector2();

        private Vector2 _realPos;

        private Color _color = Color.White;

        private string _fontFile;
        private SpriteFont _fontXna;

        private Orientation _align = Orientation.Center;

        private static readonly shaderGlow _shader = new shaderGlow() { 
            Multiply = 0.5f,
            Color = new Vector4(0, 0.66f, 1, 1)
        };
        #endregion

        #region Init
        public Label()
            : this("font")
        { 
        }

        public Label(string fontFile)
        {
            this.FontFile = fontFile;
        }

        public Label(string text, Vector2 pos)
            : this("font", text, pos)
        { 
        }

        public Label(string fontFile, string text, Vector2 pos)
            : this(fontFile)
        {
            _text = text;

            this.StartPosition = pos;

            _setSize();
        }
        #endregion

        #region Private Member
        private void _setSize()
        {
            _size = _fontXna.MeasureString(_text);

            Vector2 offset = Vector2.Zero;

            switch (_align)
            { 
                case Orientation.Center:
                    offset = _size / 2;
                    break;
                case Orientation.LeftCenter:
                    offset = new Vector2(0, _size.Y / 2);
                    break;
            }

            _realPos = ConvertUnits.ToDisplayUnits(positionCurrent) - offset;
        }
        #endregion

        #region Member
        public override void Draw(GameTime gameTime)
        {
            GI.PostProcessingBatch.Draw(DrawBatch, _shader, layerDepth, this.SpriteBatchType);
        }

        private void DrawBatch(SpriteBatch batch)
        {
            batch.DrawString(
                _fontXna,
                _text,
                new Vector2(
                    (int)_realPos.X,
                    (int)_realPos.Y
                ),
                _color,
                0.0f,
                Vector2.Zero,
                1.0f,
                SpriteEffects.None,
                layerDepth
            );
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Dispose()
        {
        }

        protected override void ApplyChanges()
        {
        }
        #endregion

        #region Properties
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                _setSize();
            }
        }

        public Orientation Align
        {
            get { return _align; }
            set
            {
                _align = value;
                _setSize();
            }
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public Vector2 Size
        {
            get { return _size; }
        }

        public string FontFile
        {
            get { return _fontFile; }
            set
            {
                _fontFile = value;

                this.Font = GI.Content.Load<SpriteFont>(value);
            }
        }

        public SpriteFont Font
        {
            get { return _fontXna; }
            set
            {
                _fontXna = value;
                _setSize();
            }
        }

        public override Vector2 CurrentPosition
        {
            get { return base.CurrentPosition; }
            set
            {
                base.CurrentPosition = value;
                _setSize();
            }
        }

        public override bool Ready
        {
            get { return _fontXna != null; }
        }
        #endregion
    }
}
