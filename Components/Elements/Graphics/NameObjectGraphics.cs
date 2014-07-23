using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.Serialization;

namespace TikiEngine.Elements.Graphics
{
    [Serializable]
    public abstract class NameObjectGraphics : NameObject
    {
        #region Vars
        protected float layerDepth = 0.5f;

        protected SpriteBatch spriteBatch = GI.SpriteBatch;
        private SpriteBatchType _spriteBatchType = SpriteBatchType.Default;
        #endregion

        #region Init
        public NameObjectGraphics()
            : this(SpriteBatchType.Default)
        {
        }

        public NameObjectGraphics(SpriteBatchType type)
        {
            this.SpriteBatchType = type;
        }

        public NameObjectGraphics(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { 
        }
        #endregion

        #region Properties
        public virtual SpriteBatchType SpriteBatchType
        {
            get { return _spriteBatchType; }
            set
            {
                _spriteBatchType = value;

                spriteBatch = value.GetSpriteBatch();
            }
        }

        public virtual float LayerDepth
        {
            get { return this.layerDepth; }
            set { this.layerDepth = value; }
        }
        #endregion
    }
}
