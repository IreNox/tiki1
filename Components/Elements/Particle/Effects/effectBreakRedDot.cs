using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TikiEngine.Elements.Particle
{
    public class effectBreakRedDot : ParticleEffect
    {
        #region Vars
        private float _halfBlockSize;
        #endregion

        #region Init
        public effectBreakRedDot()
        { 
            this.Budget = 5000;
            this.LifeTime = 1.5f;
            this.TriggerTime = 0.1f;
            this.ReleaseQuantity = 40;
            this.TextureFile = "particle/dot_glow";

            _halfBlockSize = TikiConfig.BuildBlockSize / 2;
        }
        #endregion

        #region Member
        protected override unsafe void CreateParticle(Particle* particle)
        {
            particle->Color = Color.Red.ToVector4();
            particle->Scale = Functions.GetRandom(1.0f, 2.0f);
            particle->Position = positionCurrent + new Vector2(
                Functions.GetRandom(-_halfBlockSize, _halfBlockSize),
                Functions.GetRandom(-_halfBlockSize, _halfBlockSize)
            );
        }

        protected override unsafe void UpdateParticle(float elapsed, Particle* particle)
        {
            particle->Color.W = 1 - particle->Age;
        }
        #endregion
    }
}
