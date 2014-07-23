using System;
using System.Collections.Generic;
using System.Linq;
using TikiEngine.Elements.Particle;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using TikiEngine.Elements.Physic;

namespace TikiEngine.Elements
{
    internal partial class Robo
    {
        #region Vars
        private systemDestroy _particleDestroy;

        private double _destroyStartTime;

        public static readonly Material[] IgnoreBuild = new Material[] { 
            Material.chain
        };

        public static readonly Material[] IgnoreDestroy = new Material[] { 
            Material.chain,
            Material.gear,
            Material.charakter
        };
        #endregion

        #region Init
        private void _initDestroy()
        {
            _particleDestroy = new systemDestroy()
            {
                Velocity = 20,
                IsAlive = false,
                LayerDepth = 1
            };
        }
        #endregion

        #region Member
        private void _drawDestroy(GameTime gameTime)
        {
            _particleDestroy.Draw(gameTime);
        }

        private void _updateDestroy(GameTime gameTime)
        {
            if (GI.Control.MouseClick(MouseButtons.Left) && (_destroyStartTime + 0.5465 < gameTime.TotalGameTime.TotalSeconds || !_particleDestroy.IsAlive))
            {
                Vector2 vec = GI.Control.MouseSimVector() - this.CurrentPosition;

                _destroyStartTime = gameTime.TotalGameTime.TotalSeconds;
                _particleDestroy.Start(
                    this.CurrentPosition,
                    (float)Math.Atan2(vec.X, vec.Y)
                );
                Fixture fix = this.World.TestPoint(this.CurrentPosition);

                GI.Sound.PlaySFX(TikiSound.Shoot, 1f);
            }

            _particleDestroy.Update(gameTime);

            if (_particleDestroy.IsAlive)
            {
                List<Fixture> list = this.World.TestPointAll(
                    _particleDestroy.CurrentPosition
                );
                

                if (list.Count != 0)
                {
                    NameObjectPhysic aa = list.SelectMany(
                         f => GI.Level.ElementsDestroy.Where(b => b.Body.FixtureList.Contains(f))
                     ).FirstOrDefault();

                    if (aa != null)
                    {
                        var nop = GI.Level.ElementsDestroyIsland.OfType<PhysicTextureBreakable>().First(e => e.AttachedAssets.Any(a => a.Attachable == aa));
                        var boxes = nop.AttachedAssets.Select(a => a.Attachable).OfType<PhysicBox>().ToArray();
                        nop.AttachedAssets.Clear();

                        foreach (PhysicBox box in boxes)
                        {
                            box.Body.Rotation = 0;
                            box.Body.AngularVelocity = 0;
                            box.Body.LinearVelocity = Vector2.Zero;
                            box.TextureFile += "_e";

                            box.AddBehavior<behaviorTracking>().TrackingBody = body;
                            box.AddBehavior<behaviorCollectable>().CollectingBody = body;

                            GI.Level.ElementsDestroy.Remove(box);
                        }

                        nop.BreakableBody.Break();
                    }
                    else if (!list.All(f => Robo.IgnoreDestroy.Contains(((NameObjectPhysic)f.Body.UserData).Material)))
                    {
                        foreach (Fixture f in list)
                        {
                            Material mat = ((NameObjectPhysic)f.Body.UserData).Material;
                            if (!Robo.IgnoreDestroy.Contains(mat))
                            {
                                switch (mat)
                                {
                                    case Physic.Material.island:
                                        GI.Sound.PlaySFX(TikiSound.Shoot_Hit, 1f);
                                        break;
                                    case Physic.Material.metal:
                                    case Physic.Material.block:
                                    case Physic.Material.slide:
                                        GI.Sound.PlaySFX(TikiSound.Shoot_Hit, 1f);
                                        break;
                                    case Physic.Material.wood:
                                        GI.Sound.PlaySFX(TikiSound.Shoot_Hit, 1f);
                                        break;
                                }
                                break;
                            }
                        }
                        _particleDestroy.IsAlive = false;
                    }
                }
            }
        }
        #endregion

        #region Properties
        public Vector2 DestroyPosition
        {
            get { return _particleDestroy.CurrentPosition; }
        }
        #endregion
    }
}
