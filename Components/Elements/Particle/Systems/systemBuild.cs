using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TikiEngine.Elements.Particle
{
    public class systemBuild : ParticleSystem
    {
        #region Init
        public systemBuild()
        {
            this.AddEffect<effectBuildBackground>();
            this.AddEffect<effectAbsorb>().Size = 2;
            this.AddEffect<effectBuildCube>();
        }
        #endregion
    }
}
