using Microsoft.Xna.Framework;
using TikiEngine.Elements.Graphics;

namespace TikiEngine
{
    public interface IAttachableCreator
    {
        AttachedElement CreateAttachableElement();

        Vector2 Offset { get; set; }
    }
}
