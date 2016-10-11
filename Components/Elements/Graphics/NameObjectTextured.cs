using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TikiEngine.Elements.Graphics
{
    [Serializable]
    public abstract class NameObjectTextured : NameObjectGraphics
    {
        #region Vars
        private string _textureFile;
        private Vector2 _textureSize;

        protected Texture2D textureXna;
        #endregion

        #region Init
        public NameObjectTextured()
        { 
        }

        public NameObjectTextured(string assetName)
        {
            this.TextureFile = assetName;
        }

        public NameObjectTextured(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { 
        }
        #endregion

        #region Protected Member
        protected virtual void SetTexture(Texture2D texture)
        {
            if (textureXna != null)
            {
                _textureSize = new Vector2(texture.Width, texture.Height);
            }
        }
        #endregion

        #region Member
        public override void Dispose()
        {
        }
        #endregion

        #region Properties
        public string TextureFile
        {
            get { return _textureFile; }
            set
            {
                _textureFile = value;

                if (_textureFile != null)
                {
					try
					{
						this.Texture = GI.Content.Load<Texture2D>(value);
					}
					catch
					{
					}
                }
            }
        }

        [Browsable(false)]
        [NonSerializedTiki]
        public Texture2D Texture
        {
            get { return textureXna; }
            set
            {
                textureXna = value;
                this.SetTexture(value);
            }
        }

        [Browsable(false)]
        [NonSerializedTiki]
        public Vector2 TextureSize
        {
            get { return _textureSize; }
        }
        #endregion
    }
}
