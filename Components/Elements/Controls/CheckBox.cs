using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TikiEngine.Elements.Graphics;

namespace TikiEngine.Elements.Controls
{
    public class CheckBox : NameObjectGraphics
    {
        #region Vars
        protected bool varChecked;

        private Label _label;

        private Vector4 _recClick;

        protected Sprite textureChecked;
        protected Sprite textureUnchecked;

        private static readonly Vector2 _lableOffset = new Vector2(0.35f, 0.06f);
        #endregion

        #region Init
        public CheckBox()
        {
            _label = new Label();

            textureChecked = new Sprite(@"controls/check_true");
            textureUnchecked = new Sprite(@"controls/check_false");
        }

        public CheckBox(string text, Vector2 pos)
            : this()
        {
            _label.Text = text;
            this.StartPosition = pos;
        }
        #endregion

        #region Private Member
        private void _setRectangle()
        {
            Vector2 size = new Vector2(
                ConvertUnits.ToSimUnits(_label.Size.X) + _lableOffset.X,
                ConvertUnits.ToSimUnits(textureChecked.TextureSize.Y)
            );
            Vector2 center = ConvertUnits.ToSimUnits(textureChecked.TextureSize / 2);

            _recClick = new Vector4(
                positionCurrent.X - center.X,
                positionCurrent.Y - center.Y,
                size.X + (center.X * 2),
                size.Y + (center.Y * 2)
            );
        }
        #endregion

        #region Member
        public override void Draw(GameTime gameTime)
        {
            _label.Draw(gameTime);
            (varChecked ? textureChecked : textureUnchecked).Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (GI.Control.MouseClick(_recClick))
            {
                varChecked = !varChecked;
            }
        }

        public override void Dispose()
        {
            _label.Dispose();
        }

        protected override void ApplyChanges()
        {
        }
        #endregion

        #region Properties
        public virtual bool Checked
        {
            get { return varChecked; }
            set { varChecked = value; }
        }

        public string FontFile
        {
            get { return _label.FontFile; }
            set { _label.FontFile = value; }
        }

        public SpriteFont Font
        {
            get { return _label.Font; }
            set { _label.Font = value; }
        }

        public string Text
        {
            get { return _label.Text; }
            set
            {
                _label.Text = value;

                _setRectangle();
            }
        }

        public override Vector2 CurrentPosition
        {
            get { return base.CurrentPosition; }
            set
            {
                base.CurrentPosition = value;

                textureChecked.CurrentPosition = value;
                textureUnchecked.CurrentPosition = value;
                _label.CurrentPosition = value + _lableOffset + new Vector2(_label.Size.X / 200, 0);
                _setRectangle();
            }
        }

        public override bool Ready
        {
            get { return true; }
        }
        #endregion
    }
}
