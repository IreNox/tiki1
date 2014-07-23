using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Dynamics.Contacts;
using TikiEngine.Components;
using TikiEngine.Elements.Physic;
using TikiEngine.Elements.Graphics;
using TikiEngine.Elements.Particle;

namespace TikiEngine.Elements
{
    internal partial class Robo : PhysicBox
    {
        #region Vars - Control
        private bool _allowMove = true;

        private Vector2 jumpForce;

        public static Vector2 size = new Vector2(0.9f, 2.4f);//5//2.4
        public const float mass = 20f;

        private float MAXSPEEDX = 10;

        private float runSpeed = 14 / size.X;
        private float sidewalkspeed = 4.5f;
        private float jumpImpulse = -18;

        private bool onGround = false;
        private bool jump = false;

        private float rayPixels = 0.2f;
        private float distanceToGround = 0;

        private Material prevMaterial = Material.island;
        private Material currMaterial;

        private Fixture fixtureCollision;
        #endregion

        #region Vars - Physic Elements
        private PhysicCircle circle;

        private FixedAngleJoint fixedAngleJoint;
        private RevoluteJoint motor;

        private RotatedRectangle rotatedRectangle;

        private float friction = 10;

        private List<Material> GroundMaterials = new List<Material>() { 
            Material.island,
            Material.block,
            Material.metal,
            Material.slide,
            Material.wood,
        };
        #endregion
      
        #region Init
        public Robo()
            : base(new Vector2(size.X+0.1f, size.Y - size.X / 2), Vector2.Zero, mass / 2, "Elements/green")
        {
            float width = Robo.size.X;
            float height = Robo.size.Y;

            this.rotatedRectangle = GI.RoboBounding;// new RotatedRectangle(new Rectangle(0, 0, (int)ConvertUnits.ToDisplayUnits(width), (int)ConvertUnits.ToDisplayUnits(height)));

            body.Friction = 0f;
            body.Restitution = 0f;
            body.BodyType = BodyType.Dynamic;
            body.Position = new Vector2(0f, -2f);
            body.UserData = this;
            this.Material = Material.charakter;

            fixedAngleJoint = JointFactory.CreateFixedAngleJoint(world, body);

            circle = new PhysicCircle(width / 2, mass / 2, "Elements/circle");

            circle.Body.Position = body.Position + new Vector2(0f, (height - width / 2) / 2);
            circle.Body.BodyType = BodyType.Dynamic;
            circle.Body.Restitution = 0f;
            circle.Body.UserData = circle;

            circle.Material = Material.charakter;


            motor = JointFactory.CreateRevoluteJoint(world, this.body, circle.Body, Vector2.Zero);
            motor.MotorEnabled = true;
            motor.MaxMotorTorque = 2000f;
            motor.MotorSpeed = 0;

            body.IgnoreCollisionWith(circle.Body);
            circle.Body.Friction = this.friction;
            circle.Body.IgnoreCollisionWith(body);
            circle.Body.OnCollision += new OnCollisionEventHandler(Circle_OnCollision);
            circle.Body.OnSeparation += new OnSeparationEventHandler(Circle_OnSeparation);
            
            _initBuild();
            _initDestroy();
            _initEffects();
            _initAnimation();
        }    
        #endregion

        #region Member
        public void IgnoreCollisionWith(Body b)
        {
            body.IgnoreCollisionWith(b);
            circle.Body.IgnoreCollisionWith(b);
        }
        #endregion

        #region Member - Draw
        public override void Draw(GameTime gameTime)
        {
#if DEBUG
            //base.Draw(gameTime);
            //circle.Draw(gameTime);

            GI.RoboBounding.Draw(gameTime);
#endif
            // MEGA DILDO PENIS ACTION

            _drawBuild(gameTime);
            _drawDestroy(gameTime);
            _drawEffects(gameTime);
            _drawAnimation(gameTime);
        }
        #endregion
 
        #region Member - Update
        public override void Update(GameTime gameTime)
        {

            UpdateRectangle();

            bool b = false;

            foreach (NameObject no in GI.Level.Elements)
            {
                if(no is PhysicTexture && this.rotatedRectangle.Intersects(((PhysicTexture)no).RotatedRectangle))
                    b = true;
            }

            this.rotatedRectangle.Color = b ? Color.Red : Color.White;

            this.UpdateDistance(gameTime);
            this.UpdateFalling(gameTime);
            this.UpdateJumping(gameTime);
            this.UpdateRunning(gameTime);

            this.UpdateBlockPushing(gameTime);
            this.UpdateSound(gameTime);
            
            _updateBuild(gameTime);
            _updateDestroy(gameTime);
            _updateEffects(gameTime);
            _updateAnimation(gameTime);
        }

        #region Controls
        public bool Left()
        {
            if (!_allowMove) return false;

            return (GI.Control.KeyboardDown(Keys.A) && GI.Control.KeyboardUp(Keys.D))
                || (GI.Control.KeyboardDown(Keys.Left) && GI.Control.KeyboardUp(Keys.Right));
        }

        public bool Right()
        {
            if (!_allowMove) return false;

            return (GI.Control.KeyboardDown(Keys.D) && GI.Control.KeyboardUp(Keys.A))
                || (GI.Control.KeyboardDown(Keys.Right) && GI.Control.KeyboardUp(Keys.Left));
        }

        public bool Jump()
        {
            if (!_allowMove) return false;

            return GI.Control.KeyboardPressed(Keys.Space) || GI.Control.KeyboardPressed(Keys.Up);
        }

        public bool Slide()
        {
            return (this.circle.Body.Friction == 0.1f);
        }
        #endregion

        public void UpdateDistance(GameTime gameTime)
        {
            float mid = Robo.size.X / 2;

            this.distanceToGround = -1;

            Vector2 start = circle.Body.Position;
            Vector2 stop = circle.Body.Position + new Vector2(0, mid + this.rayPixels );

            GI.World.RayCast(RayCastCallback, start, stop);
            GI.World.RayCast(RayCastCallback, start + new Vector2(mid,0), stop + new Vector2(mid,0));
            GI.World.RayCast(RayCastCallback, start + new Vector2(-mid,0), stop + new Vector2(-mid,0));

            this.onGround = distanceToGround != -1;
        }

        public float RayCastCallback(Fixture fixture, Vector2 point, Vector2 normal, float fraction)
        {
            NameObjectPhysic nop = (NameObjectPhysic)(fixture.Body.UserData);

            this.UpdateMaterial(nop.Material);


            if (GroundMaterials.Contains(nop.Material))
            {
                if (nop.Material == Material.slide)
                    circle.Body.Friction = 0.1f;
                else
                    circle.Body.Friction = this.friction;

                return this.distanceToGround = 1;
            }
            return fraction;
        }

        public void UpdateFalling(GameTime gameTime)
        {
            if (!jump && !onGround)
            {
                this.charakterAnimation.Fall(gameTime);
                SoundFall();
            }   
        }

        private void UpdateJumping(GameTime gameTime)
        {
            if (Jump() && this.onGround)
            {
                motor.MotorSpeed = 0;
                jumpForce.Y = jumpImpulse;
                body.ApplyLinearImpulse(jumpForce, body.Position);
                jump = true;
                SoundJump();
            }
        }

        public void UpdateRunning(GameTime gameTime)
        {
            if(Right())
            {
                motor.MotorSpeed = runSpeed;

                if (!onGround && circle.Body.Friction == this.friction)
                {
                    if (body.LinearVelocity.X < sidewalkspeed)
                        body.LinearVelocity += new Vector2(1f, 0);
                }
                this.charakterAnimation.Orientation = true;
            }
            else if (Left())
            {
                motor.MotorSpeed = -(runSpeed);

                if (!onGround && circle.Body.Friction == this.friction)
                {
                    if (body.LinearVelocity.X > -sidewalkspeed)
                        body.LinearVelocity += new Vector2(-1f, 0);
                }

                this.charakterAnimation.Orientation = false;
            }
            else
            {
                motor.MotorSpeed = 0;
            }   
        }

        public void UpdateRectangle()
        {
            this.rotatedRectangle.ChangePosition(ConvertUnits.ToDisplayUnits(this.CurrentPosition - Robo.size / 2 + new Vector2(-0.35f, Robo.size.X / 4)));
            this.rotatedRectangle.Rotation = this.Body.Rotation;
        }

        public void UpdateMaterial(Material m)
        {
            prevMaterial = currMaterial;
            currMaterial = m;

            this.UpdateMaterialSound();
        }
        public void ClearVelocity()
        {
            circle.Body.ResetDynamics();
            Body.ResetDynamics();
        }
        #endregion

        #region Member - EventHandler
        bool Circle_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            NameObjectPhysic nop = (NameObjectPhysic)(fixtureB.Body.UserData);
            if (GroundMaterials.Contains(nop.Material))
            {
                this.UpdateMaterial(nop.Material);
                this.charakterAnimation.GroundCollision();
                this.GroundCollisionSound(fixtureB);
                fixtureCollision = fixtureB;
                jump = false;
            }
            return true;
        }

        private void Circle_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            #region new
            NameObjectPhysic nop = (NameObjectPhysic)(fixtureB.Body.UserData);

            if (GroundMaterials.Contains(nop.Material))
            {

                circle.Body.LinearVelocity -= new Vector2(fixtureB.Body.LinearVelocity.X, 0);

                if (circle.Body.LinearVelocity.X < -MAXSPEEDX)
                {
                    circle.Body.LinearVelocity = new Vector2(-MAXSPEEDX, circle.Body.LinearVelocity.Y);
                }
                else if (circle.Body.LinearVelocity.X > MAXSPEEDX)
                {
                    body.LinearVelocity = new Vector2(MAXSPEEDX, circle.Body.LinearVelocity.Y);
                } 
            }
            #endregion
        }
        #endregion

        #region  Properties
        public override Vector2 StartPosition
        {
            get { return base.StartPosition; }
            set { base.StartPosition = value; }
        }

        public override Vector2 CurrentPosition
        {
            get { return body.Position; }
            set
            {
                Vector2 tmp = value - body.Position;
                body.Position += tmp;
                circle.CurrentPosition += tmp;
            }
        }

        public Vector2 Origin
        {
            get { return this.CurrentPosition + origin; }
        }

        public bool AllowMove
        {
            get { return _allowMove; }
            set { _allowMove = value; }
        }
        #endregion
    }
}
