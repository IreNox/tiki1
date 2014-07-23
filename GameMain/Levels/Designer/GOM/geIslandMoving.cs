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
    internal class geIslandMoving : GameElementIsland
    {
        #region Vars
        private long _time;
        private designStaticValue _timeCom;

        private Vector2 _target;
        private designConstructorCall _targetCom;
        #endregion

        #region Init
        public geIslandMoving()
        {
        }

        public geIslandMoving(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Member
        protected override designMethodCall CreateMethodCall(DesignActionVar var)
        {
            _timeCom = new designStaticValue(typeof(long), _time);

            _targetCom = new designConstructorCall(
                typeof(Vector2).GetConstructor(
                    new Type[] { typeof(float), typeof(float) }
                )
            );
            _targetCom.Args.Add(new designStaticValue(typeof(float), 0.0f));
            _targetCom.Args.Add(new designStaticValue(typeof(float), 0.0f));

            var callCom = new designMethodCall(
                var,
                typeof(Setup).GetMethod(
                    "CreateMovingIsland",
                    new Type[] { typeof(string), typeof(Vector2), typeof(Vector2), typeof(long) }
                )
            );
            callCom.Args.Add(textureCom);
            callCom.Args.Add(positionCom);
            callCom.Args.Add(_targetCom);
            callCom.Args.Add(_timeCom);

            return callCom;
        }
        #endregion

        #region Properties
        public long Time
        {
            get { return _time; }
            set
            {
                _time = value;
                _timeCom.Value = value;
            }
        }

        public Vector2 Target
        {
            get { return _target; }
            set
            {
                _target = value;
                ((designStaticValue)_targetCom.Args[0]).Value = value.X;
                ((designStaticValue)_targetCom.Args[1]).Value = value.Y;
            }
        }
        #endregion
    }
}
#endif