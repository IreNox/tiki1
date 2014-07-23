using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using TikiEngine.States;
using TikiEngine.Elements.Physic;
using TikiEngine.Elements.Graphics;

namespace TikiEngine.Elements
{
    [Serializable]
    internal class LevelGame :
#if DESIGNER
        LevelDesigner
#else
        Level
#endif
    {
        #region Vars
        private Robo _robo;

        private float _zoom = 1;
        #endregion

        #region Init
        public LevelGame()
        {
            _robo = new Robo();
            _robo.StartPosition = this.StartPosition;
        }

        public LevelGame(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Protected Member
#if DEBUG2
        protected override NameObject[] selectAll()
        {
            return base.selectAll().Where(e => Vector2.DistanceSquared(_robo.CurrentPosition, e.CurrentPosition) < (1000 / GI.Camera.RealZoom)).ToArray();
        }
#endif
        #endregion

        #region Member
#if DESIGNER
        public override void OnActivate()
        {
            GI.Camera.Zoom = _zoom;

            base.OnActivate();
        }
#else
        public virtual void OnActivate()
        {
            GI.Camera.Zoom = _zoom;

            Program.Game.GetGameState<stateMenu>().Menus["main"][4].Enabled = true;
        }
#endif

        protected override void ApplyChanges()
        {
        }        
        #endregion

        #region Member - Xna - Draw
        //public override void Draw(GameTime gameTime)
        //{
        //    _sprite.TextureFile = "base/leftHand";

        //    Vector2 pos = (GI.Control.KinectCurrentState.LeftPoint + GI.Control.KinectCurrentState.RightPoint) / 2;

        //    _sprite.CurrentPosition = ConvertUnits.ToSimUnits(pos);
        //    _sprite.Draw(gameTime);

        //    pos = GI.Control.KinectCurrentState.LeftPoint;

        //    _sprite.CurrentPosition = ConvertUnits.ToSimUnits(pos);
        //    _sprite.Draw(gameTime);

        //    if (!GI.Control.KinectCurrentState.PointsJointly)
        //    {
        //        _sprite.TextureFile = "base/rightHand";

        //        _sprite.CurrentPosition = ConvertUnits.ToSimUnits(GI.Control.KinectCurrentState.RightPoint);
        //        _sprite.Draw(gameTime);
        //    }

        //    base.Draw(gameTime);
        //}
        #endregion

        #region Member - Xna - Update
        public override void Update(GameTime gameTime)
        {
            Vector2 mouse = GI.Control.MouseSimVector();
            
            bool build = true;
            if (Vector2.Distance(mouse, _robo.CurrentPosition) < TikiConfig.BuildMaxDistance)
            {
                RayCastCallback rayCast = (Fixture fixture, Vector2 point, Vector2 normal, float fraction) =>
                {
                    if (!Robo.IgnoreBuild.Contains(((NameObjectPhysic)fixture.Body.UserData).Material))
                    {
                        build = false;
                    }

                    return fraction;
                };

                world.RayCast(
                    rayCast,
                    mouse + new Vector2(1f, -1f),
                    mouse + new Vector2(-1f, 1f)
                );

                world.RayCast(
                    rayCast,
                    mouse + new Vector2(-1f, -1f),
                    mouse + new Vector2(1f, 1f)
                );
            }
            else
            {
                build = false;
            }

            bool destroy = world.TestPointAll(mouse).Any(
                f => GI.Level.ElementsDestroy.Any(b => b.Body.FixtureList.Contains(f))
            );

            if (destroy)
            {
                GI.Mouse.CurrentMouse = MouseCursor.Destroy;
            }
            else if (build)
            {
                GI.Mouse.CurrentMouse = MouseCursor.Build;
            }
            else
            {
                GI.Mouse.CurrentMouse = MouseCursor.Default;
            }

            base.Update(gameTime);
        }
        #endregion

        #region Properties
        internal Robo Robo
        {
            get { return _robo; }
            set { _robo = value; }
        }

        public override Vector2 CurrentPosition
        {
            get { return _robo.CurrentPosition; }
            set { _robo.CurrentPosition = value; }
        }

        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; }
        }

        public override bool Ready
        {
            get { return true; }
        }
        #endregion

        #region Static
        public static LevelGame CurrentLevel
        {
            get { return Program.Game.GetGameState<stateLevel>().CurrentLevel; }
        }
        #endregion
    }
}
