using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TikiEngine.Elements.Particle
{
    public class effectRunCloud : ParticleEffect
    {
        #region Vars
        private modifierInterpolator _inter;
        #endregion

        #region Init
        public effectRunCloud()
            : base(BlendState.NonPremultiplied)
        {
            this.TriggerTime = 0.05f;
            this.ReleaseQuantity = 2;
            this.TextureFile = "particle/cloud2";

            _inter = new modifierInterpolator()
            {
                ValueInit = 0,
                ValueMiddle = 0.75f,
                ValueFinal = 0,
                MiddlePosition = 0.3f
            };
        }
        #endregion

        #region Member
        protected override unsafe void CreateParticle(Particle* particle)
        {
            particle->Color = new Vector4(0.5f, 0.3f, 0, 1f);
            particle->Scale = Functions.GetRandom(0.33f, 0.66f);
            particle->Velocity = Functions.AngleToVector(
                Functions.GetRandom(0f, MathHelper.Pi)
            ) * Functions.GetRandom(0.3f, 0.5f);
        }

        protected override unsafe void UpdateParticle(float elapsed, Particle* particle)
        {
            particle->Color.W = _inter.GetValue(particle->Age);

            base.UpdateParticle(elapsed, particle);
        }
        #endregion
    }
}
