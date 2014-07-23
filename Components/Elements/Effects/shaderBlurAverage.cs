using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TikiEngine.Elements.Effects
{
    public class shaderBlurAverage : ShaderEffect
    {
        #region Vars
        private float _speed = 1f;
        private float _multiply = 1f;
        #endregion

        #region Init
        public shaderBlurAverage()
            : base("Effects/BlurAverage")
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

#if DEBUG
            GI.GameVars["test"] = _multiply;
#endif
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
        #endregion
    }
}
