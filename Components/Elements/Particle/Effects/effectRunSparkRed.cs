using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TikiEngine.Elements.Particle
{
    public class effectRunSparkRed : ParticleEffect
    {
        #region Vars
        private modifierInterpolator _inter;
        #endregion

        #region Init
        public effectRunSparkRed()
            : base(BlendState.AlphaBlend)
        {
            this.TriggerTime = 0.05f;
            this.ReleaseQuantity = 5;
            this.TextureFile = "particle/spark";
            
            _inter = new modifierInterpolator()
            {
                ValueInit = 0.0f,
                ValueMiddle = 1f,
                ValueFinal = 0,
                MiddlePosition = 0.3f
            };
        }
        #endregion

        #region Member
        protected override unsafe void CreateParticle(Particle* particle)
        {
            particle->Color = new Vector4(1f, 1f, 0f, 0f);
            particle->Scale = Functions.GetRandom(0.5f, 0.9f);
            particle->Position += new Vector2(
                Functions.GetRandom(-0.3f, 0.3f),
                0.2f
            );
            particle->Velocity = Functions.AngleToVector(
                Functions.GetRandom(0f, MathHelper.Pi)
            ) * Functions.GetRandom(0.3f, 0.5f);
            //particle->Velocity.X += Functions.GetRandom(0.0f, 1f);
            particle->Velocity.Y -= Functions.GetRandom(0.4f, 0.6f);
        }

        protected override unsafe void UpdateParticle(float elapsed, Particle* particle)
        {
            particle->Color.Y = 1 - (particle->Age * 0.3f);
            particle->Color.W = _inter.GetValue(particle->Age);

            base.UpdateParticle(elapsed, particle);
        }
        #endregion
    }
}
