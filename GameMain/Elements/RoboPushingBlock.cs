using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TikiEngine.Elements.Physic;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;

namespace TikiEngine.Elements
{

    internal partial class Robo
    {
        private PhysicBox currentBlockPushing;

        private float blockFriction = 0;//0.031f;
        private long pushTimer = 0;
        private bool pushing = false;
        private float blockDistanceToGround = 0;

        private float restPushpower = 1;

        public void UpdateBlockPushing(GameTime gameTime)
        {
            this.PushingSound();


            if (currentBlockPushing != null && !GI.World.BodyList.Contains(currentBlockPushing.Body))
                currentBlockPushing = null;

            if (currentBlockPushing == null)
            {
                foreach (PhysicBox pb in GI.Level.ElementsBuild)
                {
                    if (pb.Material == Physic.Material.block && pb.Bounds.Intersects(GI.RoboBounding) && currMaterial != Material.block)
                    {
                        currentBlockPushing = pb;
                        break;
                    }
                }
                pushTimer = 0;
                charakterAnimation.Pushing = pushing = false;
            } 
            else if (onGround && currMaterial != Physic.Material.block)
            {
                if (currentBlockPushing.Bounds.Intersects(GI.RoboBounding))
                {

                    pushTimer += gameTime.ElapsedGameTime.Milliseconds;
                    if (pushTimer > 50)
                    {

                        if(this.CurrentPosition.X < currentBlockPushing.CurrentPosition.X)
                        {
                            if (Right())
                            {
                                currentBlockPushing.Body.Friction = blockFriction;
                                charakterAnimation.Pushing = pushing = true;
                            }
                            else
                            {
                                pushTimer = 0;
                                if (circle.Body.Friction != 0.1f)
                                {
                                    if(this.GroundUnderBlock())
                                        currentBlockPushing.Body.LinearVelocity = new Vector2(0,currentBlockPushing.Body.LinearVelocity.Y);
                                    else
                                        currentBlockPushing.Body.LinearVelocity = new Vector2(
                                            (currentBlockPushing.Body.LinearVelocity.X > restPushpower ? restPushpower : currentBlockPushing.Body.LinearVelocity.X),
                                            currentBlockPushing.Body.LinearVelocity.Y);
                                }
                                charakterAnimation.Pushing = pushing = false;
                            }   
                        }
                        else
                        {
                            if (Left())
                            {
                                currentBlockPushing.Body.Friction = blockFriction;
                                charakterAnimation.Pushing = pushing = true;
                            }
                            else
                            {
                                pushTimer = 0;
                                if (circle.Body.Friction != 0.1f && this.GroundUnderBlock())
                                {
                                    if(this.GroundUnderBlock())
                                        currentBlockPushing.Body.LinearVelocity = new Vector2(0,currentBlockPushing.Body.LinearVelocity.Y);
                                    else
                                        currentBlockPushing.Body.LinearVelocity = new Vector2(
                                            (currentBlockPushing.Body.LinearVelocity.X < -restPushpower ? -restPushpower : currentBlockPushing.Body.LinearVelocity.X),
                                            currentBlockPushing.Body.LinearVelocity.Y);
                                }
                                charakterAnimation.Pushing = pushing = false;
                            }   
                        }       
                    }
                }
                else
                {
                    pushTimer = 0;
                    currentBlockPushing.Body.Friction = 3f;
                    currentBlockPushing = null;
                    charakterAnimation.Pushing = pushing = false;
                }
            }
            else
            {
                pushTimer = 0;
                currentBlockPushing.Body.Friction = 3f;
                currentBlockPushing = null;
                charakterAnimation.Pushing = pushing = false;
            }
        }
        public bool GroundUnderBlock()
        {
            this.blockDistanceToGround = -1;
            GI.World.RayCast(BlockRayCastCallback, currentBlockPushing.CurrentPosition, currentBlockPushing.CurrentPosition + new Vector2(0,2.2f));
            return this.blockDistanceToGround != -1;
        }
        public float BlockRayCastCallback(Fixture fixture, Vector2 point, Vector2 normal, float fraction)
        {
            NameObjectPhysic nop = (NameObjectPhysic)(fixture.Body.UserData);

            this.UpdateMaterial(nop.Material);

            if (GroundMaterials.Contains(nop.Material))
            {
                return this.blockDistanceToGround = 1;
            }
            return fraction;
        }

        public void PushingSound()
        {
            if (pushing)
                SoundPushingStart();
            else
                SoundPushingStop();
        }
    }
}
