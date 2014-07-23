using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TikiEngine.Elements.Effects
{
    public class shaderBasic : ShaderEffect
    {
        #region Init
        public shaderBasic()
            : base(new BasicEffect(GI.Device))
        {
            this.SetTexture(null);
        }
        #endregion

        #region Member
        protected override void SetTexture(Texture2D texture)
        {
            base.SetTexture(texture);

            BasicEffect effect = (BasicEffect)this.EffectXna;

            effect.TextureEnabled = (texture != null);
            effect.VertexColorEnabled = !effect.TextureEnabled;

            if (effect.TextureEnabled)
            {
                effect.Texture = texture;
            }            
        }
        #endregion
    }
}
