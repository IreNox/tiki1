using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TikiEngine.Elements.Graphics;
using Microsoft.Xna.Framework;
using TikiEngine.Elements.Trigger;

namespace TikiEngine.Elements.Events
{
    public class DoWhatIWantEvent : GameEvent
    {
        #region Vars
        private Action<GameTime> _doWhatIWant;
        #endregion

        #region Init
        public DoWhatIWantEvent(GameTrigger trigger, Action<GameTime> doWhatIWant, bool unique)
            : base(trigger)
        {
            _doWhatIWant = doWhatIWant;

            this.unique = unique;
        }
        #endregion

        #region Member
        public override void Update(GameTime gameTime)
        {
            trigger.Update(gameTime);

            if (trigger.Triggered())
            {
                _doWhatIWant(gameTime);
            }

            base.Update(gameTime);
        }
        #endregion

        #region Properties
        public Action<GameTime> DoWhatIWant
        {
            get { return _doWhatIWant; }
            set { _doWhatIWant = value; }
        } 
        #endregion
    }
}
