using System;
using Microsoft.Xna.Framework;

namespace TikiEngine.Elements
{
    public interface IAttachable
    {
        float Angle { get; set; }
        float LayerDepth { get; set; }

        Vector2 Offset { get; set; }
        Vector2 CurrentPosition { get; set; }

        void Draw(GameTime gameTime);
        void Update(GameTime gameTime);
    }
}
