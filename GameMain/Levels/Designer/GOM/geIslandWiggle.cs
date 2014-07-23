#if DESIGNER
using System;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization;

namespace TikiEngine.Levels.Designer
{
    [Serializable]
    internal class geIslandWiggle : GameElementIsland
    {
        #region Vars
        private float _density = 1f;

        private designStaticValue _densityCom;
        #endregion

        #region Init
        public geIslandWiggle()
        {
        }

        public geIslandWiggle(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Member
        protected override designMethodCall CreateMethodCall(DesignActionVar var)
        {
            var info = typeof(Setup).GetMethod(
                "CreateWiggleIsland",
                new Type[] { typeof(string), typeof(Vector2), typeof(float), typeof(float) }
            );

            _densityCom = new designStaticValue(typeof(float), _density);
            var callCom = new designMethodCall(var, info);
            callCom.Args.Add(textureCom);
            callCom.Args.Add(positionCom);
            callCom.Args.Add(_densityCom);
            callCom.Args.Add(rotationCom);

            return callCom;
        }
        #endregion

        #region Properties
        public float Density
        {
            get { return _density; }
            set
            {
                _density = value;
                _densityCom.Value = value;
            }
        }
        #endregion
    }
}
#endif