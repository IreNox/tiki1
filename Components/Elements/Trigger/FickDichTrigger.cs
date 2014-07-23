using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TikiEngine.Elements.Physic;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics;

namespace TikiEngine.Elements.Trigger
{
    public class FickDichTrigger : GameTrigger
    {
        #region Vars
        private bool _active = true;
        private bool _triggered = false;

        private Body _body;
        #endregion

        #region Init
        public FickDichTrigger(Body body)
        {
            _body = body;
            _body.OnCollision += new FarseerPhysics.Dynamics.OnCollisionEventHandler(Body_OnCollision);
        }
        #endregion

        #region Member
        public override bool Triggered()
        {
            try
            {
                return _triggered;
            }
            finally
            {
                if (_triggered) _active = false;
                _triggered = false;
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
        }
        #endregion

        #region Member - EventHandler
        private bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            NameObjectPhysic nop = (NameObjectPhysic)(fixtureB.Body.UserData);

            if (nop.Material == Material.charakter && _active)
            {
                _triggered = _active;
            }

            return true;
        }
        #endregion
    }
}
