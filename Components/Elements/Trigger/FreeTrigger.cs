using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace TikiEngine.Elements.Trigger
{
    public class FreeTrigger : GameTrigger
    {

        private Vector2 oldOffset;
        private Vector2 offset;

        private Vector2 oldOffsetSpeed;
        private Vector2 offsetSpeed;

        private Body oldBody;
        private Body body;

        private delegate void TriggerEvent();
        private TriggerEvent triggerEvent;

        public FreeTrigger(Rectangle r, float rotation)
            : base(r, rotation)
        {

        }

        public FreeTrigger(Rectangle r, float rotation, Vector2 offset, Vector2 offsetSpeed, Body body)
            : base(r, rotation)
        {
            if (offsetSpeed != Vector2.Zero)
            {
                OffsetSpeed = offsetSpeed;
                triggerEvent += ChangeOffsetSpeed;
            }

            if (offset != Vector2.Zero)
            {
                Offset = offset;
                triggerEvent += ChangeOffset;
            }

            if (body != null)
            {
                Body = body;
                triggerEvent += ChangeTrackingBody;
            }
        }

        public FreeTrigger(Rectangle r, Vector2 offset, Vector2 offsetSpeed, Body body)
            :base(r)
        {
            if (offsetSpeed != Vector2.Zero)
            {
                OffsetSpeed = offsetSpeed;
                triggerEvent += ChangeOffsetSpeed;
            }

            if (offset != Vector2.Zero)
            {
                Offset = offset;
                triggerEvent += ChangeOffset;
            }

            if (body != null)
            {
                Body = body;
                triggerEvent += ChangeTrackingBody;
            }
        }

        public override void Update(GameTime gameTime)
        {
            
        }
        public override bool Triggered()
        {
            return GI.RoboBounding.Intersects(rotatedRectangle);
        }

        public void ChangeOffset()
        {
            if (GI.Camera.Offset != Offset)
                GI.Camera.Offset = Offset;
            else
                GI.Camera.Offset = oldOffset;
        }
        public void ChangeOffsetSpeed()
        {
            if (GI.Camera.OffsetSpeed != OffsetSpeed)
                GI.Camera.OffsetSpeed = OffsetSpeed;
            else
                GI.Camera.OffsetSpeed = oldOffsetSpeed;
        }
        public void ChangeTrackingBody()
        {
            if (GI.Camera.TrackingBody != Body)
                GI.Camera.TrackingBody = Body;
            else
                GI.Camera.TrackingBody = oldBody;
        }

        #region Properties
        public Body Body
        {
            get { return this.body; }
            set 
            {
                this.oldBody = GI.Camera.TrackingBody;
                this.body = value;
            }
        }
        public Vector2 OffsetSpeed
        {
            get { return this.offsetSpeed; }
            set 
            {
                this.oldOffsetSpeed = GI.Camera.OffsetSpeed;
                this.offsetSpeed = value; 
            }
        }
        public Vector2 Offset
        {
            get { return this.offset; }
            set 
            {
                this.oldOffset = GI.Camera.Offset;
                this.offset = value; 
            }
        }
        #endregion


    }
}
