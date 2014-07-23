using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Joints;
using TikiEngine.Elements.Trigger;
using TikiEngine.Elements.Graphics;

namespace TikiEngine.Elements.Physic
{
    public class Riddle2 : NameObject
    {
        protected PhysicTexture element1;

        protected FixedPrismaticJoint fpj;
        protected FixedDistanceJoint fdj;

        protected GameTrigger ct;

        protected float damping = 8f;
        protected long timer = 0;
        protected float length = 5;

        public Riddle2( Vector2 pos , float length)
        {
            this.length = length;

            this.element1 = new PhysicTexture();

            this.element1.TextureFile = "Islands/metal_platform_small_n";
            this.element1.TextureBodyFile = "Islands/metal_platform_small_c";
            this.element1.Material = Material.metal;

            this.element1.Density = 50;
            this.element1.StartPosition = pos;
            this.element1.LayerDepth = LayerDepthEnum.Island;
            this.element1.Body.Restitution = 0;
            this.element1.Body.Friction = 10;

            this.element1.Body.LinearDamping = 3f;

            this.element1.AddAttachedElement(new AttachedElement(new Sprite(@"Elements/chain_4096"), new Vector2(-3.909998f, -21.60005f), LayerDepthEnum.Foreground));
            this.element1.AddAttachedElement(new AttachedElement(new Sprite(@"Elements/chain_4096"), new Vector2(3.879998f, -21.60005f), LayerDepthEnum.Foreground));

            fpj = JointFactory.CreateFixedPrismaticJoint(GI.World, element1.Body, element1.StartPosition, new Vector2(0, 1));
            fdj = JointFactory.CreateFixedDistanceJoint(GI.World, element1.Body, Vector2.Zero, element1.StartPosition);

            fdj.DampingRatio = damping;
            fdj.Frequency = 2;

            ct = new CollisionTrigger(element1);

        }

        protected override void ApplyChanges()
        {
        }

        public override void Draw(GameTime gameTime)
        {
            this.element1.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {

            ct.Update(gameTime);
            this.element1.Update(gameTime);
            this.element1.Body.LinearVelocity = new Vector2(0,1f);

            if (ct.Triggered())
            {
                timer = 0;
                this.fdj.Length = length;
            }
            else
            {
                timer += gameTime.ElapsedGameTime.Milliseconds;
                if (timer > 500)
                {
                    this.fdj.Length = 0;
                }

            }
        }

        public override void Dispose()
        {
        }

        public GameTrigger Trigger
        {
            get { return this.ct; }
            set { this.ct = value; }

        }

        public override Vector2 CurrentPosition
        {
            get { return element1.CurrentPosition; }
            set { base.CurrentPosition = value; }
        }

        public override bool Ready
        {
            get { throw new NotImplementedException(); }
        }
    }
}
