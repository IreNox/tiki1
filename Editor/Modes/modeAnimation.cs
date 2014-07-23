using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TikiEngine.Elements;
using TikiEngine.Editor.Controls;
using TikiEngine.Elements.Graphics;

namespace TikiEngine.Editor.Modes
{
    class modeAnimation : EditorMode
    {
        #region Vars
        private Animation _animation;

        private SpriteBatch _spriteBatch;
        #endregion

        #region Init
        public modeAnimation()
        {
            this.UseCamera = true;
        }

        public override void Init()
        {
            _spriteBatch = GI.SpriteBatch;

            this.AddTabPage<ucAnimationList>("Animations");
            this.AddTabPage<ucAnimationPropertys>("Propertys");
        }
        #endregion

        #region Member
        public override void Activate()
        {
        }
        #endregion

        #region Member - XNA
        public override void Draw(GameTime gameTime)
        {
            if (_animation.Ready)
            {
                _animation.Draw(gameTime);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (_animation == null)
            {
                _animation = new Animation();
            }

            _animation.Update(gameTime);
        }
        #endregion

        #region Member - File
        public override void New()
        {
            _animation = new Animation();

            this.RaiseObjectChanged();
        }

        public override void Open(string name)
        {
            _animation = DataManager.GetObject<Animation>(name);

            this.RaiseObjectChanged();
        }

        public override void Save()
        {
            DataManager.SetObject<Animation>(_animation);
        }

        public override void SaveAs(string name)
        {
            _animation.Name = name;
            this.Save();
        }

        public override void ShowPreferences()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Propertys
        public override Type ObjectType
        {
            get { return typeof(Animation); }
        }

        public override NameObject CurrentObject
        {
            get { return this.CurrentAnimation; }
        }

        public Animation CurrentAnimation
        {
            get { return _animation; }
            set
            {
                if (value != null)
                {
                    _animation = value;

                    this.RaiseObjectChanged();
                }
            }
        }

        public override string ObjectName
        {
            get { return _animation.Name; }
            set { _animation.Name = value; }
        }
        #endregion
    }
}
