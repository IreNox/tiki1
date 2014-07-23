using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TikiEngine.Elements.Physic;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using TikiEngine.Elements.Audio;
using TikiEngine.Elements.Graphics;

namespace TikiEngine.Elements
{
    internal class Cube : PhysicBox
    {
        #region Vars
        protected bool destroy = true;
        #endregion

        #region Init
        public Cube(Vector2 pos)
            : this(pos, TikiConfig.BuildBlockSize, 7, "Elements/cube")
        { 

        }

        protected Cube(Vector2 pos, float size, float density, string texture)
        {
            this.StartPosition = pos;
            this.Density = density;
            this.BodyType = BodyType.Dynamic;
            this.TextureFile = texture;
            this.Size = new Vector2(size);
            this.LayerDepth = 0.62f;
            this.Material = Material.block;

            body.Friction = 3f;
            this.Bounds = new RotatedRectangle(Bounds, new Vector2(10));

            this.SoundProfil = new CubeSoundProfil(this);

            GI.Level.ElementsBuild.Add(this);
        }
        #endregion

        #region Member
        public override void Reset()
        {
            base.Reset();

            if (destroy)
            {
                this.Dispose();
            }
            else
            {
                this.Body.ResetDynamics();
                this.CurrentPosition = this.StartPosition;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if((this.CurrentPosition.Y - this.StartPosition.Y) > 20)
            {
                Reset();
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            GI.Level.ElementsBuild.Remove(this);
        }
        #endregion
    }
}
