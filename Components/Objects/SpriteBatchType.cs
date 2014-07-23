using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace TikiEngine
{
    public enum SpriteBatchType
    {
        Default,
        Parallax,
        Interface
    }

    public static class SpriteBatchTypeExtensions
    {
        public static SpriteBatch GetSpriteBatch(this SpriteBatchType type)
        {
            switch (type)
            { 
                case SpriteBatchType.Parallax:
                    return GI.SpriteBatchParallax;
                case SpriteBatchType.Interface:
                    return GI.SpriteBatchInterface;
            }

            return GI.SpriteBatch;
        }
    }
}
