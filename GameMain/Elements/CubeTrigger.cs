using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TikiEngine.Components;
using TikiEngine.Elements.Physic;
using TikiEngine.Elements.Graphics;

namespace TikiEngine.Elements.Trigger
{
    public class CubeTrigger : GameTrigger , IAttachable
    {
        private Vector2 offset;
        private RotatedRectangle rotatedRectangle2;
        private bool triggered = false;

        protected Sprite back;
        protected Sprite front;

        protected Sprite backT;
        protected Sprite frontT;

        protected Type type;

        private float shining = 0;

        protected float scale = 1f;

        private PhysicBox currentBox;

        public CubeTrigger()
            :base(new Rectangle(0,0,75,100))
        {
            this.rotatedRectangle2 = new RotatedRectangle(new Rectangle(0,0,75,100));
            this.rotatedRectangle2.Color = Color.Red;

            this.back = new Sprite(@"Environment/Trigger/i_trigger_back");
            this.back.LayerDepth = 0.51f;
            this.backT = new Sprite(@"Environment/Trigger/i_trigger_back_glow");
            this.backT.LayerDepth = 0.52f;

            this.front = new Sprite(@"Environment/Trigger/i_trigger_front");
            this.front.LayerDepth = 0.71f;
            this.frontT = new Sprite(@"Environment/Trigger/i_trigger_front_glow");
            this.frontT.LayerDepth = 0.72f;

            ScaleTriggerImages();

            type = typeof(Cube);
        }
        public void ScaleTriggerImages()
        {
            back.Scale   = this.scale;
            backT.Scale  = this.scale;
            front.Scale  = this.scale;
            frontT.Scale = this.scale;
        }

        public override void Draw(GameTime gameTime)
        {
#if DEBUG
            base.Draw(gameTime);
            this.rotatedRectangle2.Draw(gameTime);
#endif


            back.Draw(gameTime);
            front.Draw(gameTime);

            backT.Draw(gameTime);
            frontT.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            frontT.Opacity = (float)Math.Sin(MathHelper.ToRadians(shining));
            backT.Opacity = (float)Math.Sin(MathHelper.ToRadians(shining));

            if (Unique && triggered)
            {
                shining++;
                shining %= 180;
            }
            else
            {
                if (triggered)
                {
                    if (!GI.Level.ElementsBuild.Contains(currentBox))
                    {
                        shining = 0;
                        triggered = false;
                        currentBox = null;
                        return;
                    }
                    if (currentBox.Bounds.Intersects(rotatedRectangle) && currentBox.Bounds.Intersects(rotatedRectangle2))
                    {
                        shining++;
                        shining %= 180;
                    }
                    else
                    {
                        shining = 0;
                        currentBox = null;
                        triggered = false;
                    }
                }
                else
                {
                    foreach (PhysicBox b in GI.Level.ElementsBuild.OfType<PhysicBox>())
                    {
                        if (b.GetType() == type)
                        {
                            if (b.Bounds.Intersects(rotatedRectangle) && b.Bounds.Intersects(rotatedRectangle2))
                            {
                                triggered = true;
                                currentBox = b;
                            }
                        }
                    }
                }
            }
        }

        public override bool Triggered()
        {
            return triggered;
        }

        public float Angle
        {
            get { return this.rotatedRectangle.Rotation; }
            set 
            {
                this.back.Angle = value;
                this.backT.Angle = value;
                this.front.Angle = value;
                this.frontT.Angle = value;

                this.rotatedRectangle.Rotation = value;
            }
        }

        public float LayerDepth
        {
            get { return 0; }
            set { return; }
        }

        public Vector2 Offset
        {
            get { return this.offset; }
            set 
            {
                this.back.Offset = value;
                this.backT.Offset = value;
                this.front.Offset = value;
                this.frontT.Offset = value;

                this.offset = value; 
            }
        }

        Vector2  IAttachable.CurrentPosition
        {
            get { return Vector2.Zero; }
            set 
            {
                this.back.CurrentPosition = value;
                this.backT.CurrentPosition = value;
                this.front.CurrentPosition = value;
                this.frontT.CurrentPosition = value;
                
                Matrix rot = Matrix.CreateRotationZ(Angle);
                this.rotatedRectangle.ChangePosition(
                    ConvertUnits.ToDisplayUnits(value + Vector2.Transform(Offset + new Vector2(-0.65f,0),rot)) 
                    - new Vector2(rotatedRectangle.Width,rotatedRectangle.Height)/2);
                this.rotatedRectangle2.ChangePosition(
                    ConvertUnits.ToDisplayUnits(value + Vector2.Transform(Offset + new Vector2(0.65f, 0), rot))
                    - new Vector2(rotatedRectangle.Width, rotatedRectangle.Height) / 2);
            }
        }
    }

    public class StoneTrigger : CubeTrigger
    {
        public StoneTrigger()
            :base()
        {
            this.back = new Sprite(@"Environment/Trigger/trigger_back");
            this.back.LayerDepth = 0.51f;
            this.backT = new Sprite(@"Environment/Trigger/trigger_back_glow");
            this.backT.LayerDepth = 0.52f;

            this.front = new Sprite(@"Environment/Trigger/trigger_front");
            this.front.LayerDepth = 0.71f;
            this.frontT = new Sprite(@"Environment/Trigger/trigger_front_glow");
            this.frontT.LayerDepth = 0.72f;

            this.type = typeof(CubeStone);

            this.ScaleTriggerImages();
        }

    }
}
