using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TikiEngine.Elements.Physic
{
    public class behaviorAngularImpulse : Behavior
    {
        public behaviorAngularImpulse(NameObjectPhysic nop)
            :base(nop)
        {

        }
      

        public override void ApplyChanges()
        {
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            this.nop.Body.Rotation += MathHelper.ToRadians(0.25f);
        }
    }
}
