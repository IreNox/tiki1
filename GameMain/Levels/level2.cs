using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TikiEngine.Elements;
using Microsoft.Xna.Framework;
using TikiEngine.Elements.Physic;
using TikiEngine.Elements.Trigger;
using TikiEngine.Elements.Graphics;
using TikiEngine.Elements.Events;
using Microsoft.Xna.Framework.Input;
using TikiEngine.States;

namespace TikiEngine.Levels
{
    internal class level2 : LevelGame
    {
        #region Vars
        public const int CUBE_WIDTH = 2;
        public const int CUBE_HEIGHT = 2;
        public const int JUMP_WIDTH = 6;
        public const int JUMP_HEIGHT = 2;

        private Vector2 pos;

        private NameObjectPhysic tmpIsland;

        private TutorialPoint _tp1;
        private TutorialPoint _tpNext;
        #endregion

        public level2()
        {
            this.Zoom = 0.4f;

            this.Wolken();

            this.Robo.StartPosition = new Vector2(8, -6);
            //this.Robo.StartPosition = new Vector2(150, 15);
            //this.Robo.StartPosition = new Vector2(180, 10);
            //this.Robo.StartPosition = new Vector2(260, 35);
           
#if DEBUG
            _debug();
#endif
        }
        public override void Initialize()
        {
            //GI.Sound.AddLoopingSound(TikiSound.music_01, 0.3f);
            ////GI.Sound.AddLoopingSound(TikiSound.music_02, 1f);
            //GI.Sound.AddLoopingSound(TikiSound.Atmo, 0.5f);
            //GI.Sound.ChangeSystemVolume(-1);

            //PhysicTexture p1m1 = Setup.CreatePipe("pipe_middle", Vec(0, 0), 20);
            //this.Elements.Add(p1m1);

            #region Islands
            PhysicTexture p1m1 = Setup.CreatePipe("pipe_middle", Vec(8.55f, 3.15f), 20);
            this.Elements.Add(p1m1);

            PhysicTexture p1m2 = Setup.CreatePipe("pipe_middle", Vec(8.55f, 3.15f), 20);
            this.Elements.Add(p1m2);

            PhysicTexture iW1 = Setup.CreateWiggleIsland("island7", 0, Vec(21.5f, 10));
            iW1.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch5"), new Vector2(-1.4f,-2.5f), 0.51f));
            iW1.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\tree2"), new Vector2(-3.099999f,-5.529995f), 0.5f));
            this.Elements.Add(iW1);

            //PhysicTexture iO1 = Setup.CreateWiggleIsland("islandO2a", 0, nVec(0, -15f));
            //this.Elements.Add(iO1);

            PhysicTextureBreakable iO1 = Setup.CreateBreakable("islandO2a_p", nVec(0, -12f),
                new Setup.AttachedGear(1, new Vector2(-0.9f, 1.2f))
                );

            PhysicTexture iW2 = Setup.CreateWiggleIsland("islandF1", 0, Vec(14.5f, -2.5f));
            this.Elements.Add(iW2);

            //PhysicTexture iO2 = Setup.CreateWiggleIsland("islandO1", 0, Vec(6f, -5f));
            //this.Elements.Add(iO2);

            PhysicTextureBreakable iO2 = Setup.CreateBreakable("islandO1_p", Vec(6f, -5f),
                new Setup.AttachedGear(1, new Vector2(-0.9f, 1.2f))
                );


            PhysicTexture iW3 = Setup.CreateWiggleIsland("islandM1", 0, Vec(6f, 6f));
            iW3.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\tree4"), new Vector2(-2.7f,-4.840003f), 0.51f));
            iW3.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch1"), new Vector2(-1f,-2.41f), 0.52f));
            this.Elements.Add(iW3);

            PhysicTextureBreakable iD1 = Setup.CreateBreakableCountdown("islandD1a_p", Vec(9, -1.6f), 1000, 400);
            this.Elements.Add(iD1);

            PhysicTexture iD2 = Setup.CreateWiggleIsland("island1", 0, Vec(12f, -2f));
            iD2.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone1"), new Vector2(2.2f,-3.939998f), 0.51f));
            iD2.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\tree4"), new Vector2(-2.899999f,-5.389989f), 0.52f));
            this.Elements.Add(iD2);

            PhysicTexture iW4 = Setup.CreateWiggleIsland("island6", 0, Vec(19f, -2.5f));
            iW4.AddAttachedElement(new AttachedElement(new CheckPoint(), new Vector2(1.6f,-4.4f), LayerDepthEnum.IslandBackground));
            this.Elements.Add(iW4);

            PhysicTexture p2s = Setup.CreatePipe("pipe_broken_left", Vec(17f, 2f), 20);
            this.Elements.Add(p2s);

            PhysicTexture p2m1 = Setup.CreatePipe("pipe_middle", Vec(5.775f, 2.1f), 20);
            this.Elements.Add(p2m1);

            PhysicTexture p2m2 = Setup.CreatePipe("pipe_middle", Vec(8.55f, 3.15f), 20);
            this.Elements.Add(p2m2);

            PhysicTexture iW5 = Setup.CreateWiggleIsland("island8", 0, Vec(12f, 6.5f));
            iW5.Body.IgnoreCollisionWith(p2m2.Body);
            CubeTrigger ct = new StoneTrigger();
            iW5.AddAttachedElement(new AttachedElement(ct, new Vector2(6.09f, -2.53f), LayerDepthEnum.IslandBackground));
            iW5.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\tree6"), new Vector2(-1.77f,-5.169997f), 0.5f));
            iW5.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone3"), new Vector2(1.2f,-2.889999f), 0.53f));
            iW5.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch5"), new Vector2(3.499999f,-2.9f), 0.51f));
            this.Elements.Add(iW5);

            PhysicTextureBreakable iO3 = Setup.CreateBreakable("islandD1a_p", nVec(-12.5f, -13f),
                new Setup.AttachedGear(10, new Vector2(0.5f, 0f), LayerDepthEnum.Background1)
                );

            Setup.CreateCube(nVec(-13.5f, -14f));

            Riddle2 lu1 = new Riddle2ext(Vec(14f, -1), -5);
            lu1.Trigger = ct;
            this.Elements.Add(lu1);

            PhysicTexture iW6 = Setup.CreateWiggleIsland("island7", 0, Vec(16f, -6.5f));
            iW6.AddAttachedElement(new AttachedElement(new CheckPoint(), new Vector2(0.7800013f, -4.260003f), LayerDepthEnum.IslandBackground));
            this.Elements.Add(iW6);

            PhysicTexture p3s = Setup.CreatePipe("pipe_start", Vec(7f, 0f), 0);
            p3s.Body.IgnoreCollisionWith(iW6.Body);
            this.Elements.Add(p3s);

            PhysicTexture p3m1 = Setup.CreatePipe("pipe_middle", Vec(5.9f, 1.65f), 20);
            this.Elements.Add(p3m1);

            PhysicTexture p3m2 = Setup.CreatePipe("pipe_middle", Vec(8.55f, 3.15f), 20);
            this.Elements.Add(p3m2);

            PhysicTexture p3e = Setup.CreatePipe("pipe_broken_right", Vec(6.32f, 2.33f), 20);
            this.Elements.Add(p3e);

            PhysicTextureBreakable iO4 = Setup.CreateBreakable("islandO1_p", Vec(5f, 1f),
                new Setup.AttachedGear(6, new Vector2(-0.9f, -1f))
                );

            PhysicTexture p4s = Setup.CreatePipe("pipe_broken_left", Vec(4f, 5f), 20);
            this.Elements.Add(p4s);

            PhysicTexture p4m1 = Setup.CreatePipe("pipe_middle", Vec(5.85f, 2.2f), 20);
            this.Elements.Add(p4m1);

            PhysicTexture p4m2 = Setup.CreatePipe("pipe_middle", Vec(8.55f, 3.15f), 20);
            this.Elements.Add(p4m2);

            PhysicTexture iW7 = Setup.CreateWiggleIsland("island6", 0, Vec(20, 10));
            this.Elements.Add(iW7);
            Setup.CreateCube(nVec(-3, -3));

            PhysicTexture iW8 = Setup.CreateWiggleIsland("islandO2a", 0, Vec(10, -1));
            this.Elements.Add(iW8);

            PhysicTexture iW9 = Setup.CreateWiggleIsland("island8", 0, Vec(14f, -5.5f));
            iW9.AddAttachedElement(new AttachedElement(new CheckPoint(), new Vector2(0.9f,-4.37f), LayerDepthEnum.IslandBackground));
            this.Elements.Add(iW9);

            PhysicTexture iM1 = Setup.CreateMovingIsland("metal_platform_small", Vec(15, -1), nVec(10, 0), 18000);
            iM1.AddAttachedElement(new AttachedElement(new Sprite("Elements/chain_4096"), new Vector2(-3.909998f, -21.60005f), LayerDepthEnum.IslandForeGround));
            iM1.AddAttachedElement(new AttachedElement(new Sprite("Elements/chain_4096"), new Vector2(3.879998f, -21.60005f), LayerDepthEnum.IslandForeGround));
            this.Elements.Add(iM1);

            PhysicTexture iW10 = Setup.CreateWiggleIsland("island10", 0, Vec(10, -8));
            this.Elements.Add(iW10);

            PhysicTextureBreakable iO5 = Setup.CreateBreakable("islandO2a_p", Vec(10, 0f),
                new Setup.AttachedGear(2, new Vector2(-0.5f, 1.2f))
                );

            PhysicTexture iW11 = Setup.CreateWiggleIsland("island10", 0, Vec(43, 8));
            iW11.AddAttachedElement(new AttachedElement(new Sprite("Elements/levelstone"), new Vector2(1.119999f, -4.390007f), 0.5f));
            this.Elements.Add(iW11);
            #endregion

            

            //PhysicTexture p3e = Setup.CreatePipe("pipe_broken_right", new Vector2(167.57f, 14.57f), 20.00f);
            //this.Elements.Add(p3e);

            //PhysicTextureBreakable iO3 = Setup.CreateBreakable("island8_p", nVec(-5f, -10f),
            //    new Setup.AttachedGear(2, new Vector2(-0.9f, 1.2f))
            //    );
            //this.Elements.Add(iO3);

            #region Trigger
            _tp1 = new TutorialPoint(
                "Thermomagnetic neutrinuo unstabelizer reactivated.\n" +
                "Hover over a gear.\nIf your curser gets red, click to destroy!",
                "destroy",
                new Vector2(3, 3)
            );
            this.Elements.Add(_tp1);

            _tpNext = new TutorialPoint(
                "Blueprint Scanned.\n" +
                "Great you just found another ancient Blueprint part.\n" +
                "There are just 2 of them left.",
                "end",
                new Vector2(3, 3)
            );
            _tpNext.Started += (sender, e) =>
            {
                Setup.Robo.AllowMove = false;
            };
            _tpNext.Finished += (sender, e) =>
            {
                Program.Game.ChangeGameState<stateLevel>().SetLevel<level3>();
            };
            this.Elements.Add(_tpNext);

            FickDichTrigger trigger = new FickDichTrigger(iW1.Body);
            this.Events.Add(
                new DoWhatIWantEvent(
                    trigger,
                    _tp1.Start,
                    true
                )
            );

            trigger = new FickDichTrigger(iW11.Body);
            this.Events.Add(
                new DoWhatIWantEvent(
                    trigger,
                    _tpNext.Start,
                    true
                )
            );
            #endregion

            base.Initialize();
        }

        public Vector2 Vec(float x, float y)
        {
            this.pos += new Vector2(x, y);
            return this.pos;
        }
        public Vector2 nVec(float x, float y)
        {
            return this.pos + new Vector2(x, y);
        }


//        public override void Update(GameTime gameTime)
//        {
//#if DEBUG
//            if(GI.Control.KeyboardPressed(Keys.NumPad0))
//            {
//            }
//            if (GI.Control.KeyboardPressed(Keys.LeftControl) && this.tmpIsland == null)
//            {
//                createElement();
//            }
//            if (GI.Control.KeyboardPressed(Keys.RightControl) && this.tmpIsland != null)
//            {
//                removeElement();
//            }
//#endif

//            base.Update(gameTime);
//        }
        private void createElement()
        {
            this.tmpIsland =

                //Setup.CreatePipe("pipe_broken_right", nVec(6.32f, 2.33f), 20);
                //Setup.CreatePipe("pipe_middle", nVec(5.85f, 2.2f), 20);
                //Setup.CreatePipe("pipe_end", nVec(5.7f, 1.7f), 0);
                //Setup.CreatePipe("pipe_start", nVec(7f, 0f), 0);
                //Setup.CreatePipe("pipe_broken_left", nVec(4f, 5f), 20);
                //Setup.CreateMovingIsland("metal_platform_small", nVec(15, -1), nVec(20, 0), 9000);

                Setup.CreateWiggleIsland("island10", 0, nVec(10, -8));
                //Setup.CreateTeeterIsland("islandW1", nVec(40, 2f), 2f, 0.2f);

                //Setup.CreateWhipIsland("islandF1", nVec(20, 0), new Vector2 (10,0));
                //Setup.CreateBreakableCountdown("islandD1a_p", nVec(9, -2), 1000, 400);
                //Setup.CreateBreakable("islandO1_p", Vec(5, 5));
                //new Setup.AttachedGear(1, new Vector2(-1, 1)));

                this.Elements.Add(tmpIsland);

                //Setup.CreateWiggleIsland("island7", -10 ,nVec(25, 0));




                //PhysicTextureBreakable islandO1_p = Setup.CreateBreakable("islandO1_p", Vec(10, 2),
                //    new Setup.AttachedGear(1, new Vector2(-1, 1)));
                //this.Elements.Add(islandO1_p);
        }
        private void removeElement()
        {
            this.tmpIsland.Dispose();
            this.Elements.Remove(tmpIsland);
            tmpIsland = null;
        }
#if DEBUG
        private PhysicTexture _debugCircle;

        public override void Update(GameTime gameTime)
        {
            var f = GI.World.TestPoint(_debugCircle.CurrentPosition);

            if (f != null)
            {
                GI.GameVars["Debug"] = f.Body.Position;
                GI.GameVars["DebugTex"] = ((NameObjectPhysic)f.Body.UserData).TextureFile;
            }

            base.Update(gameTime);
        }

        private void _debug()
        {
            _debugCircle = new PhysicTexture("Elements/debug_circle");
            _debugCircle.AddBehavior<behaviorDebug>();

            this.Elements.Add(_debugCircle);
        }
#endif
    }
}
