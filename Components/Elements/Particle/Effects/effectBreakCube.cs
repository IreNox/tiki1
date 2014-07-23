using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TikiEngine.Elements.Particle
{
    public class effectBreakCube : ParticleEffect
    {
        #region Vars
        private float _cubeSizeMod;
        #endregion

        #region Init
        public effectBreakCube()
        {
            this.Budget = 1;
            this.LifeTime = 1;
            this.TriggerTime = 0f;
            this.ReleaseQuantity = 1;
            this.TextureFile = "elements/cube";

            _cubeSizeMod = (1 / ConvertUnits.ToSimUnits(this.TextureSize.X)) * TikiConfig.BuildBlockSize;
        }
        #endregion

        #region Member
        protected override unsafe void CreateParticle(Particle* particle)
        {
            particle->Scale = _cubeSizeMod;
        }

        protected override unsafe void UpdateParticle(float elapsed, Particle* particle)
        {
            particle->Color.W = 1 - particle->Age;
        }
        #endregion
    }
}
