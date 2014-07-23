using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TikiEngine.Elements.Particle
{
    public class effectLighting : ParticleEffect
    {
        #region Init
        public effectLighting()
        {
            this.Budget = 4;
            this.LifeTime = 4;
            this.TriggerTime = 2f;
            this.ReleaseQuantity = 1;
            this.TextureFile = "Particle/light";
        }
        #endregion

        #region Member
        protected override unsafe void  CreateParticle(Particle* particle)
        {
            particle->Scale = 4.5f;
            particle->Roation = MathHelper.Pi;
            particle->Color = new Vector4(0.5019608f, 0.847058833f, 1, 1);
        }

        protected override unsafe void UpdateParticle(float elapsed, Particle* particle)
        {
            particle->Color.W = (float)Math.Sin(particle->Age * MathHelper.Pi);
            particle->Position.Y = particle->PositionStart.Y - (float)Math.Cos(particle->Age * MathHelper.TwoPi) - 1.5f;
        }
        #endregion  
    }
}
