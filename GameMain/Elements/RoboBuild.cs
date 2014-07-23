using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using TikiEngine.Elements.Physic;
using TikiEngine.Elements.Particle;
using TikiEngine.Elements.Audio;
using TikiEngine.Elements.Graphics;

namespace TikiEngine.Elements
{
    internal partial class Robo
    {
        #region Vars
        private double _buildTime;
        private Vector2 _buildPos;

        private double _buildPreviewTime;
        private Circle _buildPreviewCircle;
        private Sprite _buildPreviewSprite;

        private systemBuild _particleBuild;
        private List<systemBuildBreak> _listBreak = new List<systemBuildBreak>();
        #endregion

        #region Init
        private void _initBuild()
        {
            _particleBuild = new systemBuild() { 
                IsAlive = false,
                LayerDepth = 0.61f
            };

            _buildPreviewCircle = new Circle(
                new Circle.CircleElement("circle/circle_build1", 0.0001f),
                new Circle.CircleElement("circle/circle_build2", -0.0002f),
                new Circle.CircleElement("circle/circle_build3", 0f)
            ) { 
                LayerDepth = 1.0f
            };
            _buildPreviewCircle.Elements[2].Opacity = 0.25f;

            _buildPreviewSprite = new Sprite(@"elements/cube")
            {
                Opacity = 0.5f,
                LayerDepth = 1.0f
            };
        }
        #endregion

        #region Member - Xna - Draw
        private void _drawBuild(GameTime gameTime)
        {
            _particleBuild.Draw(gameTime);

            foreach (systemBuildBreak sys in _listBreak.ToArray())
            {
                if (sys.IsAlive)
                {
                    sys.Draw(gameTime);
                }
            }

            if (_buildPreviewTime != 0)
            {
                float time = (float)(gameTime.TotalGameTime.TotalSeconds - _buildPreviewTime);
                time *= 3f;
                if (time > 1) time = 1;

                _buildPreviewCircle.Scale = time;
                _buildPreviewCircle.CurrentPosition = body.Position;
                _buildPreviewCircle.Draw(gameTime);

                _buildPreviewSprite.Color = new Color(
                    1f,
                    (GI.Mouse.CurrentMouse == MouseCursor.Build ? 1f : 0f),
                    (GI.Mouse.CurrentMouse == MouseCursor.Build ? 1f : 0f),
                    0.5f
                );
                _buildPreviewSprite.CurrentPosition = GI.Control.MouseSimVector();
                _buildPreviewSprite.Draw(gameTime);
            }
        }
        #endregion

        #region Member - Xna - Update
        private void _updateBuild(GameTime gameTime)
        {
            bool destroy = false;

            _particleBuild.Update(gameTime);
            _buildPreviewCircle.Update(gameTime);

            foreach (systemBuildBreak sys in _listBreak.ToArray())
            {
                if (!sys.IsAlive)
                {
                    sys.Dispose();
                    _listBreak.Remove(sys);
                }
                else
                {
                    sys.Update(gameTime);
                }
            }

            if (_buildPos != Vector2.Zero && _buildTime + 0.99 < gameTime.TotalGameTime.TotalSeconds)
            {
                _particleBuild.IsAlive = false;

                new Cube(_buildPos);
                _buildPos = Vector2.Zero;
            }

            if (GI.Control.MouseDown(MouseButtons.Right) && _buildPreviewTime == 0)
            {
                _buildPreviewTime = gameTime.TotalGameTime.TotalSeconds;
            }
            else if (GI.Control.MouseUp(MouseButtons.Right))
            {
                _buildPreviewTime = 0;
            }

            if (GI.Mouse.CurrentMouse == MouseCursor.Build && !_particleBuild.IsAlive && GI.Control.MouseUp(GI.Control.MouseCurrentState, MouseButtons.Right) && GI.Control.MouseDown(GI.Control.MousePrevState, MouseButtons.Right))
            {
                Vector2 pos = GI.Control.MouseSimVector();

                _particleBuild.IsAlive = true;
                _particleBuild.CurrentPosition = pos;

                _buildPos = pos;
                _buildTime = gameTime.TotalGameTime.TotalSeconds;

                destroy = true;
                GI.Sound.PlaySFX(TikiSound.Cube_Spawn, 0.5f);
            }

            if (GI.Control.MouseClick(MouseButtons.Middle))
            {
                destroy = true;
            }

            if (destroy)
            {
                foreach (PhysicBox pb in GI.Level.ElementsBuild.ToArray())
                {
                    if (!(pb is CubeStone))
                    {
                        _listBreak.Add(
                            new systemBuildBreak()
                            {
                                CurrentPosition = pb.CurrentPosition,
                                LayerDepth = 0.63f
                            }
                        );
                        pb.Dispose();
                        GI.Sound.PlaySFX(TikiSound.Cube_Destroy, 0.5f);

                        GI.Level.ElementsBuild.Remove(pb);
                    }
                }
            }
        }
        #endregion
    }
}
