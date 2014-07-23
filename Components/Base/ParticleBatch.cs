using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using TikiEngine.Elements.Particle;
using Microsoft.Xna.Framework;

namespace TikiEngine
{
    public class ParticleBatch : BasicBatch<ParticleBatch.ParticleDrawInfo>
    {
        #region Vars
        public readonly static BlendState NonPremultipliedAdditive = new BlendState()
        {
            AlphaBlendFunction = BlendFunction.Add,
            AlphaDestinationBlend = Blend.One,
            AlphaSourceBlend = Blend.SourceAlpha,
            ColorBlendFunction = BlendFunction.Add,
            ColorDestinationBlend = Blend.One,
            ColorSourceBlend = Blend.SourceAlpha,
        };
        #endregion

        #region Init
        public ParticleBatch(GraphicsDevice device)
            : base(device)
        { 
        }
        #endregion

        #region Member
        public void Draw(Particle[] particles, int count, Vector2 origin, ParticleEffect effect)
        { 
            drawQueue.Add(
                new ParticleDrawInfo(particles, count, origin, effect)
            );
        }
        #endregion

        #region Member - Protected
        protected override void SpriteBatchBegin(ParticleDrawInfo info)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, info.Effect.BlendState, null, null, null, null, GI.Camera.ViewMatrix2D);
        }
        #endregion

        #region Class - ParticleDrawInfo
        public class ParticleDrawInfo : BasicBatch<ParticleDrawInfo>.BasicDrawInfo
        {
            #region Vars
            private int _count;
            private Vector2 _origin;
            private Particle[] _particles;

            public ParticleEffect Effect;
            #endregion

            #region Init
            internal ParticleDrawInfo(Particle[] particles, int count, Vector2 origin, ParticleEffect effect)
                : base(effect.LayerDepth, effect.SpriteBatchType)
            {
                _count = count;
                _origin = origin;
                _particles = particles;

                this.Effect = effect;
            }
            #endregion

            #region Member
            public override void Draw(SpriteBatch batch)
            {
                bool offsetPosition = this.Effect.OffsetPosition;
                Vector2 positionCurrent = this.Effect.CurrentPosition;
                Texture2D texture = this.Effect.Texture;

                unsafe
                {
                    fixed (Particle* particles = _particles)
                    {
                        int i = _count;
                        Particle* particle = particles + (i - 1);

                        while (--i >= 0)
                        {
                            Vector2 pos = ConvertUnits.ToDisplayUnits(offsetPosition ? positionCurrent + particle->Position : particle->Position);

                            batch.Draw(
                                texture,
                                pos,
                                null,
                                new Color(particle->Color),
                                particle->Roation,
                                _origin,
                                particle->Scale,
                                SpriteEffects.None,
                                1
                            );

                            particle--;
                        }
                    }
                }
            }
            #endregion
            
            #region Operators
            public override bool Equals(object obj)
            {
                if (obj is ParticleDrawInfo)
                {
                    return this == (ParticleDrawInfo)obj;
                }

                return false;
            }

            public override int GetHashCode()
            {
                return (int)(100 * this.LayerDepth) * ((int)this.BatchType) * (this.Effect.BlendState != null ? this.Effect.BlendState.GetHashCode() : 1);
            }

            public static bool operator ==(ParticleDrawInfo o1, ParticleDrawInfo o2)
            {
                return (o1.LayerDepth == o2.LayerDepth) && (o1.BatchType == o2.BatchType) && (o1.Effect.BlendState == o2.Effect.BlendState);
            }

            public static bool operator !=(ParticleDrawInfo o1, ParticleDrawInfo o2)
            {
                return !(o1 == o2);
            }
            #endregion
        }
        #endregion
    }
}
