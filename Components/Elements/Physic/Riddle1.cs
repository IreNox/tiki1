using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Joints;
using TikiEngine.Elements.Trigger;
using FarseerPhysics.Dynamics;
using TikiEngine.Elements.Graphics;

namespace TikiEngine.Elements.Physic
{
    public class Riddle1 : NameObject
    {
        private PhysicTexture element1;
        private PhysicTexture element2;

        private FixedDistanceJoint fdj;
        private FixedPrismaticJoint fpj;
        private float firstLimit = 5;
        private float secondLimit = 15;
        private CollisionTrigger ct;
        private float damping = 8f;
        private long timer = 0;


        private FixedRevoluteJoint frj;
        private Vector2 localAnchor = new Vector2(10,0);
        private float limit = -30;
        

        public Riddle1(Vector2 pos1, Vector2 pos2)
        {
            #region element1
            this.element1 = new PhysicTexture();

            this.element1.TextureFile = "Islands/movingPlatformSmall_n";
            this.element1.TextureBodyFile = "Islands/movingPlatformSmall_c";
            this.element1.Material = Material.wood;

            this.element1.Density = 50;
            this.element1.StartPosition = pos1;
            this.element1.LayerDepth = LayerDepthEnum.Island;
            this.element1.Body.Restitution = 0;
            this.element1.Body.Friction = 10;

            this.element1.Body.LinearDamping = 3f;
            this.element1.AddAttachedElement(new AttachedElement(new Sprite(@"Elements/rope_100"), new Vector2(2.079998f, -15.28003f), LayerDepthEnum.Foreground));
            this.element1.AddAttachedElement(new AttachedElement(new Sprite(@"Elements/rope_100"), new Vector2(-2.089999f, -15.30002f), LayerDepthEnum.Foreground));

            fpj =JointFactory.CreateFixedPrismaticJoint(GI.World, element1.Body, element1.StartPosition, new Vector2(0, 1));
            fdj = JointFactory.CreateFixedDistanceJoint(GI.World, element1.Body, Vector2.Zero, element1.StartPosition);

            fdj.DampingRatio = damping;
            fdj.Frequency = 2;

            ct = new CollisionTrigger(element1);
            #endregion
            #region element2
            this.element2 = new PhysicTexture();

            this.element2.TextureFile = "Islands/bridge_n";
            this.element2.TextureBodyFile = "Islands/bridge_c";
            this.element2.Material = Material.wood;
            this.element2.Body.Friction = 10f;

            this.element2.Density = 5;
            this.element2.StartPosition = pos2;
            this.element2.LayerDepth = LayerDepthEnum.Island;
            this.element2.Body.Restitution = 0;

            this.element2.BodyType = BodyType.Dynamic;

            this.frj = JointFactory.CreateFixedRevoluteJoint(GI.World, element2.Body, -localAnchor, element2.StartPosition - localAnchor);
          
            Limit = limit;

            #endregion
        }

        public override void Draw(GameTime gameTime)
        {
            element1.Draw(gameTime);
            element2.Draw(gameTime);
        }
        public void UpdateElement1(GameTime gameTime)
        {

            element1.Update(gameTime);
            element2.Update(gameTime);

            this.element1.Body.LinearVelocity = new Vector2(0, 1f);
            ct.Update(gameTime);

            if (ct.BoxCollision())
            {
                this.fdj.Length = secondLimit;
                timer = 0;
            }
            else if (ct.RoboCollision())
            {
                this.fdj.Length = firstLimit;
                timer = 0;
            }

            else
            {
                timer += gameTime.ElapsedGameTime.Milliseconds;
                if(timer > 500)
                    this.fdj.Length = 0.5f;
            }
        }

        public void UpdateElement2(GameTime gameTime)
        {

        }
        public void UpdateRiddle1(GameTime gameTime)
        {
            float differenz = element1.CurrentPosition.Y - element1.StartPosition.Y;
            float koef = differenz / secondLimit;// element1.CurrentPosition.Y / (element1.StartPosition.Y + secondLimit);
            koef = 1 - koef;
            Limit = limit * koef;
        }
        public override void Update(GameTime gameTime)
        {
            UpdateElement1(gameTime);
            UpdateElement2(gameTime);
            UpdateRiddle1(gameTime);
        }
        public float Limit
        {
            set
            {
                this.frj.LimitEnabled = true;
                this.frj.LowerLimit = MathHelper.ToRadians(value);
                this.frj.UpperLimit = MathHelper.ToRadians(value);
            }
        }

        public override Vector2 CurrentPosition
        {
            get { return (element1.CurrentPosition + element2.CurrentPosition) / 2; }
            set { base.CurrentPosition = value; }
        }

        protected override void ApplyChanges()
        {
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
