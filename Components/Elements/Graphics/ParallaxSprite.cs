using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TikiEngine.Elements.Graphics
{
    [Serializable]
    public class ParallaxSprite : NameObjectGraphics
    {
        #region Vars
        private Texture2D backgroundImage;
        private List<ParallaxLayer> _layer = new List<ParallaxLayer>();
        #endregion

        #region Init
        public ParallaxSprite()
        {
            this.SpriteBatchType = SpriteBatchType.Parallax;
            this.backgroundImage = GI.Content.Load<Texture2D>("Layer/cloud_background 2");
        }

        public ParallaxSprite(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { 
        }
        #endregion

        #region Member
        public ParallaxLayer AddLayer()
        {
            return new ParallaxLayer();
        }

        public override void Dispose()
        {
            _layer.ForEach(
                l => l.Dispose()
            );
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
            if (backgroundImage != null)
            {
                spriteBatch.Draw(
                     backgroundImage,
                     new Rectangle(
                         0,
                         0,
                         //(int)(GI.Camera.CurrentPositionNagativ.X),
                         //(int)(GI.Camera.CurrentPositionNagativ.Y),
                         (int)(GI.Device.Viewport.Width),// / GI.Camera.Zoom),
                         (int)(GI.Device.Viewport.Height)// / GI.Camera.Zoom)
                     ),
                     null,
                     Color.White,
                     0,
                     Vector2.Zero,
                     SpriteEffects.None,
                 0);
            }

            foreach (ParallaxLayer layer in _layer)
            {
                layer.Draw(gameTime);
            }
        }
        #endregion

        #region Member - Xna - Update
        public override void Update(GameTime gameTime)
        {
            foreach (ParallaxLayer layer in _layer)
            {
                layer.Update(gameTime);
            }
        }
        #endregion

        #region Properties
        public override bool Ready
        {
            get { return _layer.All(l => l.Ready); }
        }

        public List<ParallaxLayer> Layer
        {
            get { return _layer; }
            set { _layer = value; }
        }

        public override float LayerDepth
        {
            get { return base.LayerDepth; }
            set
            {
                base.LayerDepth = value;

                foreach (ParallaxLayer layer in _layer)
                {
                    layer.LayerDepth = value;
                }
            }
        }

        public override SpriteBatchType  SpriteBatchType
        {
            get { return base.SpriteBatchType; }
	        set 
	        { 
		        base.SpriteBatchType = value;

                foreach (ParallaxLayer layer in _layer)
                {
                    layer.SpriteBatchType = value;
                }
	        }
        }
        #endregion

        #region Class - ParallaxLayer
        [Serializable]
        public class ParallaxLayer : NameObjectGraphics
        {
            #region Vars
            private float _scale = 1.0f;

            private List<Sprite> _sprites = new List<Sprite>();
            #endregion

            #region Init
            public ParallaxLayer()
            {
            }

            public ParallaxLayer(SerializationInfo info, StreamingContext context)
                : base(info, context)
            { 
            }
            #endregion

            #region Member
            public override void Dispose()
            {
                _sprites.ForEach(
                    s => s.Dispose()
                );
            }
            #endregion

            #region Member - Xna
            public override void Draw(GameTime gameTime)
            {
                foreach (Sprite sprite in _sprites)
                {
                    if (Vector2.Distance(sprite.CurrentPosition, Vector2.Zero) < 30)
                    {
                        sprite.Draw(gameTime);
                    }
                }
            }

            public override void Update(GameTime gameTime)
            {
                //Später in Relation zum Abstand einer Insel vll
                float speedX = -0.005f;

                float y = GI.Camera.TrackingPosition.Y;
                //float koef = (float)Math.Sqrt(Math.Abs(y));

                foreach (Sprite sprite in _sprites)
                {
                    sprite.CurrentPosition = new Vector2(
                        sprite.CurrentPosition.X + (speedX * sprite.Speed),
                        sprite.StartPosition.Y + 0.05f * -y * sprite.Speed
                    );

                    #region Debug Old
                    //if (GI.Control.KeyboardDown(Keys.D))
                    //{
                    //    if (_scale != 0)
                    //    {
                    //        sprite.CurrentPosition += new Vector2(4 * speedX, 0);
                    //    }
                    //    else
                    //    {
                    //        sprite.CurrentPosition += new Vector2(test, 0);
                    //        sprite.CurrentPosition += new Vector2(speedBX, 0);
                    //    }
                    //}
                    //else if (GI.Control.KeyboardDown(Keys.A))
                    //{
                    //    if (_scale != 0)
                    //    {
                    //        sprite.CurrentPosition += new Vector2(-2 * speedX * 0.9f, 0);
                    //    }
                    //    else
                    //    {
                    //        sprite.CurrentPosition += new Vector2(-speedBX * 0.9f, 0);
                    //        sprite.CurrentPosition += new Vector2(speedBX, 0);
                    //    }
                    //}
                    //else
                    //{
                    //    if (_scale != 0)
                    //    {
                    //        sprite.CurrentPosition += new Vector2(2 * speedX, 0);
                    //    }
                    //    else
                    //    {
                    //        sprite.CurrentPosition += new Vector2(speedBX, 0);
                    //    }
                    //}

                    //if (GI.Control.KeyboardDown(Keys.D))
                    //{
                    //    if (_scale != 0)
                    //    {
                    //        sprite.CurrentPosition += new Vector2(-0.01f * 1.2f, 0);
                    //    }
                    //    else
                    //    {
                    //        sprite.CurrentPosition += new Vector2(-0.02f * 1.2f, 0);
                    //    }
                    //}
                    //else if (GI.Control.KeyboardDown(Keys.A))
                    //{
                    //    if (_scale != 0)
                    //    {
                    //        sprite.CurrentPosition += new Vector2(-0.01f * 0.9f, 0);
                    //    }
                    //    else
                    //    {
                    //        sprite.CurrentPosition += new Vector2(0.02f * 1.2f, 0);
                    //    }
                    //}
                    //else
                    //{
                    //    if (_scale != 0)
                    //    {
                    //        sprite.CurrentPosition += new Vector2(-0.01f, 0);
                    //    }
                    //    else
                    //    {
                    //        sprite.CurrentPosition += new Vector2(-0.002f, 0);
                    //    }
                    //}

                    //sprite.CurrentPosition = sprite.StartPosition - (GI.Camera.CurrentPosition / (GI.Camera.ScreenCenter * 2) * ((1 - _scale) * 10));

                    //if (Scale == 0)
                    //{
                    //    //sprite.CurrentPosition += new Vector2(-0.005f, 0);
                    //}
                    //else
                    //{
                    //    sprite.CurrentPosition = sprite.StartPosition - (GI.Camera.CurrentPosition / (GI.Camera.ScreenCenter * 2) * ((1 - _scale) * 10));
                    //}
                    #endregion
                }
            }
            #endregion

            #region Member - Protected
            protected override void ApplyChanges()
            {                
            }
            #endregion
            
            #region Properties
            public override bool Ready
            {
                get { return _sprites.All(s => s.Ready); }
            }

            public float Scale
            {
                get { return _scale; }
                set 
                { 
                    _scale = value;
                    foreach (Sprite s in _sprites)
                    {
                        if (s.Speed != 0) s.Speed = value;
                    }
                }
            }

            public List<Sprite> Sprites
            {
                get { return _sprites; }
                set { _sprites = value; }
            }

            public override float LayerDepth
            {
                get { return base.LayerDepth; }
                set
                {
                    base.LayerDepth = value;

                    foreach (Sprite s in _sprites)
                    {
                        s.LayerDepth = value;
                    }
                }
            }
            #endregion
        }
        #endregion
    }
}
