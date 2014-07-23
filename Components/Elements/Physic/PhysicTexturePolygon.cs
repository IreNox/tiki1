using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace TikiEngine.Elements.Physic
{
    [Serializable]
    public class PhysicTexturePolygon : NameObjectPhysicPolygon
    {
        #region Init
        public PhysicTexturePolygon()
            : base()
        { 
        }

        public PhysicTexturePolygon(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Member - Protected
        protected override Body CreateBodyByList(List<Vertices> list)
        {
            Body body2 = BodyFactory.CreateCompoundPolygon(world, list, this.Density, BodyType.Dynamic);
            body2.Position = this.CurrentPosition;
            body2.BodyType = BodyType.Dynamic;

            return body2;           
        }
        #endregion

        #region Properties
        public override bool Ready
        {
            get { return this.Texture != null; }
        }
        #endregion
    }
}
