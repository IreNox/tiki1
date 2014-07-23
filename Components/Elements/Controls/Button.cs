using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TikiEngine.Components;
using TikiEngine.Elements.Graphics;

namespace TikiEngine.Elements.Controls
{
    public class Button : NameObjectGraphics
    {
        #region Vars
        private Vector4 _rect;

        private Label _label;
        private Sprite _texOut;
        private Sprite _texOver;

        private bool _mouseOver = false;
        #endregion

        #region Vars - Events
        public event EventHandler MouseOver;
        public event EventHandler MouseClick;
        #endregion

        #region Init
        public Button()
        {
            _label = new Label();

            _texOut = new Sprite(@"controls/button_false");
            _texOver = new Sprite(@"controls/button_true");
        }

        public Button(Vector2 pos, string text, EventHandler click)
            : this(pos, text, GI.Content.Load<SpriteFont>("font"), click)
        { 
        }

        public Button(Vector2 pos, string text, SpriteFont font, EventHandler click)
            : this()
        {
            _label.Text = text;

            this.MouseClick += click;
            this.StartPosition = pos;
        }
        #endregion

        #region Private Member
        private void _setLocation()
        {
            Vector2 halfSize = ConvertUnits.ToSimUnits(_texOut.TextureSize / 2);

            _rect = new Vector4(
                positionCurrent.X - halfSize.X,
                positionCurrent.Y - halfSize.Y,
                halfSize.X * 2,
                halfSize.Y * 2
            );
        }
        #endregion

        #region Member
        public void Click()
        {
            if (MouseClick != null) MouseClick(this, EventArgs.Empty);
        }

        public override void Draw(GameTime gameTime)
        {
            _label.Draw(gameTime);
            (_mouseOver ? _texOver : _texOut).Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            _mouseOver = _rect.Contains(GI.Control.MouseSimVector());

            if (_mouseOver)
            {
                if (MouseOver != null) MouseOver(this, EventArgs.Empty);

                if (GI.Control.MouseClick(_rect)) Click();
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
            set { _label.Text = value; }
        }

        public override Vector2 CurrentPosition
        {
            get { return base.CurrentPosition; }
            set
            {
                base.CurrentPosition = value;
                _label.CurrentPosition = value;
                _texOut.CurrentPosition = value;
                _texOver.CurrentPosition = value;

                _setLocation();
            }
        }

        public override bool Ready
        {
            get { return true; }
        }
        #endregion
    }
}
