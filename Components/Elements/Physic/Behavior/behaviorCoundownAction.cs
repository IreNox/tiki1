using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TikiEngine.Elements.Physic
{
    public class behaviorCoundownAction : Behavior
    {
        #region Vars
        private bool _started = false;

        private double _timer = 0.0;
        private double _timeCountdown = 10.0;

        private Action<NameObjectPhysic> _onDispose;
        #endregion

        #region Init
        public behaviorCoundownAction(NameObjectPhysic nop)
            : base(nop)
        { 
        }
        #endregion

        #region Member
        public void Start(double countdown, Action<NameObjectPhysic> onDispose)
        {
            _timer = 0;
            _started = true;
            _timeCountdown = countdown;

            _onDispose = onDispose;
        }

        public override void ApplyChanges()
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (_started)
            {
                _timer += gameTime.ElapsedGameTime.TotalSeconds;

                if (_timer > _timeCountdown)
                {
                    if (_onDispose != null) _onDispose(nop);

                    nop.Dispose();
                }
            }
        }
        #endregion
    }
}
