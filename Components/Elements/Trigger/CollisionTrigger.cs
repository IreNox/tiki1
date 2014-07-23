using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using TikiEngine.Elements.Physic;

namespace TikiEngine.Elements.Trigger
{
    public class CollisionTrigger : GameTrigger
    {
        private bool triggered = false;
        private bool charakterCollision = true;
        private bool cubeCollision = true;
        private bool OnCollisionOnly = false;

        private PhysicTexture pt;

        public CollisionTrigger(PhysicTexture pt)
        {
            this.pt = pt;
        }

        public CollisionTrigger(PhysicTexture pt, bool charCol, bool cubeCol, bool onlyCol)
            : this(pt)
        {
            charakterCollision = charCol;
            cubeCollision = cubeCol;
            OnCollisionOnly = onlyCol;

            this.OnCollision(onlyCol);
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (Unique && triggered)
                return true;

            NameObjectPhysic nop = (NameObjectPhysic)(fixtureB.Body.UserData);

            if (CharakterCollision)
            {
                if (nop.Material == Material.charakter)
                {
                    triggered = true;
                }
            }
            if (CubeCollsion)
            {
                if (nop.Material == Material.block)
                {
                    triggered = true;
                }
            }
            return true;
        }


        public override bool Triggered()
        {
            return triggered;
        }

        public override void Update(GameTime gameTime)
        {
            if (OnCollisionOnly || (Unique && triggered))
            {
                return;
            }

            triggered = false;
            if (BoxCollision())
                triggered = true;
            if (RoboCollision())
                triggered = true;
        }

        public override void Reset()
        {
            base.Reset();
            if (OnCollisionOnly)
                triggered = false;
        }
        public bool BoxCollision()
        {
            foreach (PhysicBox pb in GI.Level.ElementsBuild.OfType<PhysicBox>())
            {
                if (pb.Bounds.Intersects(pt.RotatedRectangle))
                    return true;
            }
            return false;
        }
        public bool RoboCollision()
        {
            return GI.RoboBounding.Intersects(pt.RotatedRectangle);

        }
        public CollisionTrigger OnCollision(bool b)
        {
            if (b)
            {
                pt.Body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
                this.OnCollisionOnly = true;
            }
            else
            {
                pt.Body.OnCollision -= new OnCollisionEventHandler(body_OnCollision);
            }

            return this;
        }


        public bool CharakterCollision
        {
            get { return this.charakterCollision; }
            set { this.charakterCollision = value; }
        }
        public bool CubeCollsion
        {
            get { return this.cubeCollision; }
            set { this.cubeCollision = value; }
        }
    }
}
