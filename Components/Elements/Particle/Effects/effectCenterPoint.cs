using Microsoft.Xna.Framework;

namespace TikiEngine.Elements.Particle
{
    public class effectCenterPoint : ParticleEffect
    {
        #region Init
        public effectCenterPoint()
        {
            this.Budget = 1;
            this.LifeTime = 0.1f;
            this.TriggerTime = 0f;
            this.ReleaseQuantity = 1;
            this.OffsetPosition = true;
            this.TextureFile = "Particle/dot_big";
        }
        #endregion

        #region Member
        protected override unsafe void CreateParticle(Particle* particle)
        {
            particle->Position = Vector2.Zero;
        }
        #endregion
    }
}
