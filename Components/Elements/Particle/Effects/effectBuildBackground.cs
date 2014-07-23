using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TikiEngine.Elements.Particle
{
    public class effectBuildBackground : ParticleEffect
    {
        #region Init
        public effectBuildBackground()
        {
            this.Budget = 50;
            this.LifeTime = 1.5f;
            this.TriggerTime = 0.5f;
            this.ReleaseQuantity = 1;
            this.TextureFile = "particle/dot";
        }
        #endregion

        #region Member
        protected override unsafe void CreateParticle(Particle* particle)
        {
            particle->Color = new Vector4(0, 0.66f, 1, 1);
            particle->Scale = 25;
        }

        protected override unsafe void UpdateParticle(float elapsed, Particle* particle)
        {
            particle->Color.W = 1 - particle->Age;

            base.UpdateParticle(elapsed, particle);
        }
        #endregion
    }
}
