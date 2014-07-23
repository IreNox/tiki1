using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TikiEngine.Elements.Particle
{
    public class effectMask : ParticleEffect
    {
        #region Vars
        private Vector2 _size;

        private string _textureMaskFile;
        private Texture2D _textureMaskXna;

        private Vector4 _black = new Vector4(0, 0, 0, 1);

        private Vector2[] _maskVector;
        private Dictionary<Vector2, Vector4> _maskData = new Dictionary<Vector2, Vector4>();

        private Vector2 _mouseCache;
        private float _mouseVelocity;

        private modifierInterpolator _inter;
        #endregion

        #region Init
        public effectMask()
            : base(BlendState.AlphaBlend)
        {
            this.Budget = 40000;
            this.LifeTime = 2;
            this.TriggerTime = 0.025f;
            this.ReleaseQuantity = 500;
            this.TextureFile = "Particle/dot";

            _inter = new modifierInterpolator() { 
                ValueInit = 1,
                ValueMiddle = 1,
                ValueFinal = 0,
                MiddlePosition = 0.7f
            };
        }
        #endregion

        #region Private Member
        private void _loadTexture()
        {
            _maskData.Clear();

            Vector2 size = new Vector2(_textureMaskXna.Width, _textureMaskXna.Height);

            if (_size == Vector2.Zero)
            { 
                _size = size;
            }

            float scale = (1.0f * _size.X) / _textureMaskXna.Width;

            RenderTarget2D target = new RenderTarget2D(GI.Device, GI.Device.Viewport.Width, GI.Device.Viewport.Height);
            GI.Device.SetRenderTarget(target);
            GI.Device.Clear(Color.Black);

            GI.SpriteBatch.Begin();
            GI.SpriteBatch.Draw(
                _textureMaskXna,
                GI.Camera.ScreenCenter,
                null,
                Color.White,
                0,
                size / 2,
                scale,
                SpriteEffects.None,
                0
            );
            GI.SpriteBatch.End();
            GI.Device.SetRenderTarget(null);

            Color[] maskData = new Color[target.Width * target.Height];
            target.GetData<Color>(maskData);
            target.Dispose();

            int i = 0;
            int width = target.Width;
            _maskData.AddRange(
                maskData.Select(
                    c => {
                        Vector2 pos = new Vector2(i % width, i / width);

                        i++;

                        return new KeyValuePair<Vector2, Vector4>(ConvertUnits.ToSimUnits(pos), c.ToVector4());
                    }
                ).Where(kvp => kvp.Value != _black).ToArray()
            );
            _maskVector = _maskData.Keys.ToArray();
        }
        #endregion

        #region Member
        public override void Update(GameTime gameTime)
        {
            _mouseCache = GI.Control.MouseSimVectorNoCamera();
            _mouseVelocity = GI.Control.MouseDistanceSim().Length() * 4;

            base.Update(gameTime);
        }

        protected override unsafe void CreateParticle(Particle* particle)
        {
            Vector2 v = _maskVector[Functions.GetRandom(0, _maskData.Count - 1)];
            Vector4 c = _maskData[v]; // _black; // (Functions.GetRandom(0, 1) == 0 ? _maskData[v] : _black);

            particle->Temp = c.W;
            particle->Color = c;
            particle->Position = v;
        }

        protected override unsafe void UpdateParticle(float elapsed, Particle* particle)
        {
            particle->Scale = particle->Age * 0.25f;
            particle->Color.W = _inter.GetValue(particle->Age) * particle->Temp;

            float dis = (0.5f - Vector2.DistanceSquared(particle->Position, _mouseCache)) * _mouseVelocity;
            if (dis > 0 && particle->PositionStart != Vector2.Zero)
            {
                particle->Color.X = dis;
                particle->Velocity = (particle->Position - _mouseCache) * (dis * 50);

                particle->PositionStart = Vector2.Zero;
            }

            base.UpdateParticle(elapsed, particle);
        }
        #endregion

        #region Properties
        public Texture2D TextureMask
        {
            get { return _textureMaskXna; }
            set
            {
                _textureMaskXna = value;

                if (_textureMaskXna != null)
                {
                    _loadTexture();
                }
            }
        }

        public Vector2 Size
        {
            get { return _size; }
            set
            {
                _size = value;

                if (_textureMaskXna != null)
                {
                    _loadTexture();
                }
            }
        }

        public string TextureMaskFile
        {
            get { return _textureMaskFile; }
            set
            {
                _textureMaskFile = value;
                this.TextureMask = GI.Content.Load<Texture2D>(value);
            }
        }

        public override bool Ready
        {
            get { return base.Ready && _textureMaskXna != null; }
        }
        #endregion
    }
}
