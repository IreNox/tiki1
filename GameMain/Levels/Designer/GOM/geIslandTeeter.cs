#if DESIGNER
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace TikiEngine.Levels.Designer
{
    [Serializable]
    internal class geIslandTeeter : GameElementIsland
    {
        #region Vars
        private float _density = 1;
        private designStaticValue _densityCom;

        private float _damping;
        private designStaticValue _dampingCom;

        private Vector2 _localAnchor;
        private designConstructorCall _localAnchorCom;
        #endregion

        #region Init
        public geIslandTeeter()
        {
        }

        public geIslandTeeter(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Member
        protected override designMethodCall CreateMethodCall(DesignActionVar var)
        {
            _densityCom = new designStaticValue(typeof(float), _density);
            _dampingCom = new designStaticValue(typeof(float), _damping);

            _localAnchorCom = new designConstructorCall(
                typeof(Vector2).GetConstructor(
                    new Type[] { typeof(float), typeof(float) }
                )
            );
            _localAnchorCom.Args.Add(new designStaticValue(typeof(float), 0.0f));
            _localAnchorCom.Args.Add(new designStaticValue(typeof(float), 0.0f));

            var callCom = new designMethodCall(
                var,
                typeof(Setup).GetMethod(
                    "CreateTeeterIsland",
                    new Type[] { typeof(string), typeof(Vector2), typeof(Vector2), typeof(float), typeof(float) }
                )
            );
            callCom.Args.Add(textureCom);
            callCom.Args.Add(positionCom);
            callCom.Args.Add(_localAnchorCom);
            callCom.Args.Add(_dampingCom);
            callCom.Args.Add(_densityCom);

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

        public float Damping
        {
            get { return _damping; }
            set
            {
                _damping = value;
                _dampingCom.Value = value;
            }
        }

        public Vector2 LocalAnchor
        {
            get { return _localAnchor; }
            set
            {
                _localAnchor = value;
                ((designStaticValue)_localAnchorCom.Args[0]).Value = value.X;
                ((designStaticValue)_localAnchorCom.Args[1]).Value = value.Y;
            }
        }
        #endregion
    }
}
#endif