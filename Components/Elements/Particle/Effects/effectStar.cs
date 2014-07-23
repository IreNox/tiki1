using Microsoft.Xna.Framework;

namespace TikiEngine.Elements.Particle
{
    public class effectStar : ParticleEffect
    {
        #region Init
        public effectStar()
        {
            this.Budget = 1;
            this.LifeTime = 0.1f;
            this.TriggerTime = 0;
            this.ReleaseQuantity = 1;
            this.OffsetPosition = true;
            this.TextureFile = "Particle/star";
        }
        #endregion

        #region Member
        protected override unsafe void CreateParticle(Particle* particle)
        {
            particle->Position = Vector2.Zero;
        }

        protected override unsafe void UpdateParticle(float elapsed, Particle* particle)
        {
            particle->Roation += 2 * elapsed;
        }
        #endregion
    }
}
