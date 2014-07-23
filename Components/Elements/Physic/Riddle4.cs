using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics.Joints;
using TikiEngine.Elements.Trigger;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using TikiEngine.Elements.Graphics;

namespace TikiEngine.Elements.Physic
{
    public class Riddle4 : NameObject
    {
        private PhysicTexture element1;
        private PhysicTexture element2;

        private float length1;
        private float length2;

        private float targetLength1 = 0;
        private float targetLength2 = 0;

        private FixedDistanceJoint fdj;
        private FixedPrismaticJoint fpj;

        private FixedDistanceJoint fdj2;
        private FixedPrismaticJoint fpj2;

        private CollisionTrigger ct1;
        private CollisionTrigger ct2;

        private long timer = 0;

        private float CUBEWEIGHT = 4;
        private float ROBOWEIGHT = 1;

        public Riddle4(Vector2 pos1, Vector2 pos2, float length1, float length2)
        {
            this.length1 = length1;
            this.length2 = length2;

            #region element1
            this.element1 = new PhysicTexture();

            this.element1.TextureFile = "Islands/movingPlatformSmall_n";
            this.element1.TextureBodyFile = "Islands/movingPlatformSmall_c";
            this.element1.Material = Material.wood;

            this.element1.Density = 50;
            this.element1.StartPosition = pos1 - new Vector2(0, length1 / 2);
            this.element1.LayerDepth = LayerDepthEnum.Island;
            this.element1.Body.Restitution = 0;

            fpj = JointFactory.CreateFixedPrismaticJoint(
                GI.World, element1.Body, element1.StartPosition, new Vector2(0, 1));
            fdj = JointFactory.CreateFixedDistanceJoint(
                GI.World, element1.Body, Vector2.Zero, element1.StartPosition);

            this.element1.CurrentPosition = element1.StartPosition + new Vector2(0, length1 / 2);

            fdj.Length = length1/2;

            this.ct1 = new CollisionTrigger(element1);


            #endregion
            #region element2
            this.element2 = new PhysicTexture();

            this.element2.TextureFile = "Islands/movingPlatformSmall_n";
            this.element2.TextureBodyFile = "Islands/movingPlatformSmall_c";
            this.element2.Material = Material.wood;

            this.element2.Density = 50;
            this.element2.StartPosition = pos2 - new Vector2(0, length2 / 2);
            this.element2.LayerDepth = LayerDepthEnum.Island;
            this.element2.Body.Restitution = 0;

            fpj2 = JointFactory.CreateFixedPrismaticJoint(GI.World, element2.Body, element2.StartPosition, new Vector2(0, 1));
            fdj2 = JointFactory.CreateFixedDistanceJoint(
                GI.World, element2.Body, Vector2.Zero, element2.StartPosition);

            this.element2.CurrentPosition = element2.StartPosition + new Vector2(0, length2 / 2);

            fdj2.Length = length2/2;

            this.ct2 = new CollisionTrigger(element2);
        
            #endregion

            //this.element1.BodyType = BodyType.Static;
            //this.element2.BodyType = BodyType.Static;

            fdj.Frequency = 3;
            fdj2.Frequency = 3;

            element1.Body.LinearDamping = 1;
            element2.Body.LinearDamping = 1;

            this.element1.AddAttachedElement(new AttachedElement(new Sprite(@"Elements/rope_100"), new Vector2(2.079998f, -15.28003f), LayerDepthEnum.Foreground));
            this.element1.AddAttachedElement(new AttachedElement(new Sprite(@"Elements/rope_100"), new Vector2(-2.089999f, -15.30002f), LayerDepthEnum.Foreground));

            this.element2.AddAttachedElement(new AttachedElement(new Sprite(@"Elements/rope_100"), new Vector2(2.079998f, -15.28003f), LayerDepthEnum.Foreground));
            this.element2.AddAttachedElement(new AttachedElement(new Sprite(@"Elements/rope_100"), new Vector2(-2.089999f, -15.30002f), LayerDepthEnum.Foreground));

            //fdj.DampingRatio = 3;
            //fdj2.DampingRatio = 3;
            
        }
        public override void Draw(GameTime gameTime)
        {
            this.element1.Draw(gameTime);
            this.element2.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {

            this.element1.Update(gameTime);
            this.element2.Update(gameTime);

            this.element1.Body.LinearVelocity = new Vector2(0, 0.1f);
            this.element2.Body.LinearVelocity = new Vector2(0, 0.1f);

            this.ct1.Update(gameTime);
            this.ct2.Update(gameTime);

            fdj.Length += (targetLength1 - fdj.Length) * 0.01f;
            fdj2.Length += (targetLength2 - fdj2.Length) * 0.01f;


            if (!ct1.Triggered() && !ct2.Triggered())
            {
                timer += gameTime.ElapsedGameTime.Milliseconds;
                if (timer > 500)
                {
                    targetLength1 = length1 / 2;
                    targetLength2 = length2 / 2;

                    //fdj.Length -= (fdj.Length - length1 / 2) * 0.1f;
                    //fdj2.Length -= (fdj2.Length - length2 / 2) * 0.1f;
                }
            }
            else// if (ct1.Triggered() && ct2.Triggered())
            {
                timer = 0;

                float counter1 = 0;
                float counter2 = 0;

                if (ct1.BoxCollision())
                    counter1 += CUBEWEIGHT;
                if (ct1.RoboCollision())
                    counter1 += ROBOWEIGHT;

                if (ct2.BoxCollision())
                    counter2 += CUBEWEIGHT;
                if (ct2.RoboCollision())
                    counter2 += ROBOWEIGHT;

                if (counter1 == counter2)
                {
                    //fdj.Length -= (fdj.Length - length1 / 2) * 0.1f;
                    //fdj2.Length -= (fdj2.Length - length2 / 2) * 0.1f;

                    targetLength1 = length1 / 2;
                    targetLength2 = length2 / 2;
                }
                if (counter1 > counter2)
                {
                    counter1 -=counter2;

                    targetLength1 = length1 / 2 + counter1 * (length1 / 2) / (CUBEWEIGHT + ROBOWEIGHT);
                    //fdj.Length = length1 / 2 + counter1 * (length1 / 2) / (CUBEWEIGHT + ROBOWEIGHT);

                    float differenz = this.element1.CurrentPosition.Y - this.element1.StartPosition.Y;
                    float koef = differenz / length1;
                    //fdj2.Length = length2 * (1 - koef);
                    targetLength2 = length2 * (1 - koef);
                }
                else
                {
                    counter2 -= counter1;

                    targetLength2 = length2 / 2 + counter2 * (length2 / 2) / (CUBEWEIGHT + ROBOWEIGHT);
                    //fdj2.Length = length2 / 2 + counter2 * (length2 / 2) / (CUBEWEIGHT + ROBOWEIGHT);

                    float differenz = this.element2.CurrentPosition.Y - this.element2.StartPosition.Y;
                    float koef = differenz / length2;
                    //fdj.Length = length1 * (1 - koef);
                    targetLength1 = length1 * (1 - koef);
                }




            }
        }
        protected override void ApplyChanges()
        {
            throw new NotImplementedException();
        }

        public override Vector2 CurrentPosition
        {
            get { return (element1.CurrentPosition + element2.CurrentPosition) / 2; }
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
