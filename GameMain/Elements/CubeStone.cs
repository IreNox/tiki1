using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TikiEngine.Elements.Physic;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace TikiEngine.Elements
{
    internal class CubeStone : Cube
    {
        #region Init
        public CubeStone(Vector2 pos)
            : base(pos, TikiConfig.StoneCube, 15, "Elements/cube_stone")
        {
            destroy = false;

            body.SleepingAllowed = false;
        }
        #endregion
    }
}
