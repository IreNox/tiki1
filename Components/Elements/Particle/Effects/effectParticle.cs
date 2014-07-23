using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TikiEngine.Elements.Particle
{
    public class effectParticle : ParticleEffect
    {
        #region Vars
        private float _width;
        private float _halfWidth;
        #endregion

        #region Init
        public effectParticle()
        {
            this.Budget = 1000;
            this.LifeTime = 5f;
            this.TriggerTime = 0.1f;
            this.TextureFile = "Particle/particle";
        }
        #endregion

        #region Member
        protected override unsafe void CreateParticle(Particle* particle)
        {
            particle->Color = new Vector4(
                0.4f,
                0.33f,
                0.25f,
                Functions.GetRandom(0.3f, 0.7f)
            );

            particle->Scale = Functions.GetRandom(0.1f, 0.8f);
            particle->Velocity.Y = Functions.GetRandom(1f, 2f);
            particle->Position.X += Functions.GetRandom(-_halfWidth, _halfWidth);
        }

        protected override unsafe void UpdateParticle(float elapsed, Particle* particle)
        {
            particle->Velocity.Y += 2f * elapsed;

            base.UpdateParticle(elapsed, particle);
        }
        #endregion

        #region Properties
        public float Width
        {
            get { return _width; }
            set
            {
                _width = value;
                _halfWidth = value / 2;
            }
        }
        #endregion
    }
}
