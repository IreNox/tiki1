using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TikiEngine.Elements.Physic;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;

namespace TikiEngine.Elements
{
    internal class SuspensionBridge : NameObject
    {
        #region Vars
        private PhysicBox _leftStone;
        private PhysicBox _rightStone;

        private PhysicPath _topRope;
        private PhysicPath _bottomWood;

        private List<PhysicPath> _listCrossRope = new List<PhysicPath>();
        #endregion

        #region Init
        public SuspensionBridge(Vector2 start, Vector2 end)
        {
            _initStones(start, end);
            _initRobe(start, end);
            _initCrossRope();
        }
        #endregion

        #region Init - Stones
        private void _initStones(Vector2 start, Vector2 end)
        {
            _leftStone = new PhysicBox(
                new Vector2(0.91f, 2.38f),
                start + new Vector2(-0.45f, -1.04f),
                100,
                "elements/bridge/stone_left"
            ) {
                BodyType = BodyType.Static,
                LayerDepth = 0.9f
            };
            _leftStone.Body.CollisionCategories = Category.None;

            _rightStone = new PhysicBox(
                new Vector2(0.91f, 2.38f),
                end + new Vector2(0.35f, -1.04f),
                100,
                "elements/bridge/stone_right"
            ) { 
                BodyType = BodyType.Static,
                LayerDepth = 0.9f
            };
            _rightStone.Body.CollisionCategories = Category.None;
        }
        #endregion

        #region Init - Robe
        private void _initRobe(Vector2 start, Vector2 end)
        {
            float len = Math.Abs(end.X - start.X);

            _topRope = new PhysicPath(
                start + new Vector2(0, -1.8f),
                end + new Vector2(0, -1.8f),
                new Vector2(0.90f, 0.145f),
                (int)(len * 1.22f),
                "Elements/rope_6",
                40,
                false,
                false
            )
            {
                CollisionCategories = Category.None,
                Material = Material.chain,
                LayerDepth = 0.8f
            };

            GI.World.AddJoint(
                new RevoluteJoint(_topRope.Body, _leftStone.Body, new Vector2(0, -0.45f), new Vector2(0.38f, -0.62f))
            );

            GI.World.AddJoint(
                new RevoluteJoint(_topRope.LastBody, _rightStone.Body, new Vector2(0, 0.45f), new Vector2(-0.23f, -0.62f))
            );

            _bottomWood = new PhysicPath(
                start,
                end,
                new Vector2(0.90f, 0.45f),
                (int)(len * 1.22f),
                "Elements/bridge",
                10,
                false,
                false
            )
            {
                Material = Material.wood
            };

            _bottomWood.DoForAll(
                b => b.Friction = 1000
            );

            GI.World.AddJoint(
                new RevoluteJoint(_bottomWood.Body, _leftStone.Body, new Vector2(0, -0.325f), new Vector2(0.45f, 1.19f))
            );

            GI.World.AddJoint(
                new RevoluteJoint(_bottomWood.LastBody, _rightStone.Body, new Vector2(0, 0.225f), new Vector2(-0.45f, 1.19f))
            );

            Setup.Robo.IgnoreCollisionWith(_bottomWood.Body);
            Setup.Robo.IgnoreCollisionWith(_bottomWood.LastBody);
        }
        #endregion

        #region Init - CrossRobe
        private void _initCrossRope()
        {
            for (int i = 0; i < _bottomWood.Bodies.Count; i += 3)
            {
                Body body1 = _topRope.Bodies[i];
                Body body2 = _bottomWood.Bodies[i];

                PhysicPath path = new PhysicPath(
                    new Vector2(body2.Position.X, body2.Position.Y - 1.8f),
                    new Vector2(body2.Position.X, body2.Position.Y),
                    new Vector2(0.3f, 0.145f),
                    6,
                    "Elements/rope_2",
                    5,
                    false,
                    false
                )
                {
                    CollisionCategories = Category.None,
                    Material = Material.chain,
                    LayerDepth = 0.8f
                };
                _listCrossRope.Add(path);

                GI.World.AddJoint(
                    new RevoluteJoint(body1, path.Body, new Vector2(-0.12f, 0), new Vector2(0, -0.05f))
                );

                GI.World.AddJoint(
                    new RevoluteJoint(body2, path.LastBody, new Vector2(0.24f, 0), new Vector2(0, 0.05f))
                );
            } 
        }
        #endregion

        #region Member
        protected override void ApplyChanges()
        {
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Dispose()
        {
        }
        #endregion

        #region Member - Xna - Draw
        public override void Draw(GameTime gameTime)
        {
            _topRope.Draw(gameTime);
            _bottomWood.Draw(gameTime);

            _leftStone.Draw(gameTime);
            _rightStone.Draw(gameTime);

            foreach (PhysicPath path in _listCrossRope)
            {
                path.Draw(gameTime);
            }
        }
        #endregion

        #region Properties
        public PhysicBox LeftStone
        {
            get { return _leftStone; }
        }

        public PhysicBox RightStone
        {
            get { return _rightStone; }
        }

        public Body FirstBody
        {
            get { return _bottomWood.Bodies[1]; }
        }

        public override bool Ready
        {
            get { return true; }
        }
        #endregion
    }
}
