using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TikiEngine.Components;
using TikiEngine.Elements.Physic;

namespace TikiEngine.Elements.Trigger
{
    public class CharakterTrigger : GameTrigger, IAttachable
    {
        private Vector2 offset;
        private RotatedRectangle rotatedRectangle2;

        public CharakterTrigger()
            : base(new Rectangle(0, 0, 100, 100))
        {
            this.rotatedRectangle.Color = Color.RoyalBlue;
            this.rotatedRectangle2 = new RotatedRectangle(new Rectangle(0, 0, 100, 100));
            this.rotatedRectangle2.Color = Color.RoyalBlue;
        }

#if DEBUG
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            this.rotatedRectangle2.Draw(gameTime);
        }
#endif

        public override void Update(GameTime gameTime)
        {
            
        }
        public override bool Triggered()
        {
            if(rotatedRectangle.Intersects((GI.RoboBounding)) && rotatedRectangle2.Intersects(GI.RoboBounding))
                return true;
            return false;
        }

        public float Angle
        {
            get { return this.rotatedRectangle.Rotation; }
            set { this.rotatedRectangle.Rotation = value; }
        }

        public float LayerDepth
        {
            get { return 0; }
            set { return; }
        }

        public Vector2 Offset
        {
            get { return this.offset; }
            set { this.offset = value; }
        }

        Vector2 IAttachable.CurrentPosition
        {
            get { return Vector2.Zero; }
            set
            {
                Matrix rot = Matrix.CreateRotationZ(Angle);
                this.rotatedRectangle.ChangePosition(
                    ConvertUnits.ToDisplayUnits(value + Vector2.Transform(Offset + new Vector2(-0.25f, 0), rot))
                    - new Vector2(rotatedRectangle.Width, rotatedRectangle.Height) / 2);
                this.rotatedRectangle2.ChangePosition(
                    ConvertUnits.ToDisplayUnits(value + Vector2.Transform(Offset + new Vector2(0.25f, 0), rot))
                    - new Vector2(rotatedRectangle.Width, rotatedRectangle.Height) / 2);
            }
        }
    }
}
