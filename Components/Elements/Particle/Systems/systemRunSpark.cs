using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TikiEngine.Elements.Particle
{
    public class systemRunSpark : ParticleSystem
    {
        #region Init
        public systemRunSpark()
        {
            this.AddEffect<effectRunSparkRed>();
            //this.AddEffect<effectRunSparkOrange>();
        }
        #endregion

        #region Properties
        public float Speed
        {
            set
            {
                foreach (ParticleEffect e in this.Effects)
                {
                    e.ReleaseQuantity = (int)(2f * value);
                }
            }
        }
        #endregion
    }
}
