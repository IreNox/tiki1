using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;

namespace TikiEngine.Elements.Physic
{
    public class behaviorCollectable : Behavior
    {
        #region Vars
        private Body _collectingBody;

        public event EventHandler OnCollect;
        #endregion

        #region Init
        public behaviorCollectable(NameObjectPhysic nop)
            : base(nop)
        {
        }
        #endregion

        #region Member
        public override void ApplyChanges()
        {
            if (!GI.Level.ElementsCollectable.Contains(nop))
            {
                GI.Level.ElementsCollectable.Add(nop);
            }

            nop.Body.IgnoreGravity = true;
            nop.Body.BodyType = BodyType.Dynamic;
        }

        public override void Update(GameTime gameTime)
        {
            if (_collectingBody != null)
            {
                if (Vector2.Distance(nop.Body.Position, _collectingBody.Position) < 0.5f)
                {
                    GI.Level.ElementsCollectable.Remove(nop);
                    nop.Dispose();

                    GI.Sound.PlaySFX(TikiSound.Collect_Gear, 1);

                    if (this.OnCollect != null) this.OnCollect(this, EventArgs.Empty);
                }
            }
        }
        #endregion

        #region Properties
        public Body CollectingBody
        {
            get { return _collectingBody; }
            set { _collectingBody = value; }
        }
        #endregion
    }
}
