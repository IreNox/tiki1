using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TikiEngine.Elements.Effects
{
    public class shaderBlurDiagonal : ShaderEffect
    {
        #region Vars
        private float _speed = 1f;
        private float _multiply = 1f;
        #endregion

        #region Init
        public shaderBlurDiagonal()
            : base("Effects/BlurDiagonal")
        {
        }
        #endregion

        #region Member
        protected override void UpdateInternal(GameTime gameTime)
        {
            float multi = _multiply + (_speed * (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (multi > 0.2f)
            {
                multi = 0.2f;
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
            get { return (float)getValue("inSize"); }
            set
            {
                _multiply = value;
                setValue("inSize", value);
            }
        }
        #endregion
    }
}
