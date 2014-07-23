using System;
using System.Collections.Generic;

namespace TikiEngine.Elements.Particle
{
    public class systemHolo : ParticleSystem
    {
        #region Init
        public systemHolo()
        {
            this.AddEffect<effectBeam>().LayerDepth = 0.12f;
            this.AddEffect<effectMoveUp>().LayerDepth = 0.11f;
            this.AddEffect<effectLighting>().LayerDepth = 0.10f;
        }
        #endregion
    }
}
