using System;
using Microsoft.Xna.Framework;
using TikiEngine.Elements.Graphics;
using TikiEngine.Elements.Controls;

namespace TikiEngine.Elements
{
    internal class TutorialPoint : NameObject
    {
        #region Vars
        private bool _visible = false;

        private string _text;

        private Label _label;
        private Circle _circle;

        public event EventHandler Started;
        public event EventHandler Finished;
        #endregion

        #region Init
        public TutorialPoint(string text, string img, Vector2 pos)
        {
            _text = text;

            _label = new Label()
            {
                FontFile = "fontIF",
                Text = text,
                Align = Orientation.LeftCenter,                
                SpriteBatchType = SpriteBatchType.Interface
            };

            _circle = new Circle(
                new Circle.CircleElement("circle/circle_m1_0", 0.0001f),
                new Circle.CircleElement("circle/circle_m0_1", -0.0005f),
                new Circle.CircleElement("circle/circle_m1_2", 0.0002f),
                new Circle.CircleElement("tutorial/" + img, 0)
            ) { 
                SpriteBatchType = SpriteBatchType.Interface
            };

            this.CurrentPosition = pos;
        }
        #endregion

        #region Member
        public void Start(GameTime gameTime)
        {
            GI.Voice.addMessage(_text);
            GI.Voice.play();

            _visible = true;

            if (this.Started != null)
            {
                this.Started(this, EventArgs.Empty);
            }
        }

        protected override void ApplyChanges()
        {
        }

        public override void Dispose()
        {
        }
        #endregion

        #region Memner - Xna - Draw
        public override void Draw(GameTime gameTime)
        {
            if (_visible)
            {
                _label.Draw(gameTime);
                _circle.Draw(gameTime);
            }
        }
        #endregion

        #region Member - Xna - Update
        public override void Update(GameTime gameTime)
        {
            _circle.Update(gameTime);

            if (_visible && !GI.Voice.Playing)
            {
                _visible = false;

                if (this.Finished != null)
                {
                    this.Finished(this, EventArgs.Empty);
                }
            }
        }
        #endregion

        #region Properties
        public override Vector2 CurrentPosition
        {
            get { return base.CurrentPosition; }
            set
            {
                base.CurrentPosition = value;
                
                _circle.CurrentPosition = value;
                _label.CurrentPosition = value + new Vector2(2.1f, 0);
            }
        }

        public override bool Ready
        {
            get { return true; }
        }
        #endregion
    }
}
