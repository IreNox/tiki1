using Microsoft.Xna.Framework;

namespace TikiEngine.Elements.Particle
{
    public class systemBuildBreak : ParticleSystem
    {
        #region Init
        public systemBuildBreak()
        {
            this.AddEffect<effectBreakCube>();
            this.AddEffect<effectBreakRedDot>();
        }
        #endregion

        #region Member
        public override void Update(GameTime gameTime)
        {
            if (this.Effects[0].TotalTime > 1)
            {
                this.IsAlive = false;
            }

            base.Update(gameTime);
        }
        #endregion
    }
}
