using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TikiEngine.Elements.Particle
{
    public class effectRunSparkOrange : ParticleEffect
    {
        #region Vars
        private modifierInterpolator _inter;
        #endregion

        #region Init
        public effectRunSparkOrange()
            : base(ParticleBatch.NonPremultipliedAdditive)
        {
            this.TriggerTime = 0.05f;
            this.ReleaseQuantity = 5;
            this.TextureFile = "particle/dot_glow";
            
            _inter = new modifierInterpolator()
            {
                ValueInit = 0,
                ValueMiddle = 1f,
                ValueFinal = 0,
                MiddlePosition = 0.5f
            };
        }
        #endregion

        #region Member
        protected override unsafe void CreateParticle(Particle* particle)
        {
            particle->Color = new Vector4(1f, 0.8f, 0f, 0f);
            particle->Scale = Functions.GetRandom(0.6f, 0.9f);
            particle->Velocity = Functions.AngleToVector(
                Functions.GetRandom(0f, MathHelper.Pi)
            ) * Functions.GetRandom(0.5f, 0.75f);
            particle->Velocity.Y -= 0.3f;
        }

        protected override unsafe void UpdateParticle(float elapsed, Particle* particle)
        {
            particle->Color.W = _inter.GetValue(particle->Age);

            base.UpdateParticle(elapsed, particle);
        }
        #endregion
    }
}
