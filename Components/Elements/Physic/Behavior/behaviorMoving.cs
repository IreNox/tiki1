using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using TikiEngine.Elements.Trigger;

namespace TikiEngine.Elements.Physic
{
    public class behaviorMoving : Behavior
    {
        private Vector2 axis = new Vector2(1f,0f);
        private Vector2 distance;
        //private float distance = 5f;
        private Vector2 targetPos;

        private long timer = 0;
        private long duration = 10000;

        private GameTrigger trigger;

        private FixedPrismaticJoint pris;

        public behaviorMoving(NameObjectPhysic nop)
            : base(nop)
        {
            this.nop.StartPosition = this.nop.CurrentPosition;
            CollisionTrigger ct = new CollisionTrigger((PhysicTexture)nop);
            ct.OnCollision(true);
            ct.Unique = true;
            
            this.trigger = ct;
            
        }
        public override void ApplyChanges()
        {
            if (this.pris != null)
            {
                GI.World.RemoveJoint(this.pris);
                this.pris = null;
            }
            
            this.nop.BodyType = BodyType.Dynamic;
            this.pris = JointFactory.CreateFixedPrismaticJoint(GI.World, this.nop.Body, this.nop.StartPosition, axis);
            this.nop.Body.IgnoreGravity = true;

            this.nop.Body.Friction = float.MaxValue;

        }
        public override void Update(GameTime gameTime)
        {
            this.Trigger.Update(gameTime);

            if (Trigger.Triggered())
            {
                timer += gameTime.ElapsedGameTime.Milliseconds;
                timer %= duration;
                float koef = 360 * timer / duration;

                double sinus = Math.Sin(MathHelper.ToRadians(koef));

                this.nop.Body.LinearVelocity = Distance * (float)sinus;
            }
            else
                this.nop.Body.LinearVelocity = Vector2.Zero;

        }
        public void Reset()
        {
            nop.Body.LinearVelocity = Vector2.Zero;
            nop.CurrentPosition = nop.StartPosition;
            timer = 0;
            nop.BodyType = BodyType.Dynamic;
            this.trigger.Reset();
        }
        public GameTrigger Trigger
        {
            get { return this.trigger; }
            set { this.trigger = value; }
        }

        public Vector2 Distance
        {
            get { return this.distance; }
            set { this.distance = value; }
        }
        public long Time
        {
            get { return this.duration; }
            set { this.duration = value; }
        }
        public Vector2 TargetPos
        {
            get { return this.targetPos; }
            set 
            {
                this.targetPos = value;
                this.axis = targetPos - this.nop.CurrentPosition;
                this.Distance = this.axis * 0.873f;
                while (true)
                {
                    if (axis.Length() > 1f)
                        axis /= 2;
                    else
                        break;
                }
                ApplyChanges();
            }
        }
    }
}
