#if DESIGNER
using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using TikiEngine.Elements.Graphics;

namespace TikiEngine.Levels.Designer
{
    [Serializable]
    internal abstract class GameElement : NameObjectGraphics
    {
        #region Init
        public GameElement()
        { 
        }

        public GameElement(SerializationInfo info, StreamingContext context)
        {
            Serialization.ObjectDeserialize(this, info);
        }
        #endregion

        #region Member
        public abstract void Save();

        public abstract string GenerateCode();

        protected override void ApplyChanges()
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Properties
        public abstract Vector2 Size { get; }

        public Vector4 Rectangle
        {
            get
            { 
                return new Vector4(
                    positionCurrent - (this.Size / 2),
                    this.Size.X,
                    this.Size.Y
                );
            }
        }
        #endregion
    }
}
#endif