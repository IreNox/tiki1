using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using TikiEngine.Elements.Trigger;
using FarseerPhysics.Dynamics;

namespace TikiEngine.Elements.Physic
{
    public class Riddle3 : NameObject
    {
        private PhysicTexture element1;
        private PhysicTexture element2;

        private float length1;
        private float length2;

        private FixedDistanceJoint fdj;
        private FixedPrismaticJoint fpj;

        private FixedDistanceJoint fdj2;
        private FixedPrismaticJoint fpj2;



        CollisionTrigger ct;

        private long timer = 0;


        public Riddle3(Vector2 pos1, Vector2 pos2, float length1, float length2)
        {
            this.length1 = length1;
            this.length2 = length2;

            #region element1
            this.element1 = new PhysicTexture();

            this.element1.TextureFile = "Islands/island1_n";
            this.element1.TextureBodyFile = "Islands/island1_c";
            this.element2.Material = Material.wood;

            this.element1.Density = 50;
            this.element1.StartPosition = pos1;
            this.element1.LayerDepth = LayerDepthEnum.Island;
            this.element1.Body.Restitution = 0;

            fpj = JointFactory.CreateFixedPrismaticJoint(GI.World, element1.Body, element1.StartPosition, new Vector2(0, 1));
            fdj = JointFactory.CreateFixedDistanceJoint(GI.World, element1.Body, Vector2.Zero, element1.StartPosition);

            fdj.DampingRatio = 15;
            fdj.Frequency = 2;
            fdj.Length = 0;

            this.ct = new CollisionTrigger(element1);
            #endregion
            #region element2
            this.element2 = new PhysicTexture();

            this.element2.TextureFile = "Islands/island1_n";
            this.element2.TextureBodyFile = "Islands/island1_c";
            this.element2.Material = Material.wood;

            this.element2.Density = 50;
            this.element2.StartPosition = pos2 -new Vector2(0, length2);
            this.element2.LayerDepth = LayerDepthEnum.Island;
            this.element2.Body.Restitution = 0;

            //this.element2.BodyType = BodyType.Static;

            fpj2 = JointFactory.CreateFixedPrismaticJoint(GI.World, element2.Body, element2.StartPosition, new Vector2(0, 1));
            fdj2 = JointFactory.CreateFixedDistanceJoint(GI.World, element2.Body, Vector2.Zero, element2.StartPosition);

            //fdj2.DampingRatio = 15;
            fdj2.Frequency = 2;
            fdj2.Length = length2;
            #endregion


        }

        protected override void ApplyChanges()
        {
            throw new NotImplementedException();
        }

        public override void Draw(GameTime gameTime)
        {
            this.element1.Draw(gameTime);
            this.element2.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            UpdateElement1(gameTime);
            UpdateElement2(gameTime);
            UpdateLogic(gameTime);
        }
        public void UpdateElement1(GameTime gameTime)
        {
            this.element1.Update(gameTime);
            this.element1.Body.LinearVelocity = new Vector2(0, 1f);

            ct.Update(gameTime);

            if (ct.BoxCollision())
            {
                this.fdj.Length = length1;
                timer = 0;
            }
            else
            {
                timer += gameTime.ElapsedGameTime.Milliseconds;
                if (timer > 500)
                    this.fdj.Length = 0f;
            }
        }
        public void UpdateElement2(GameTime gameTime)
        {
            this.element2.Body.LinearVelocity = new Vector2(0, 1f);
        }
        public void UpdateLogic(GameTime gameTime)
        {
            float differenz = this.element1.CurrentPosition.Y - this.element1.StartPosition.Y;
            float koef = differenz / length1;
            fdj2.Length = length2 * (1 - koef);
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        public override Vector2 CurrentPosition
        {
            get { return (element1.CurrentPosition + element2.CurrentPosition) / 2; }
            set { base.CurrentPosition = value; }
        }

        public override bool Ready
        {
            get { throw new NotImplementedException(); }
        }
    }
}
