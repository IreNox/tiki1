using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TikiEngine.Elements.Particle
{
    public class effectCloud : ParticleEffect
    {
        #region Vars
        private float _width;
        private float _halfWidth;
        #endregion

        #region Init
        public effectCloud()
            : base(BlendState.NonPremultiplied)
        {
            this.Budget = 5000;
            this.LifeTime = 4f;
            this.TriggerTime = 0.05f;
            this.TextureFile = "Particle/cloud";
        }
        #endregion

        #region Member
        protected override unsafe void CreateParticle(Particle* particle)
        {
            particle->Color = new Vector4(
                0.9f,
                0.7f,
                0.3f,
                0.4f
            );

            particle->Temp = Functions.GetRandom(0.5f, 2f);
            particle->Velocity.Y = Functions.GetRandom(2f, 6f);
            particle->Position.X += Functions.GetRandom(-_halfWidth, _halfWidth);
        }

        protected override unsafe void UpdateParticle(float elapsed, Particle* particle)
        {
            if (particle->Velocity.Y > 2.5f) particle->Velocity.Y -= 1f * elapsed;
            particle->Scale = particle->Temp + (particle->Age * 2f);
            particle->Color.W = 0.8f * (1f - particle->Age);

            base.UpdateParticle(elapsed, particle);
        }
        #endregion

        #region Properties
        public float Width
        {
            get { return _width; }
            set
            {
                _width = value;
                _halfWidth = value / 2;
            }
        }
        #endregion
    }
}
