using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TikiEngine.Elements.Trigger;
using Microsoft.Xna.Framework;
using TikiEngine.Elements.Graphics;
using TikiEngine.Elements.Physic;

namespace TikiEngine.Elements.Events
{
    public class GameEvent
    {
        protected  GameTrigger trigger;

        private Animation animation;

        protected bool activated = false;
        protected bool unique = true;

        public GameEvent() { }
        public GameEvent(GameTrigger trigger, NameObjectPhysic nop, Behavior behavior)
        {
            this.trigger = trigger;
            //Computer.DoWhatIWant();
        }
        public GameEvent(GameTrigger trigger)
        {
            this.trigger = trigger;
        }

        public GameEvent(GameTrigger trigger, Animation animation)
        {
            this.trigger = trigger;
            this.animation = animation;
        }
        public virtual void Update(GameTime gameTime)
        {
            if (!activated)
            {
                if (trigger.Triggered())
                {
                    if (animation != null)
                    {
                        AnimationEvent();
                    }
                    if (trigger.Unique)
                        activated = true;
                }
            }
        }

            //        if (offsetSpeed != Vector2.Zero)
            //{
            //    OffsetSpeed = offsetSpeed;
            //    triggerEvent += ChangeOffsetSpeed;
            //}

            //if (offset != Vector2.Zero)
            //{
            //    Offset = offset;
            //    triggerEvent += ChangeOffset;
            //}

            //if (body != null)
            //{
            //    Body = body;
        //    triggerEvent += ChangeTrackingBody;
        //}
        public void CameraEvent()
        {

        }

        public void AnimationEvent()
        {
            if (!Animation.IsAlive)
                Animation.IsAlive = true;
        }

        #region Properties
        public Animation Animation
        {
            get { return this.animation; }
            set { this.animation = value; }
        }

        public bool Unique
        {
            get { return this.unique; }
            set { unique = value; }
        }
        #endregion
    }
}
