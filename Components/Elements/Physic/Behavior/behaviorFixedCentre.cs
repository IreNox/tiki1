using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Graphics;

namespace TikiEngine.Elements.Physic
{
    public class behaviorFixedCentre : Behavior
    {
        #region Vars
        private Vector2 _localAnchor = Vector2.Zero;

        private FixedRevoluteJoint _fixedRevoluteJoint;
        #endregion

        #region Init
        public behaviorFixedCentre(NameObjectPhysic nop)
            : base(nop)
        {
        }
        #endregion

        #region Member
#if DEBUG
        public override void Draw(GameTime gameTime)
        {
            if (!GI.DEBUG)
                return;

            Point pos = ConvertUnits.ToDisplayUnits(_fixedRevoluteJoint.WorldAnchorA).ToPoint();
            Texture2D texture = GI.Content.Load<Texture2D>("Elements/circle");

            GI.SpriteBatch.Draw(
                texture,
                new Rectangle(pos.X, pos.Y, 10, 10),
                Color.White
            );
        }
#endif

        public override void Update(GameTime gameTime)
        {
        }

        public override void ApplyChanges()
        {
            nop.BodyType = BodyType.Dynamic;

            if (_fixedRevoluteJoint != null)
            {
                GI.World.RemoveJoint(_fixedRevoluteJoint);
                _fixedRevoluteJoint = null;
            }

            _fixedRevoluteJoint = JointFactory.CreateFixedRevoluteJoint(
                GI.World,
                nop.Body,
                _localAnchor,
                nop.Body.Position + _localAnchor
            );
        }
        #endregion

        #region Properties
        public Vector2 LocalAnchor
        {
            get { return _localAnchor; }
            set
            {
                _localAnchor = value;
                this.ApplyChanges();
            }
        }

        public FixedRevoluteJoint Joint
        {
            get { return _fixedRevoluteJoint; }
        }
        #endregion
    }
}
