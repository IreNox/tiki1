using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision.Shapes;

namespace TikiEngine.Elements.Physic
{
    [Serializable]
    public class PhysicPath : NameObjectPhysic
    {
        #region Vars
        private Path _path;
        private List<Body> _bodies;

        private int _count;
        private Vector2 _size;

        private bool _fixedStart = true;
        private bool _fixedEnd = true;

        private Vector2 _startPoint;
        private Vector2 _endPoint;
        #endregion

        #region Init
        public PhysicPath()
            : base()
        { 
        }

        public PhysicPath(Vector2 startPoint, Vector2 endPoint, Vector2 size, int count, string texture, float density, bool fixedStart, bool fixedEnd)
            : base(texture)
        {
            this.Size = size;
            _count = count;

            _startPoint = startPoint;
            _endPoint = endPoint;

            _fixedStart = fixedStart;
            _fixedEnd = fixedEnd;

            this.density = density;

            this.ApplyChanges();
        }

        public PhysicPath(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Member
        public override void Draw(GameTime gameTime)
        {
            foreach (Body body in _bodies)
            {
                Point pos = ConvertUnits.ToDisplayUnits(body.Position).ToPoint();
                Point size = ConvertUnits.ToDisplayUnits(_size).ToPoint();

                spriteBatch.Draw(
                    this.Texture,
                    new Rectangle(
                        pos.X,
                        pos.Y,
                        size.X,
                        size.Y
                    ),
                    null,
                    Color.White,
                    body.Rotation,
                    this.TextureSize / 2,
                    SpriteEffects.None,
                    layerDepth
                );
            }

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public void DoForAll(Action<Body> del)
        { 
            foreach (Body b in _bodies)
            {
                del(b);
            }
        }

        public override void Dispose()
        {
            _bodies.ForEach(
                b => b.Dispose()
            );

            base.Dispose();
        }
        #endregion

        #region Member - Protected
        protected override void CreateBody()
        {
            if (_bodies != null)
            {
                foreach (Body body in _bodies)
                {
                    body.Dispose();
                }
            }

            _path = new Path();
            _path.Add(_startPoint);
            _path.Add(_endPoint);
            _path.Closed = false;

            Vector2 halfSize = _size / 2;

            Vertices box = PolygonTools.CreateRectangle(halfSize.X, halfSize.Y);
            PolygonShape shape = new PolygonShape(box, density);

            _bodies = PathManager.EvenlyDistributeShapesAlongPath(world, _path, shape, BodyType.Dynamic, _count);

            foreach (Body b in _bodies)
            {
                b.UserData = this;
            }

            if (_fixedStart)
            {
                JointFactory.CreateFixedRevoluteJoint(world, _bodies[0], new Vector2(0f, -halfSize.Y), _startPoint);
            }

            if (_fixedEnd)
            {
                JointFactory.CreateFixedRevoluteJoint(world, _bodies[_bodies.Count - 1], new Vector2(0f, halfSize.Y), _endPoint);
            }

            PathManager.AttachBodiesWithRevoluteJoint(
                world,
                _bodies,
                new Vector2(0f, -halfSize.Y),
                new Vector2(0f, halfSize.Y),
                false,
                false
            );

            this.body = _bodies[0];
        }
        #endregion

        #region Properties
        public List<Body> Bodies
        {
            get { return _bodies; }
        }

        public int Count
        {
            get { return _count; }
            set
            {
                _count = value;
                this.ApplyChanges();
            }
        }

        public Vector2 Size
        {
            get { return _size; }
            set
            {
                _size = new Vector2(value.Y, value.X);
                this.ApplyChanges();
            }
        }

        public Body LastBody
        {
            get { return _bodies.Last(); }
        }

        public bool FixedStart
        {
            get { return _fixedStart; }
            set
            {
                _fixedStart = value;
                this.ApplyChanges();
            }
        }

        public bool FixedEnd
        {
            get { return _fixedEnd; }
            set
            {
                _fixedEnd = value;
                this.ApplyChanges();
            }
        }

        public Vector2 StartPoint
        {
            get { return _startPoint; }
            set
            {
                _startPoint = value;
                this.ApplyChanges();
            }
        }

        public Vector2 EndPoint
        {
            get { return _endPoint; }
            set
            {
                _endPoint = value;
                this.ApplyChanges();
            }
        }

        public Category CollisionCategories
        {
            set
            {
                foreach (Body b in _bodies)
                {
                    b.CollisionCategories = value;
                }
            }
        }

        public override bool Ready
        {
            get { return _endPoint.IsReady() && _size.IsReady() && _count > 0; }
        }
        #endregion
    }
}
