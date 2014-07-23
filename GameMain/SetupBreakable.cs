using System;
using System.Linq;
using System.Collections.Generic;
using TikiEngine.Elements;
using TikiEngine.Elements.Physic;
using TikiEngine.Elements.Effects;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using TikiEngine.Elements.Graphics;
using TikiEngine.Elements.Particle;

namespace TikiEngine
{
    internal static partial class Setup
    {
        #region Vars
        private static shaderGlow _shaderGlowR;
        private static shaderGlow _shaderGlowB;
        private static shaderBlurAverage _shaderA;
        private static shaderBlurDiagonal _shaderD;
        #endregion

        #region Init
        private static void _initBreakable()
        {
            _shaderA = new shaderBlurAverage();
            _shaderD = new shaderBlurDiagonal();

            _shaderGlowR = new shaderGlow();
            _shaderGlowB = new shaderGlow()
            {
                Color = new Vector4(0.8f, 0.5f, 0.24f, 1f)
            };

            ResetScreen();
        }
        #endregion

        #region Private Member
        private static void _initIsland(PhysicTextureBreakable nop, Vector2 pos)
        {
            nop.Density = 10;
            nop.Strength = 10000;
            nop.DrawSprite = true;
            nop.StartPosition = pos;
            nop.BodyType = BodyType.Static;
            nop.LayerDepth = LayerDepthEnum.Island;
            nop.Body.Friction = 50;
        }
        #endregion

        #region Member
        public static void ResetScreen()
        { 
            _shaderA.TextureSize = GI.Camera.ScreenSize;
            //_shaderD.TextureSize = GI.Camera.ScreenSize;

            _shaderGlowR.TextureSize = GI.Camera.ScreenSize;
            _shaderGlowB.TextureSize = GI.Camera.ScreenSize;
        }

        private static void _updateBreakable(GameTime gameTime)
        {
            _shaderA.Update(gameTime);
            _shaderD.Update(gameTime);

            _shaderGlowR.Update(gameTime);
            _shaderGlowB.Update(gameTime);
        }
        #endregion

        #region Member - Create - Breakable
        public static PhysicBodyBreakable CreateBreakable(string path, Vector2 pos, params IAttachableCreator[] attached)
        {
            PhysicBodyBreakable body = DataManager.LoadObject<PhysicBodyBreakable>(path, true);

            body.AttachableCreators.AddRange(attached);
            _initIsland(body, pos);
            body.OnReselectParts += _breakableOnBreak;
            body.EffectPostProcessing = _shaderA;
            body.ApplyAttachableCreators();

            GI.Level.ElementsDestroyIsland.Add(body);

            return body;
        }

        private static void _breakableOnBreak(object sender, EventArgs e)
        {
            PhysicTextureBreakable body = (PhysicTextureBreakable)sender;

            if (!body.BreakableBody.Broken) return;

            //_shaderGlowR.Speed = -1f;
            //_shaderGlowR.Multiply = 3f;
            //_shaderGlowR.IsAlive = true;

            _shaderA.Speed = 2f;
            _shaderA.Multiply = 0f;
            _shaderA.IsAlive = true;
            body.Effect = _shaderA;

            body.DrawAttachedAssets = false;

            foreach (Body b in body.Parts)
            {
                Vector2 offset = ((PolygonShape)b.FixtureList[0].Shape).Vertices.GetCentroid();

                Vector2 p = b.Position + offset;
                float d = Vector2.Distance(Setup.Robo.DestroyPosition, p);
                Vector2 v = ((Setup.Robo.DestroyPosition - p) * b.Mass * -10) / Math.Max(d, 1);

                b.IgnoreGravity = true;
                b.CollisionCategories = Category.None;
                b.ApplyLinearImpulse(v);

                GI.Level.Particles.Add(
                    new systemDestroyDust(b, offset)
                );
            }

            //GI.Sound.PlaySFX(TikiSound.Island_Destruction,1f);
        }
        #endregion

        #region Member - Create - Countdown
        public static PhysicBodyBreakable CreateBreakableCountdown(string path, Vector2 pos, float strength, float countDown)
        {
            PhysicBodyBreakable body = DataManager.LoadObject<PhysicBodyBreakable>(path, true);

            _initIsland(body, pos);
            body.AddBehavior<behaviorCountdownBreak>().CountDown = countDown;
            body.OnReselectParts += _breakableOnBreakCountdown;

            return body;
        }

        private static void _breakableOnBreakCountdown(object sender, EventArgs e)
        {
            PhysicTextureBreakable body = (PhysicTextureBreakable)sender;

            if (!body.BreakableBody.Broken) return;
            
            _shaderA.Speed = 1f;
            _shaderA.Multiply = 0f;
            _shaderA.IsAlive = true;

            //_shaderGlowB.Speed = 0.5f;
            //_shaderGlowB.Multiply = 0f;
            //_shaderGlowB.IsAlive = true;

            body.Effect = _shaderA;
            body.EffectPostProcessing = _shaderA;

            body.AttachedAssets.Clear();

            foreach (Body b in body.Parts)
            {
                Vector2 offset = ((PolygonShape)b.FixtureList[0].Shape).Vertices.GetCentroid();

                Vector2 p = b.Position + offset;
                float d = Vector2.Distance(body.CurrentPosition, p);
                Vector2 v = ((body.CurrentPosition - p) * b.Mass * -1) / Math.Max(d, 1);

                Setup.Robo.IgnoreCollisionWith(b);

                //b.CollisionCategories = Category.None;
                b.ApplyLinearImpulse(
                    new Vector2(-v.X * 5, 0)
                );
            }
        }
        #endregion

        #region Class - AttachedGear
        public class AttachedGear : IAttachableCreator
        {
            #region Vars
            private int _number;

            private Vector2 _offset;

            private float _layerDepth = LayerDepthEnum.IslandForeGround;
            #endregion

            #region Init
            public AttachedGear(int number, Vector2 offset)
            {
                _number = number;
                _offset = offset;
            }

            public AttachedGear(int number, Vector2 offset, float layerDepth)
                : this(number, offset)
            {
                _layerDepth = layerDepth;
            }
            #endregion

            #region Member
            public AttachedElement CreateAttachableElement()
            {
                PhysicBox box = new PhysicBox();
                box.Density = 0;
                box.TextureFile = "Environment/Gear/gear" + _number.ToString();
                box.Size = ConvertUnits.ToSimUnits(box.TextureSize);
                box.Material = Material.gear;
                box.LayerDepth = _layerDepth;
                box.Body.CollisionCategories = Category.None;

                GI.Level.ElementsDestroy.Add(box);

                return new AttachedElement(box, _offset, LayerDepthEnum.Foreground);
            }
            #endregion

            #region Properties
            public int Number
            {
                get { return _number; }
                set { _number = value; }
            }

            public Vector2 Offset
            {
                get { return _offset; }
                set { _offset = value; }
            }
            #endregion
        }
        #endregion
    }
}
