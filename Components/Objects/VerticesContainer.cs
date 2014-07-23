using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using TikiEngine.Elements;

namespace TikiEngine
{
    [Serializable]
    public class VerticesContainer : NameObject
    {
        #region Init
        public VerticesContainer(List<Vertices> vertices, Vector2 origin)
        {
            this.Origin = origin;
            this.Vertices = vertices;
        }

        public VerticesContainer(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { 
        }
        #endregion

        #region Properties
        public Vector2 Origin { get; set; }
        public List<Vertices> Vertices { get; set; }
        #endregion

        #region NameObject
        protected override void ApplyChanges()
        {
        }

        public override void Draw(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Dispose()
        {
        }

        public override bool Ready
        {
            get { return true; }
        }
        #endregion
    }
}
