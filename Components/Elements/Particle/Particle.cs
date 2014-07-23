using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TikiEngine.Elements.Particle
{
    public struct Particle
    {
        #region Vars
        public Vector2 Velocity;
        public Vector2 Position;
        public Vector2 PositionStart;

        public float Scale;
        public float Roation;
        public Vector4 Color;

        public float Age;
        public float BirthTime;

        public float Temp;
        #endregion
    }
}
