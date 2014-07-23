using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using TikiEngine.Elements.Graphics;
using FarseerPhysics.Dynamics.Joints;

namespace TikiEngine.Elements.Physic
{
    public class NewtonIsland : NameObject
    {
        private PhysicTexture ps1;
        private PhysicTexture ps2;

        private Vector2 startpos;
        private Vector2 linearVelocity = Vector2.Zero;

        private float distance;
        private float swingDistance = 8f;

        private bool firstTime = true;

        private PhysicPath path1;
        private PhysicPath path2;
        private PhysicPath path3;
        private PhysicPath path4;

        private const float SHORTN = 15;

        public NewtonIsland(Vector2 pos, float distance, float swingDistance)
        {


            this.startpos = pos;
            this.distance = distance;
            this.swingDistance = swingDistance;

            ps1 = new PhysicTexture();

            ps1.TextureFile = "Islands/movingPlatform_n";
            ps1.TextureBodyFile = "Islands/movingPlatform_c";
            ps1.Density = 50;
            ps1.StartPosition = pos;
            ps1.CurrentPosition -= new Vector2(swingDistance, 0);
            ps1.LayerDepth = LayerDepthEnum.Island;

            ps1.Body.FixedRotation = true;


            ps1.AddBehavior<behaviorFixedDistance>().Distance = new Vector2(swingDistance, 0f);
            ps1.Body.LinearDamping = 5f;
            ps1.Body.Friction = float.MaxValue;

            ps1.Body.OnCollision += new OnCollisionEventHandler(Body_OnCollision);


            ps2 = new PhysicTexture();

            ps2.TextureFile = "Islands/movingPlatform_n";
            ps2.TextureBodyFile = "Islands/movingPlatform_c";
            ps2.Density = 50;
            ps2.StartPosition = pos + new Vector2(distance, 0);
            ps2.CurrentPosition += new Vector2(0, swingDistance);

            ps2.LayerDepth = LayerDepthEnum.Island;

            ps2.Body.FixedRotation = true;

            ps2.AddBehavior<behaviorFixedDistance>().Distance = new Vector2(0, -swingDistance);
            ps2.Body.LinearDamping = 0f;
            ps2.Body.Friction = float.MaxValue;

            path1 = new PhysicPath(
                startpos + new Vector2(distance -4,0),
                startpos + new Vector2(distance - 4, swingDistance),
                new Vector2(0.6f, 0.29f),
                (int)(swingDistance / 0.6f),
                "Elements/rope_2",
                40,
                true,
                false
            );

            path1.CollisionCategories = Category.None;

            GI.World.AddJoint(
                new RevoluteJoint(ps2.Body, path1.LastBody, new Vector2(-4, -0.25f), new Vector2(0, 0.9f))
            );

            path2 = new PhysicPath(
                startpos + new Vector2(distance + 4, 0),
                startpos + new Vector2(distance + 4, swingDistance),
                new Vector2(0.6f, 0.29f),
                (int)(swingDistance / 0.6f),
                "Elements/rope_2",
                40,
                true,
                false
            );

            path2.CollisionCategories = Category.None;

            GI.World.AddJoint(
                new RevoluteJoint(ps2.Body, path2.LastBody, new Vector2(4, -0.25f), new Vector2(0, 0.9f))
            );

            path3 = new PhysicPath(
                startpos + new Vector2(4, 0),
                startpos + new Vector2(0, swingDistance) + new Vector2(4, 0),
                new Vector2(0.6f, 0.29f),
                (int)(swingDistance / 0.6f),
                "Elements/rope_2",
                40,
                true,
                false
            );

            path3.CollisionCategories = Category.None;

            GI.World.AddJoint(
                new RevoluteJoint(ps1.Body, path3.LastBody, new Vector2(4, -0.25f), new Vector2(0, 0.9f))
            );

            path4 = new PhysicPath(
                startpos + new Vector2(-4, 0),
                startpos + new Vector2(0, swingDistance) + new Vector2(-4, 0),
                new Vector2(0.6f, 0.29f),
                (int)(swingDistance / 0.6f),
                "Elements/rope_2",
                40,
                true,
                false
            );

            path4.CollisionCategories = Category.None;

            GI.World.AddJoint(
                new RevoluteJoint(ps1.Body, path4.LastBody, new Vector2(-4, -0.25f), new Vector2(0, 0.9f))
            );
        }

        bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (firstTime)
            {
                ps1.BodyType = BodyType.Dynamic;
                firstTime = false;
            }
            return true;
        }

        public override void Update(GameTime gameTime)
        {
            ps1.Update(gameTime);
            ps2.Update(gameTime);

            if (ps1.CurrentPosition.X > ps1.StartPosition.X && ps1.BodyType == BodyType.Dynamic)
            {

                if (linearVelocity == Vector2.Zero)
                    linearVelocity = ps1.Body.LinearVelocity;

                ps2.BodyType = BodyType.Dynamic;
                ps2.Body.LinearVelocity = linearVelocity;

                ps1.Body.LinearVelocity = Vector2.Zero;
                ps1.Body.Position = new Vector2(ps1.StartPosition.X, ps1.Body.Position.Y);
                ps1.BodyType = BodyType.Static;
                GI.Sound.PlaySFX(TikiSound.Newton_01, 0.5f, ps2.CurrentPosition);
            }
            if (ps2.CurrentPosition.X < ps2.StartPosition.X && ps2.BodyType == BodyType.Dynamic)
            {
                ps1.BodyType = BodyType.Dynamic;
                ps1.Body.LinearVelocity = -linearVelocity;

                ps2.Body.LinearVelocity = Vector2.Zero;
                ps2.Body.Position = new Vector2(ps2.StartPosition.X, ps2.Body.Position.Y);
                ps2.BodyType = BodyType.Static;

                GI.Sound.PlaySFX(TikiSound.Newton_02, 0.5f, ps2.CurrentPosition);
            }

        }

        public override void Draw(GameTime gameTime)
        {
            ps1.Draw(gameTime);
            ps2.Draw(gameTime);
            path1.Draw(gameTime);
            path2.Draw(gameTime);
            path3.Draw(gameTime);
            path4.Draw(gameTime);
        }

        public override void Reset()
        {

            ps1.CurrentPosition = ps1.StartPosition;
            ps1.CurrentPosition -= new Vector2(swingDistance, 0);
            ps1.BodyType = BodyType.Static;
            firstTime = true;


            ps2.CurrentPosition = ps2.StartPosition;
            ps2.CurrentPosition += new Vector2(0, swingDistance);
            ps2.BodyType = BodyType.Static;

            //ps1.StartPosition = ps1.CurrentPosition;
            ///ps2.StartPosition = ps2.CurrentPosition;
            //base.Reset();

            //ps1.BodyType = BodyType.Static;
            //ps1.CurrentPosition = ps1.StartPosition;
            //ps1.Body.LinearDamping = 5f;
            //firstTime = true;

            //ps2.CurrentPosition = ps2.StartPosition;
            //ps2.BodyType = BodyType.Static;


        }

        protected override void ApplyChanges()
        {
            throw new NotImplementedException();
        }

        public override Vector2 CurrentPosition
        {
            get { return (ps1.CurrentPosition + ps2.CurrentPosition) / 2; }
            set { base.CurrentPosition = value; }
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        public override bool Ready
        {
            get { throw new NotImplementedException(); }
        }
    }
}
