using System;

namespace TikiEngine.Elements.Particle
{
    public class systemRunDust : ParticleSystem
    {
        #region Init
        public systemRunDust()
        {
            this.AddEffect<effectRunCloud>();
        }
        #endregion

        #region Properties
        public float Speed
        {
            set
            {
                foreach (ParticleEffect e in this.Effects)
                {
                    e.ReleaseQuantity = (int)(0.25f * value);
                }
            }
        }
        #endregion
    }
}
