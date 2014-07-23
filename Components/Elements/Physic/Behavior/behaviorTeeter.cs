using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework.Input;
using TikiEngine.Elements.Trigger;

namespace TikiEngine.Elements.Physic
{
    public class behaviorTeeter : Behavior
    {
        private FixedRevoluteJoint fixedRevoluteJoint;
        private Vector2 localAnchor = Vector2.Zero;
        private float friction;
        private long updateDelay = 0;
        private GameTrigger trigger;


        public behaviorTeeter(NameObjectPhysic nop)
            : base(nop)
        {
            this.trigger = new CollisionTrigger((PhysicTexture)nop);
        }
        public override void ApplyChanges()
        {
            if (this.fixedRevoluteJoint != null)
            {
                GI.World.RemoveJoint((this.fixedRevoluteJoint));
                this.fixedRevoluteJoint = null;
            }

            this.fixedRevoluteJoint = JointFactory.CreateFixedRevoluteJoint(GI.World, this.nop.Body, localAnchor, this.nop.Body.Position + localAnchor);

            this.fixedRevoluteJoint.MaxMotorTorque = 10000;
            this.fixedRevoluteJoint.MotorEnabled = true;

            this.nop.BodyType = BodyType.Dynamic;
            this.friction = this.nop.Body.Friction;

            if(LocalAnchor != Vector2.Zero)
                this.nop.Body.AngularDamping = 5f;

        }        

        public override void Update(GameTime gameTime)
        {
            this.trigger.Update(gameTime);

            float rot = MathHelper.ToDegrees((this.fixedRevoluteJoint.JointAngle));

            //if (Math.Abs(rot) < 60)
            //    this.nop.Body.Friction = this.friction * ((60 - Math.Abs(rot)) / 60);
            //else
            //    this.nop.Body.Friction = 0;

            if (!trigger.Triggered())
            {
                updateDelay += gameTime.ElapsedGameTime.Milliseconds;
                if (updateDelay > 1200)
                {
                    this.fixedRevoluteJoint.MotorEnabled = true;
                    this.fixedRevoluteJoint.MotorSpeed = MathHelper.ToRadians(rot);
                }
            }
            else
            {
                this.fixedRevoluteJoint.MotorEnabled = false;
                this.fixedRevoluteJoint.MotorSpeed = 0;
                updateDelay = 0;
            }
                
        }

#if DEBUG
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            Point pos = ConvertUnits.ToDisplayUnits(fixedRevoluteJoint.WorldAnchorA).ToPoint();
            Texture2D texture = GI.Content.Load<Texture2D>("Elements/circle");

            GI.SpriteBatch.Draw(
                texture,
                new Rectangle(pos.X, pos.Y, 10, 10),
                Color.White
            );        
        }
#endif

        public Vector2 LocalAnchor
        {
            get { return this.localAnchor; }
            set 
            { 
                this.localAnchor = value;
                ApplyChanges();
            }
        }

        public float Damping
        {
            get { return this.nop.Body.AngularDamping; }
            set { this.nop.Body.AngularDamping = value; }
        }
        public float AngleLimit
        {
            set 
            {
                this.fixedRevoluteJoint.UpperLimit = MathHelper.ToRadians(value);
                this.fixedRevoluteJoint.LowerLimit = MathHelper.ToRadians(-value);
                this.fixedRevoluteJoint.LimitEnabled = true;
            }
        }
    }
}
