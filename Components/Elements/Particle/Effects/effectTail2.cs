using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TikiEngine.Elements.Particle
{
    public class effectTail2 : ParticleEffect
    {
        #region Vars
        private int _thisCount;
        private float _thisTime;
        private Vector2 _thisWay;

        private float _lastTime;
        private Vector2 _lastPosition;
        #endregion

        #region Init
        public effectTail2()
            : base(ParticleBatch.NonPremultipliedAdditive)
        {
            this.Budget = 5000;
            this.ReleaseQuantity = 10;
            this.LifeTime = 0.5f;
            this.TextureFile = "Particle/dot";
        }
        #endregion

        #region Member
        public override void Update(GameTime gameTime)
        {
            _thisCount = 0;
            _thisTime = (_lastTime - this.TotalTime);
            _thisWay = (_lastPosition - positionCurrent);
            this.ReleaseQuantity = (int)(_thisWay.Length() * 15);
            
            _thisWay /= this.ReleaseQuantity;
            _thisTime /= this.ReleaseQuantity;
            
            this.ReleaseQuantity--;

            base.Update(gameTime);

            _lastTime = this.TotalTime;
            _lastPosition = positionCurrent;
        }

        protected override unsafe void CreateParticle(Particle* particle)
        {
            particle->Color = Color.Red.ToVector4();
            particle->Position = positionCurrent + (_thisWay * _thisCount);
            particle->BirthTime -= (_thisTime * _thisCount);
            _thisCount++;
        }

        protected override unsafe void UpdateParticle(float elapsed, Particle* particle)
        {
            //float angle = (particle->Age * MathHelper.TwoPi) * 2;

            particle->Scale = 3f - (particle->Age / 2f);
            particle->Color.W = 0.5f - (particle->Age / 2);
            //particle->Position = particle->PositionStart + new Vector2(
            //    (float)Math.Sin(angle),
            //    (float)Math.Cos(angle + MathHelper.PiOver2)
            //);
        }
        #endregion

        #region Properties
        public Vector2 LastPosition
        {
            get { return _lastPosition; }
            set { _lastPosition = value; }
        }
        #endregion
    }
}
