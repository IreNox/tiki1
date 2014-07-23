using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TikiEngine.Elements;


namespace TikiEngine.States
{
    public abstract class GameState : DrawableGameComponent
    {
        #region Vars
        private bool _useCamera = false;

        private bool _backgroundStretch = true;
        private Color _backgroundColor = Color.CornflowerBlue;
        private Texture2D _backgroundImage;

        protected GameMain game;
        protected SpriteBatch spriteBatch;

        private Dictionary<string, NameObject> _components = new Dictionary<string, NameObject>();
        #endregion

        #region Init
        public GameState()
            : base(Program.Game)
        {
            this.game = Program.Game;
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();

            spriteBatch = GI.SpriteBatch;
        }
        #endregion

        #region Properties
        public bool UseCamera
        {
            get { return _useCamera; }
            set { _useCamera = value; }
        }

        public Texture2D BackgroundImage
        {
            get { return this._backgroundImage; }
            set { this._backgroundImage = value; }
        }

        public bool BackgroundStretch
        {
            get { return _backgroundStretch; }
            set { _backgroundStretch = value; }
        }

        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; }
        }

        public Dictionary<string, NameObject> Components
        {
            get { return _components; }
        }
        #endregion

        #region Member
        public void LoadBackground(String name)
        {
            _backgroundImage = GI.Content.Load<Texture2D>(name);
        }

        public virtual void ResetScreen()
        { 
        }
        #endregion

        #region Member - Components
        public T SetComponent<T>(T value)
            where T : NameObject
        {
            return this.SetComponent(
                Guid.NewGuid().ToString(),
                value
            );
        }

        public T SetComponent<T>(string key, T value)
            where T : NameObject
        {
            _components[key] = value;

            return value;
        }

        public T GetComponent<T>(string name)
            where T : NameObject
        {
            if (_components.ContainsKey(name))
            {
                return _components[name] as T;
            }

            return default(T);
        }
        #endregion

        #region Member - Xna - Draw
        public override void Draw(GameTime gameTime)
        {
            if (_backgroundImage != null)
            {
                if (_backgroundStretch)
                {
                    GI.SpriteBatch.Draw(
                         _backgroundImage,
                         new Rectangle(
                             (int)(GI.Camera.CurrentPositionNagativ.X),
                             (int)(GI.Camera.CurrentPositionNagativ.Y),
                             (int)(GI.Device.Viewport.Width / GI.Camera.RealZoom),
                             (int)(GI.Device.Viewport.Height / GI.Camera.RealZoom)
                         ),
                         null,
                         Color.White,
                         0,
                         Vector2.Zero,
                         SpriteEffects.None,
                         0.01f
                     );
                }
                else
                {
                    GI.SpriteBatch.Draw(
                         _backgroundImage,
                         new Vector2(
                             GI.Camera.ScreenCenter.X,
                             GI.Camera.ScreenCenter.Y
                         ),
                         null,
                         Color.White,
                         0,
                         _backgroundImage.GetCenter(),
                         1.1f,
                         SpriteEffects.None,
                         0
                     );
                }
            }

            foreach (NameObject com in _components.Values)
            {
                com.Draw(gameTime);
            }

            base.Draw(gameTime);
        }
        #endregion

        #region Member - Xna - Update
        public override void Update(GameTime gameTime)
        {
            foreach (NameObject com in _components.Values.ToArray())
            {
                com.Update(gameTime);
            }

            base.Update(gameTime);
        }
        #endregion

        #region Member - Abstract
        public virtual void OnActivate()
        { 
        }
        #endregion
    }
}
