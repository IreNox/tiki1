using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TikiEngine.Elements.Effects
{
    public class shaderGlow : ShaderEffect
    {
        #region Vars
        private float _speed = 0.1f;
        private float _multiply = 1f;
        #endregion

        #region Init
        public shaderGlow()
            : base("Effects/Glow")
        {
        }
        #endregion

        #region Member
        protected override void UpdateInternal(GameTime gameTime)
        {
            float multi = _multiply + (_speed * (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (multi > 2)
            {
                multi = 2;
            }

            this.Multiply = multi;
        }
        #endregion

        #region Properties
        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public float Multiply
        {
            get { return (float)getValue("multiply"); }
            set
            {
                _multiply = value;
                setValue("multiply", value);
            }
        }

        public Vector4 Color
        {
            get { return (Vector4)getValue("glowColor"); }
            set { setValue("glowColor", value); }
        }
        #endregion
    }
}
