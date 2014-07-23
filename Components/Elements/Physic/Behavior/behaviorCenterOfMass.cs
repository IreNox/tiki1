using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;

namespace TikiEngine.Elements.Physic
{
    public class behaviorCenterOfMass : Behavior
    {
        private Vector2 offset = Vector2.Zero;
        private PhysicCircle massPoint;
        private RevoluteJoint joint;

        public behaviorCenterOfMass(NameObjectPhysic nop)
            :base(nop)
        {

        }
        public override void ApplyChanges()
        {
            if (this.offset == Vector2.Zero)
                return;

            this.massPoint = new PhysicCircle(0.05f, 0.05f, "Elements/circle");
            this.massPoint.BodyType = BodyType.Dynamic;

            
            float mass = this.nop.Body.Mass;

            this.nop.Body.Mass = mass * 1 / 5;
            this.massPoint.Body.Mass = mass * 4 / 5;

            this.nop.Body.AngularDamping = 2f;

            this.massPoint.Body.Position = this.nop.Body.Position + this.offset;

            this.nop.Body.IgnoreCollisionWith(this.massPoint.Body);
            this.massPoint.Body.IgnoreCollisionWith(this.nop.Body);

            this.joint = JointFactory.CreateRevoluteJoint(GI.World, this.nop.Body, this.massPoint.Body, Vector2.Zero);



        }        
        public override void Update(GameTime gameTime)
        {

        }

#if DEBUG
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            this.massPoint.Draw(gameTime);
        }
#endif

        public Vector2 Offset
        {
            get { return this.offset; }
            set 
            { 
                this.offset = value;
                this.ApplyChanges();
            }
        }
    }
}
