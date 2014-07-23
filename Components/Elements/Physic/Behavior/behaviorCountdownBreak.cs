using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using TikiEngine.Elements.Particle;

namespace TikiEngine.Elements.Physic
{
    public class behaviorCountdownBreak : Behavior
    {
        #region Vars
        private float _countDown = 2000;
        private bool _countDownStart = false;
        private double _startTime = 0;

        private systemDust _dust;

        private PhysicTextureBreakable _breakable;
        #endregion

        #region Init
        public behaviorCountdownBreak(PhysicTextureBreakable nop)
            : base(nop)
        {
            _breakable = nop;

            _dust = new systemDust();
            _dust.IsAlive = false;
            _dust.Width = _breakable.Size.X * 0.9f;
            GI.Level.Particles.Add(_dust);
        }
        #endregion

        #region Member
        public override void ApplyChanges()
        {
            _startTime = 0;
            _countDownStart = false;

            _breakable.Body.OnCollision += new FarseerPhysics.Dynamics.OnCollisionEventHandler(Body_OnCollision);
        }

        public override void Update(GameTime gameTime)
        {
            if (_countDownStart && _startTime == 0)
            {
                _startTime = gameTime.TotalGameTime.TotalMilliseconds;
            }

            if (_countDownStart && (gameTime.TotalGameTime.TotalMilliseconds - _startTime) >= _countDown && !_breakable.BreakableBody.Broken)
            {
                _breakable.BreakableBody.Break();
                GI.Sound.PlaySFX(TikiSound.Island_Break_On_Touch,1f);
            }
        }
        #endregion

        #region Member - EventHandler
        private bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
         {
            _countDownStart = true;

            _dust.Force = 4;
            _dust.IsAlive = true;
            _dust.CurrentPosition = new Vector2(
                _breakable.CurrentPosition.X,
                _breakable.CurrentPosition.Y - (_breakable.Size.Y * 0.3f)
            );

            return true;
        }
        #endregion

        #region Properties
        public float CountDown
        {
            get { return _countDown; }
            set { _countDown = value; }
        }
        #endregion
    }
}
