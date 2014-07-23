using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using TikiEngine.Elements.Trigger;

namespace TikiEngine.Elements.Events
{
    public class CameraEvent : GameEvent
    {
        private long time = 5000;

        private Vector2 oldOffset;
        private Vector2 offset;

        private Vector2 oldOffsetSpeed;
        private Vector2 offsetSpeed;

        private Body oldBody;
        private Body body;

        private bool finished = false;

        private delegate void TriggerEvent();
        private TriggerEvent triggerEvent;

        private GameTrigger secondTrigger;

        public CameraEvent(GameTrigger trigger, GameTrigger secondTrigger, Vector2 offset, Vector2 offsetSpeed, Body body)
            :base(trigger)
        {
            this.secondTrigger = secondTrigger;

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
        public CameraEvent(GameTrigger trigger,Vector2 offset, Vector2 offsetSpeed, Body body)
            :base(trigger)
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
            if (finished)
                return;

            if (!activated)
            {
                if (trigger.Triggered())
                {
                    if (triggerEvent != null)
                    {
                        triggerEvent();
                        Console.WriteLine("Erste mal");
                    }
                    activated = true;
                }
            }
            //else
            //{
            //    if (secondTrigger != null)
            //    {
            //        if (secondTrigger.Triggered())
            //        {
            //            if (triggerEvent != null)
            //            {
            //                triggerEvent();
            //            }
            //            if (Unique)
            //                finished = true;
            //            else
            //                activated = false;
            //        }
            //    }
            //    else
            //    {
            //        time -= gameTime.ElapsedGameTime.Milliseconds;
            //        if (time < 0)
            //        {
            //            if (triggerEvent != null)
            //            {
            //                triggerEvent();
            //                Console.WriteLine("zweite mal");
            //            }
            //            if (Unique)
            //                finished = true;
            //            else
            //            {
            //                activated = true;
            //                time = 5000;
            //            }
                              
            //        }
            //    }
            //}
        }

        public void ChangeOffset()
        {
            if (GI.Camera.TargetOffset != Offset)
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
    }
}
