using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TikiEngine.Components;

namespace TikiEngine.Elements.Trigger
{
    public abstract class GameTrigger : NameObject
    {
        protected RotatedRectangle rotatedRectangle;

        private bool unique = false;

        public GameTrigger(Rectangle r, float rotation)
        {
            this.rotatedRectangle = new RotatedRectangle(
                r,rotation
            );
            this.rotatedRectangle.Color = Color.Red;
        }

        public GameTrigger(Rectangle r)
            :this(r,0)
        {
        }
        public GameTrigger() { }

#if DEBUG
        public override void Draw(GameTime gameTime)
        {
            rotatedRectangle.Draw(gameTime);
        }
#else
        public override void Draw(GameTime gameTime)
        { 
        }
#endif

        public abstract bool Triggered();

        public override void Reset()
        {
        }

        public override bool Ready
        {
            get{ throw new NotImplementedException(); }
        }
        protected override void ApplyChanges()
        {
            throw new NotImplementedException();
        }
        public bool Unique
        {
            get { return this.unique; }
            set { this.unique = value; }
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
