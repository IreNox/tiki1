using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TikiEngine.Elements.Particle
{
    public class effectMoveUp : ParticleEffect
    {
        #region Vars
        private modifierInterpolator _modScale;
        private modifierInterpolator _modOpacity;
        #endregion

        #region Init
        public effectMoveUp()
        {
            _modScale = new modifierInterpolator()
            {
                ValueInit = 0.25f,
                ValueMiddle = 0.5f,
                ValueFinal = 0.25f,
                MiddlePosition = 0.5f
            };
            
            _modOpacity = new modifierInterpolator()
            {
                ValueInit = 0,
                ValueMiddle = 0.5f,
                ValueFinal = 0,
                MiddlePosition = 0.5f
            };

            this.LifeTime = 3f;
            this.TriggerTime = 0.2f;
            this.ReleaseQuantity = 10;
            this.TextureFile = "Particle/dot2";
        }
        #endregion

        #region Member
        protected override unsafe void CreateParticle(Particle* particle)
        {
            particle->Position = positionCurrent + new Vector2(
                Functions.GetRandom(-1f, 1f), 1f
            );

            particle->Velocity = new Vector2(
                -((particle->Position.X - positionCurrent.X) / this.LifeTime) + Functions.GetRandom(-0.4f, 0.4f), 
                0
            );
        }

        protected override unsafe void UpdateParticle(float elapsed, Particle* particle)
        {
            particle->Scale = _modScale.GetValue(particle->Age);
            particle->Color.W = _modOpacity.GetValue(particle->Age);

            particle->Velocity.Y -= 0.5f * elapsed;

            base.UpdateParticle(elapsed, particle);
        }
        #endregion
    }
}
