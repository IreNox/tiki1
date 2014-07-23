using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TikiEngine.Elements.Physic
{
    public class Riddle2ext : Riddle2
    {
        public Riddle2ext(Vector2 pos, float length)
            : base(pos, length)
        {

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ct.Update(gameTime);
            this.element1.Update(gameTime);
            this.element1.Body.LinearVelocity = new Vector2(0, Math.Sign(length));
            if (ct.Triggered())
            {
                timer = 0;
                this.fdj.Length = Math.Abs(length);
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
    }
}
