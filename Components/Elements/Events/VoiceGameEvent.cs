using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TikiEngine.Elements.Graphics;
using Microsoft.Xna.Framework;
using TikiEngine.Elements.Trigger;

namespace TikiEngine.Elements.Events
{
    public class VoiceGameEvent : GameEvent
    {
        #region Vars
        private Action<GameTime> action;
        private string msg = "";
        #endregion

        #region Init
        public VoiceGameEvent(GameTrigger trigger, string msg, bool unique)
            : base(trigger)
        {
            action = Action;
            this.msg = msg;
            this.unique = unique;
        }
        #endregion

        #region Member
        public override void Update(GameTime gameTime)
        {
            trigger.Update(gameTime);

            if (trigger.Triggered())
            {
                action(gameTime);
            }

            base.Update(gameTime);
        }

        public void Action(GameTime gt)
        {
            GI.Voice.addMessage(msg, true);
        }
        #endregion
    }
}

