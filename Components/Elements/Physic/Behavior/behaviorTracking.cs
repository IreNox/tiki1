using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;

namespace TikiEngine.Elements.Physic
{
    public class behaviorTracking : Behavior
    {
        #region Vars
        private Body _trackingBody;

        private bool _tracking = false;
        private float _trackingTime = 0;
        private float _trackingDistance = float.MaxValue;
        #endregion

        #region Init
        public behaviorTracking(NameObjectPhysic nop)
            : base(nop)
        {
        }
        #endregion

        #region Member
        public override void ApplyChanges()
        {
            nop.Body.Mass = 0f;
            nop.Body.BodyType = BodyType.Dynamic;
        }

        public override void Update(GameTime gameTime)
        {
            if (_trackingBody == null) return;

            if (!_tracking && (body.Position - _trackingBody.Position).LengthSquared() < _trackingDistance * _trackingDistance)
            {
                _tracking = true;
                nop.Body.IgnoreGravity = true;
            }

            if (_tracking)
            {
                _trackingTime += gameTime.ElapsedGameTime.Milliseconds;

                float speed = TikiConfig.MoveMaxSpeed + 2;
                float koef = _trackingTime / 2000;
                if (koef < 0.33) koef = 0.33f;

                speed *= koef;

                Vector2 tmp = _trackingBody.Position - nop.Body.Position;

                double d = tmp.Length();

                nop.Body.LinearVelocity = tmp * (float)((d - (d - speed)) / d);
            }
        }
        #endregion

        #region Properties
        public Body TrackingBody
        {
            get { return _trackingBody; }
            set { _trackingBody = value; }
        }

        public float TrackingDistance
        {
            get { return _trackingDistance; }
            set { _trackingDistance = value; }
        }
        #endregion
    }
}
