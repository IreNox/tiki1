using TikiEngine.Elements.Physic;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace TikiEngine.Elements.Particle
{
    public class systemDestroyDust : systemDust
    {
        #region Vars
        private Body _body;

        private Vector2 _offset;
        #endregion

        #region Init
        public systemDestroyDust(Body body, Vector2 offset)
        {
            _body = body;
            _offset = offset;

            this.Width = 0;
            this.Force = 7;
            this.LifeTime = 10;

            this.CurrentPosition = _body.Position + _offset;
        }
        #endregion

        #region Member
        public override void Update(GameTime gameTime)
        {
            this.CurrentPosition = _body.Position + _offset;

            base.Update(gameTime);
        }
        #endregion
    }
}
