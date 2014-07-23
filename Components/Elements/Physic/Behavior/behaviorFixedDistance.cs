using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;

namespace TikiEngine.Elements.Physic
{
    public class behaviorFixedDistance : Behavior
    {
        private FixedDistanceJoint fdj;
        private Vector2 local = Vector2.Zero;

        public behaviorFixedDistance(NameObjectPhysic nop)
            : base(nop)
        {
            //nop.Body.OnCollision += new FarseerPhysics.Dynamics.OnCollisionEventHandler(Body_OnCollision);
            //nop.Body.OnSeparation += new OnSeparationEventHandler(Body_OnSeparation);
        }





        bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            NameObjectPhysic nop2 = (NameObjectPhysic)fixtureB.Body.UserData;
            if (nop2.Material == Material.charakter)
            {
                nop.BodyType = BodyType.Dynamic;
            } 
            return true;
        }
        public void AddEventListener()
        {
            nop.Body.OnCollision += new FarseerPhysics.Dynamics.OnCollisionEventHandler(Body_OnCollision);
            //nop.Body.OnSeparation += new OnSeparationEventHandler(Body_OnSeparation);
        }

        public override void ApplyChanges()
        {

            if (fdj != null)
            {
                GI.World.RemoveJoint(fdj);
                fdj = null;
            }
            fdj = JointFactory.CreateFixedDistanceJoint(GI.World, this.nop.Body, Vector2.Zero, this.nop.Body.Position + local);
            fdj.Frequency = 100f;

            nop.BodyType = BodyType.Static;
            nop.Body.FixedRotation = true;
            nop.Body.Restitution = 0;
            nop.Body.LinearDamping = 3f;

        }

        public override void Update(GameTime gameTime)
        {
            if (nop.BodyType == BodyType.Dynamic && nop.Body.LinearDamping > 0.1f)
                nop.Body.LinearDamping -= 0.1f; 
        }
#if DEBUG
        public override void Draw(GameTime gameTime)
        {

            if (!GI.DEBUG)
                return;

            Texture2D texture = GI.Content.Load<Texture2D>("background");

            GI.SpriteBatch.Draw(texture,
                new Rectangle(
                    (int)ConvertUnits.ToDisplayUnits(fdj.WorldAnchorA.X),
                    (int)ConvertUnits.ToDisplayUnits(fdj.WorldAnchorA.Y),
                    10,
                    10),
                    null,
                    Color.White,
                    0f,
                    new Vector2(5),
                    SpriteEffects.None,
                    (float)LayerDepthEnum.Debug);
            GI.SpriteBatch.Draw(texture,
                new Rectangle(
                    (int)ConvertUnits.ToDisplayUnits(fdj.WorldAnchorB.X),
                    (int)ConvertUnits.ToDisplayUnits(fdj.WorldAnchorB.Y),
                    10,
                    10),
                    null,
                    Color.Red,
                    0f,
                    new Vector2(5),
                    SpriteEffects.None,
                    (float)LayerDepthEnum.Debug);



        }
#endif
        public Vector2 Distance
        {
            get { return this.local; }
            set
            {
                this.local = value;
                ApplyChanges();
            }
        }
    }
}
