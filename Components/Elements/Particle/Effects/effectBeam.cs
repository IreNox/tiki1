using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TikiEngine.Elements.Particle
{
    public class effectBeam : ParticleEffect
    {
        #region Init
        public effectBeam()
        {
            this.Budget = 5;
            this.LifeTime = 2;
            this.TriggerTime = 0.4f;
            this.ReleaseQuantity = 1;
            this.TextureFile = "Particle/beam";
        }
        #endregion

        #region Member
        protected override unsafe void  CreateParticle(Particle* particle)
        {
            particle->Scale = 0.5f;
            particle->Color = new Vector4(0.5019608f, 0.847058833f, 1, 1);
            particle->Position = positionCurrent + new Vector2(
                Functions.GetRandom(-1f, 1),
                Functions.GetRandom(-0.5f, 0.5f)
            );
        }

        protected override unsafe void UpdateParticle(float elapsed, Particle* particle)
        {
            particle->Color.W = (float)Math.Sin(particle->Age * MathHelper.Pi);
        }
        #endregion    
    }
}
