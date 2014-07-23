using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TikiEngine.Elements.Graphics
{
    [Serializable]
    public class Parallax : NameObjectGraphics
    {
        #region Vars
        private List<ParallaxLayer> _layer = new List<ParallaxLayer>();
        #endregion

        #region Init
        public Parallax()
        {
        }

        public Parallax(params ParallaxLayer[] layer)
        {
            _layer.AddRange(layer);
        }

        public Parallax(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Member
        protected override void ApplyChanges()
        {
        }

        public override void Dispose()
        {
            _layer.ForEach(
                l => l.Dispose()
            );
        }

        public void AddLayer(string texture, float scale)
        {
            _layer.Add(
                new ParallaxLayer(texture, scale)
            );
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (ParallaxLayer pl in _layer)
            {
                pl.Draw(gameTime);
            }
        }
        #endregion

        #region Properties
        public List<ParallaxLayer> Layer
        {
            get { return _layer; }
            set { _layer = value; }
        }

        public override bool Ready
        {
            get { return true; }
        }
        #endregion

        #region Class - ParallaxLayer
        [Serializable]
        public class ParallaxLayer : NameObjectTextured
        {
            #region Vars
            private float _scale = 1;

            private float _speed = 20.0f;
            #endregion

            #region Init
            public ParallaxLayer()
            {
            }

            public ParallaxLayer(string texture, float scale)
            {
                _scale = scale;

                this.TextureFile = texture;
            }
            #endregion

            #region Member
            public override void Draw(GameTime gameTime)
            {
                if (this.Texture == null) return;

                Vector2 tmp = ConvertUnits.ToSimUnits(GI.Camera.CurrentPositionNagativ);

                spriteBatch.Draw(
                    this.Texture,
                    new Rectangle(0, 0, GI.Device.Viewport.Width, GI.Device.Viewport.Height),
                    new Rectangle((int)(250 + _speed * Scale * tmp.X),
                        (int)(250),//+ Parallax.SPEED * Scale * tmp.Y),
                        800,
                        480),
                    Color.White);

                //Vector2 pos = GI.Camera.CurrentPositionNagativ / GI.Camera.Zoom;



                //GI.SpriteBatch.Draw(
                //    _textureXna,
                //    GI.Camera.ViewRectangle,
                //    new Rectangle(
                //        (int)(pos.X * _scale),
                //        (int)(pos.Y * _scale),
                //        800,
                //        480
                //    ),
                //    Color.DarkGray
                //);

                /*GameInstances.SpriteBatch.Draw(
                    texture,
                    new Rectangle(
                        (int)position.X + 150,
                        (int)position.Y + 300,
                        800,
                        480
                    ),
                    new Rectangle(0, 0, 800, 480),
                    Color.White
                );*/
            }

            public override void Update(GameTime gameTime)
            {                
            }

            protected override void ApplyChanges()
            {
            }
            #endregion

            #region Properties
            public float Scale
            {
                get { return _scale; }
                set { _scale = value; }
            }

            public override bool Ready
            {
                get { return this.Texture != null; }
            }
            #endregion
        }
        #endregion
    }
}
