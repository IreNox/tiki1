using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TikiEngine.Elements.Particle
{
    public class effectTail : ParticleEffect
    {
        #region Vars
        private int _thisCount;
        private Vector2 _thisWay;
        private Vector2 _lastPosition;
        #endregion

        #region Init
        public effectTail()
            : base(ParticleBatch.NonPremultipliedAdditive)
        {
            this.Budget = 5000;
            this.ReleaseQuantity = 10;
            this.LifeTime = 2;
            this.TextureFile = "Particle/light";
        }
        #endregion

        #region Member
        public override void Update(GameTime gameTime)
        {
            _thisCount = 0;
            _thisWay = (_lastPosition - positionCurrent) / this.ReleaseQuantity;

            base.Update(gameTime);

            _lastPosition = positionCurrent;
        }

        protected override unsafe void CreateParticle(Particle* particle)
        {
            particle->Scale = 0.5f;

            particle->Position = positionCurrent + (_thisWay * _thisCount);
            //float halfLen = _len / 2;

            //float pos = Functions.GetRandom(-halfLen, halfLen);

            //particle->Position = positionCurrent + new Vector2(
            //    pos + (float)Math.Sin(_angle),
            //    pos + (float)Math.Cos(_angle)
            //);

            _thisCount++;
        }

        protected override unsafe void UpdateParticle(float elapsed, Particle* particle)
        {
            Vector2 diff = positionCurrent - particle->PositionStart;
            float dot = (float)Math.Atan2(diff.X, diff.Y) + MathHelper.Pi;
            float dis = Vector2.Distance(particle->PositionStart, positionCurrent);
            float disMod = particle->Age * 30; // (_thisWay.Length() * 1000)
            
            dot += ((float)Math.Sin(disMod) / (dis * 2 + 1));

            Vector2 angle = new Vector2(
                (float)Math.Sin(dot),
                (positionCurrent == particle->PositionStart ? 0 : (float)Math.Cos(dot))
            );

            particle->Scale = 1f - (particle->Age / 2f);
            particle->Position = positionCurrent + (angle * dis);
            particle->Color.W = 1 - particle->Age;
        }
        #endregion

        #region Properties
        public Vector2 LastPosition
        {
            get { return _lastPosition; }
        }
        #endregion
    }
}
