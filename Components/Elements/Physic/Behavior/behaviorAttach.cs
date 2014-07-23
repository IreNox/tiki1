using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;

namespace TikiEngine.Elements.Physic
{
    public class behaviorAttach : Behavior
    {
        #region Vars
        private Body _bodyB;
        private Vector2 localAnchor = Vector2.Zero;
        
        private RevoluteJoint _joint;
        #endregion

        #region Init
        public behaviorAttach(NameObjectPhysic nop)
            : base(nop)
        { 
        }
        #endregion

        #region Member
        public override void ApplyChanges()
        {
            if (_joint != null)
            {
                GI.World.JointList.Remove(_joint);
                _joint = null;
            }

            if (!this.Ready) return;

            _joint = JointFactory.CreateRevoluteJoint(
                nop.Body,
                _bodyB,
                localAnchor
            );
            Console.WriteLine("Attach");
        }

        public override void Update(GameTime gameTime)
        {
        }
        #endregion

        #region Properties
        public Vector2 LocalAnchor
        {
            get { return this.localAnchor; }
            set 
            {
            
                this.localAnchor = value; 
                this.ApplyChanges(); 
            }
        }
        public Body BodyB
        {
            get { return _bodyB; }
            set
            {
                _bodyB = value;
                value.IgnoreCollisionWith(this.nop.Body);
                this.nop.Body.IgnoreCollisionWith(value);
                this.ApplyChanges();
            }
        }

        public bool Ready
        {
            get { return _bodyB != null; }
        }
        #endregion
    }
}
