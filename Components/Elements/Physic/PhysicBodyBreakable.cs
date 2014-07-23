using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TikiEngine.Elements.Physic
{
    [Serializable]
    public class PhysicBodyBreakable : PhysicTextureBreakable
    {
        #region Vars
        private List<Vertices> _listVertices;
        #endregion

        #region Init
        public PhysicBodyBreakable()
            : base()
        { 
        }

        public PhysicBodyBreakable(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Member - Protected
        protected override void SetTexture(Texture2D texture)
        {
            base.SetTexture(texture);
        }

        protected override List<Vertices> CreateVertices()
        {
            origin = ConvertUnits.ToSimUnits(this.TextureSize / 2);

            return _listVertices;
        }
        #endregion

        #region Properties
        public override bool Ready
        {
            get { return this.Texture != null && this.Effect != null && _listVertices != null; }
        }

        public List<Vertices> Vertices
        {
            get { return _listVertices; }
            set
            {
                _listVertices = value;
                this.RefreshBody = true;
                this.ApplyChanges();
            }
        }
        #endregion
    }
}
