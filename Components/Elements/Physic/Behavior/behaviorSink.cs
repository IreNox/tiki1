using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using TikiEngine.Elements.Trigger;

namespace TikiEngine.Elements.Physic
{

    /*
     * Problematik:
     * Durch das Überschreiten eines Limits entsteht dein Lock was verhindert, dass die insel sich weiter bewegt
     */


    public class behaviorSink : Behavior
    {
        private FixedPrismaticJoint pris;
        private FixedDistanceJoint fdj;
        private Vector2 position;
        private float length;
        private bool reset = false;
        //private bool byWeight = false;
        private float damping = 8f;
        private long time = 1000;

        private GameTrigger trigger;

        public behaviorSink(NameObjectPhysic nop)
            : base(nop)
        {

        }
        public override void ApplyChanges()
        {
            this.nop.BodyType = FarseerPhysics.Dynamics.BodyType.Dynamic;
            this.position = this.nop.Body.Position;

            this.pris = JointFactory.CreateFixedPrismaticJoint(GI.World, this.nop.Body, this.nop.CurrentPosition, new Vector2(0, 1));
            this.fdj = JointFactory.CreateFixedDistanceJoint(GI.World, this.nop.Body, Vector2.Zero, position);
            this.fdj.Frequency = 2f;
            this.length = this.fdj.Length;

            //this.nop.Body.OnCollision += new OnCollisionEventHandler(Body_OnCollision);

            this.pris.UpperLimit = 15;
            this.pris.LowerLimit = 0f;
            this.pris.LimitEnabled = true;

            this.nop.Body.LinearDamping = 3f;

            this.Trigger = new CollisionTrigger((PhysicTexture)nop);
        }

        bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (this.fdj != null)
            {
                GI.World.RemoveJoint(fdj);
                this.fdj = null;
            }
            return true;
        }
        public override void Update(GameTime gameTime)
        {
            this.trigger.Update((gameTime));

            if (this.fdj != null)
            {
                if (Trigger.Triggered())
                {
                    GI.World.RemoveJoint(fdj);
                    this.fdj = null;
                }
            }
            else
            {
                time -= gameTime.ElapsedGameTime.Milliseconds;
                if (Trigger.Triggered())
                    time = 1000;
                if (this.reset)
                {
                    this.pris.UpperLimit--;
                    reset = false;
                }
                if (time < 0 && this.fdj == null)
                {
                    this.fdj = JointFactory.CreateFixedDistanceJoint(GI.World, this.nop.Body, Vector2.Zero, this.position);
                    this.fdj.Frequency = 2f;
                    this.fdj.Length = this.length;
                    this.fdj.DampingRatio = this.damping;
                    this.pris.UpperLimit++;
                    this.reset = true;
                }

            }
        }

        public float DampRising
        {
            get { return this.damping; }
            set { this.damping = value; }
        }
        public float DampFall
        {
            get { return this.nop.Body.LinearDamping; }
            set { this.nop.Body.LinearDamping = value; }
        }
        public float Limit
        {
            get { return this.pris.UpperLimit; }
            set { this.pris.UpperLimit = value; }
        }
        public GameTrigger Trigger
        {
            get { return this.trigger; }
            set { this.trigger = value; }
        }
    }
}
