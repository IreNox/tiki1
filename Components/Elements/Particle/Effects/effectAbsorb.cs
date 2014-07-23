using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TikiEngine.Elements.Particle
{
    public class effectAbsorb : ParticleEffect
    {
        #region Vars
        private float _size = 1;
        #endregion

        #region Init
        public effectAbsorb()
        {
            this.Budget = 250;
            this.LifeTime = 1;
            this.TriggerTime = 0.002f;
            this.ReleaseQuantity = 2;
            this.TextureFile = "Particle/dot";
            this.OffsetPosition = true;
        }
        #endregion

        #region Member
        protected override unsafe void CreateParticle(Particle* particle)
        {
            float a = Functions.GetRandom(0f, MathHelper.TwoPi);
            Vector2 circle = new Vector2(
                (float)Math.Sin(a),
                (float)Math.Cos(a)
            ) * _size;

            particle->Scale = 0.4f;
            particle->Position = circle * 1.5f;
        }

        protected override unsafe void UpdateParticle(float elapsed, Particle* particle)
        {
            particle->Color.W = (float)Math.Sin(particle->Age * MathHelper.Pi);
            particle->Velocity = -particle->Position * elapsed * (200 / _size);

            base.UpdateParticle(elapsed, particle);
        }
        #endregion

        #region Properties
        public float Size
        {
            get { return _size; }
            set { _size = value; }
        }
        #endregion
    }
}
