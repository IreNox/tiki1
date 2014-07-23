using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TikiEngine.Elements;
using Microsoft.Xna.Framework;
using TikiEngine.Elements.Physic;
using TikiEngine.Elements.Graphics;
using FarseerPhysics.Dynamics;
using TikiEngine.Elements.Events;
using TikiEngine.Elements.Trigger;
using TikiEngine.States;

namespace TikiEngine.Levels
{
    internal class level1 : LevelGame
    {
        #region Vars
        public const int PS_START_X = 280;
        public const int PS_START_Y = 0;
        public Vector2 pos = new Vector2(0, 0);

        private TutorialPoint _tp1a;
        private TutorialPoint _tp1b;
        private TutorialPoint _tp2a;
        private TutorialPoint _tp2b;
        private TutorialPoint _tp3;
        private TutorialPoint _tpNext;
        #endregion

        public level1()
        {
            this.Zoom = 0.4f;

            this.Wolken();

            this.Robo.StartPosition = new Vector2(-36, -8);
#if DEBUG
            _debug();
#endif   
        }

        public override void Initialize()
        {
            #region First island
            TikiEngine.Elements.Physic.PhysicTexture iW1 = TikiEngine.Setup.CreateWiggleIsland(@"island2", new Microsoft.Xna.Framework.Vector2(-30.54f, -4.59f), 1.00f, 0.00f);
            iW1.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch5"), new Vector2(0.0199997f, -2.43f), 0.53f));
            this.Elements.Add(iW1);

            SuspensionBridge iB1 = new SuspensionBridge(
                new Vector2(-24.1f, -6f),
                new Vector2(-2.33f, -6f)
            );
            this.Elements.Add(iB1);

            //TikiEngine.Elements.Physic.PhysicTexture iB1 = TikiEngine.Setup.CreateWiggleIsland(@"bridge2", new Microsoft.Xna.Framework.Vector2(-8.58f, -4.81f), 1.00f, 0.00f);
            //iB1.Body.IgnoreCollisionWith(iW1.Body);
            //iB1.Body.IgnoreCollisionWith(iW2.Body);
            //this.Elements.Add(iB1);

            //PhysicTexture iW1 = TikiEngine.Setup.CreateWiggleIsland(@"island1", new Vector2(-34.21f, -2.81f), 1.00f, 0.00f);
            //iW1.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone1"), new Vector2(2.2f, -3.939998f), 0.51f));
            //iW1.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\tree4"), new Vector2(-2.899999f, -5.389989f), 0.52f));
            //this.Elements.Add(iW1);

            //PhysicTexture iB1 = TikiEngine.Setup.CreateWiggleIsland(@"bridge2", new Vector2(-10f, -3.64f), 1.00f, 0.00f);
            //this.Elements.Add(iB1);

            TikiEngine.Elements.Physic.PhysicTexture iW2 = TikiEngine.Setup.CreateWiggleIsland(@"island1b", new Microsoft.Xna.Framework.Vector2(6.90f, -3.79f), 1.00f, 0.00f);
            iW2.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone1"), new Vector2(2.2f, -3.939998f), 0.51f));
            iW2.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\tree4"), new Vector2(-2.899999f, -5.389989f), 0.52f));
            this.Elements.Add(iW2);


            //PhysicTexture iW2 = TikiEngine.Setup.CreateWiggleIsland(@"island2", new Vector2(6.90f, -3.64f), 1.00f, 0.00f);
            //iW2.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch5"), new Vector2(0.0199997f, -2.43f), 0.53f));
            //this.Elements.Add(iW2);
            #endregion

            #region Islands
            PhysicTexture iW3 = TikiEngine.Setup.CreateWiggleIsland(@"islandF1", new Vector2(23.86f, -3.91f), 1.00f, 0.00f);
            this.Elements.Add(iW3);

            PhysicTexture iW4 = TikiEngine.Setup.CreateWiggleIsland(@"islandNewton3", new Vector2(38.87f, -1.03f), 1.00f, 0.00f);
            this.Elements.Add(iW4);

            PhysicTexture iW5 = TikiEngine.Setup.CreateWiggleIsland(@"island1b", new Vector2(56.09f, 0.03f), 1.00f, 0.00f);
            iW5.AddAttachedElement(new AttachedElement(new CheckPoint(), new Vector2(0.7800013f, -4.33f), LayerDepthEnum.IslandBackground));
            iW5.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone6"), new Vector2(-4.699998f, -3.799999f), 0.5f));
            this.Elements.Add(iW5);

            PhysicTexture iW6 = TikiEngine.Setup.CreateWiggleIsland(@"islandF1", new Vector2(81.30f, -4.84f), 1.00f, 0.00f);
            this.Elements.Add(iW6);

            PhysicTextureBreakable iD1 = TikiEngine.Setup.CreateBreakableCountdown(@"islandD1a_p", new Vector2(71.14f, -2.47f), 1000, 400);
            this.Elements.Add(iD1);

            PhysicTexture p1s = TikiEngine.Setup.CreatePipe(@"pipe_start", new Vector2(61.73f, 0.44f), 0.00f);
            p1s.Body.IgnoreCollisionWith(iW5.Body);
            this.Elements.Add(p1s);

            PhysicTexture p1m1 = TikiEngine.Setup.CreatePipe(@"pipe_middle", new Vector2(67.59f, 2.06f), 20.00f);
            this.Elements.Add(p1m1);

            PhysicTexture p1m2 = TikiEngine.Setup.CreatePipe(@"pipe_middle", new Vector2(76.12f, 5.17f), 20.00f);
            this.Elements.Add(p1m2);

            PhysicTexture p1m3 = TikiEngine.Setup.CreatePipe(@"pipe_middle", new Vector2(84.67f, 8.29f), 20.00f);
            this.Elements.Add(p1m3);

            PhysicTexture p1e = TikiEngine.Setup.CreatePipe(@"pipe_broken_right", new Vector2(91.00f, 10.62f), 20.00f);
            this.Elements.Add(p1e);

            PhysicTexture p2m2 = TikiEngine.Setup.CreatePipe(@"pipe_middle", new Vector2(90.54f, -1.21f), -20.00f);
            this.Elements.Add(p2m2);

            PhysicTexture p2m1 = TikiEngine.Setup.CreatePipe(@"pipe_middle", new Vector2(100.55f, -7.47f), -40.00f);
            this.Elements.Add(p2m1);

            PhysicTexture p2s1 = TikiEngine.Setup.CreatePipe(@"pipe_end", new Vector2(95.86f, -3.59f), -40.00f);
            this.Elements.Add(p2s1);

            PhysicTexture p2s2 = TikiEngine.Setup.CreatePipe(@"pipe_start", new Vector2(105.43f, -11.49f), -40.00f);
            this.Elements.Add(p2s2);

            PhysicTexture p2e = TikiEngine.Setup.CreatePipe(@"pipe_broken_left", new Vector2(84.68f, 0.98f), -20.00f);
            this.Elements.Add(p2e);

            PhysicTexture iW7 = TikiEngine.Setup.CreateWiggleIsland(@"island6", new Vector2(105.60f, 15.27f), 1.00f, 0.00f);
            iW7.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\tree4"), new Vector2(-2.35f,-5.220007f), 0.5f));
            iW7.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch4"), new Vector2(-2.999999f,-2.4f), 0.52f));
            this.Elements.Add(iW7);

            PhysicTexture iW8 = TikiEngine.Setup.CreateWiggleIsland(@"islandNewton3", new Vector2(116.87f, 13.02f), 1.00f, 0.00f);
            this.Elements.Add(iW8);

            PhysicTextureBreakable iD2 = TikiEngine.Setup.CreateBreakableCountdown(@"islandD1b_p", new Vector2(124.96f, 10.92f), 1000, 400);
            this.Elements.Add(iD2);

            PhysicTextureBreakable iD3 = TikiEngine.Setup.CreateBreakableCountdown(@"islandD1c_p", new Vector2(131.59f, 9.31f), 1000, 400);
            this.Elements.Add(iD3);

            PhysicTexture iW9 = TikiEngine.Setup.CreateWiggleIsland(@"island2", new Vector2(141.83f, 8.20f), 1.00f, 0.00f);
            iW9.AddAttachedElement(new AttachedElement(new CheckPoint(), new Vector2(1.95f,-4),0.53f));
            iW9.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone1"), new Vector2(-2.3f,-3.199999f), 0.52f));
            this.Elements.Add(iW9);

            PhysicTexture p3s = TikiEngine.Setup.CreatePipe(@"pipe_start", new Vector2(146.87f, 7.52f), 0.00f);
            p3s.Body.IgnoreCollisionWith(iW9.Body);
            this.Elements.Add(p3s);

            PhysicTexture p3m1 = TikiEngine.Setup.CreatePipe(@"pipe_middle", new Vector2(152.72f, 9.12f), 20.00f);
            this.Elements.Add(p3m1);

            PhysicTexture p3m2 = TikiEngine.Setup.CreatePipe(@"pipe_middle", new Vector2(161.25f, 12.25f), 20.00f);
            this.Elements.Add(p3m2);

            PhysicTexture p3e = TikiEngine.Setup.CreatePipe(@"pipe_broken_right", new Vector2(167.57f, 14.57f), 20.00f);
            this.Elements.Add(p3e);

            PhysicTexture p4s = TikiEngine.Setup.CreatePipe(@"pipe_broken_left", new Vector2(180.35f, 19.13f), 20.00f);
            this.Elements.Add(p4s);

            PhysicTexture p4m = TikiEngine.Setup.CreatePipe(@"pipe_middle", new Vector2(186.25f, 21.28f), 20.00f);
            this.Elements.Add(p4m);

            PhysicTexture p4e = TikiEngine.Setup.CreatePipe(@"pipe_broken_right", new Vector2(192.58f, 23.62f), 20.00f);
            this.Elements.Add(p4e);

            PhysicTexture iW10 = TikiEngine.Setup.CreateWiggleIsland(@"island7", new Vector2(208.90f, 26.50f), 1.00f, 0.00f);
            iW10.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch5"), new Vector2(-1.4f,-2.5f), 0.51f));
            iW10.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\tree2"), new Vector2(-3.099999f,-5.529995f), 0.5f));
            iW10.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\fence2"), new Vector2(-2.6f,-2.6f), 0.71f));
            this.Elements.Add(iW10);

            PhysicTexture iW11 = TikiEngine.Setup.CreateWiggleIsland(@"island8", new Vector2(227.55f, 29.86f), 1.00f, 0.00f);
            iW11.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\tree6"), new Vector2(-1.77f,-5.169997f), 0.5f));
            iW11.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone3"), new Vector2(1.2f,-2.889999f), 0.53f));
            this.Elements.Add(iW11);
            Setup.CreateCube(new Vector2(220.55f, 28.86f));

            PhysicTexture iW12 = TikiEngine.Setup.CreateWiggleIsland(@"islandO1", new Vector2(240.30f, 28.93f), 1.00f, 0.00f);
            iW12.AddAttachedElement(new AttachedElement(new CheckPoint(), new Vector2(0,-6.62f), 0.51f));
            this.Elements.Add(iW12);

            PhysicTextureBreakable iD4 = TikiEngine.Setup.CreateBreakableCountdown(@"islandD1a_p", new Vector2(248.32f, 24.97f), 1000, 400);
            this.Elements.Add(iD4);

            PhysicTexture iD5 = TikiEngine.Setup.CreateWiggleIsland(@"island1", new Vector2(262.65f, 25.90f), 1.00f, 0.00f);
            iD5.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone1"), new Vector2(2.2f,-3.939998f), 0.51f));
            iD5.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\tree4"), new Vector2(-2.899999f,-5.389989f), 0.52f));
            this.Elements.Add(iD5);

            PhysicTexture iW13 = TikiEngine.Setup.CreateWiggleIsland(@"island6", new Vector2(283.65f, 26.98f), 1.00f, 0.00f);
            CubeTrigger ct = new StoneTrigger();
            iW13.AddAttachedElement(new AttachedElement(ct, new Vector2(0.09f, -2.53f), LayerDepthEnum.IslandBackground));
            this.Elements.Add(iW13);
            Setup.CreateCube(new Vector2(283.74f, 23.45f));

            Riddle2 lu1 = new Riddle2ext(new Vector2(295.48f, 25.22f), -10);
            lu1.Trigger = ct;
            this.Elements.Add(lu1);

            PhysicTexture iW14 = TikiEngine.Setup.CreateWiggleIsland(@"islandO2b", new Vector2(303.31f, 29.42f), 1.00f, 0.00f);
            this.Elements.Add(iW14);

            PhysicTexture iW15 = TikiEngine.Setup.CreateWiggleIsland(@"island2", new Vector2(312.93f, 24.62f), 1.00f, 0.00f);
            iW15.AddAttachedElement(new AttachedElement(new CheckPoint(), new Vector2(0.92f,-3.98f),0.53f));
            iW15.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone1"), new Vector2(-2.3f,-3.199999f), 0.52f));
            this.Elements.Add(iW15);

            PhysicTexture iT1 = TikiEngine.Setup.CreateTeeterIsland(@"islandW1", new Vector2(330.29f, 23.70f), new Vector2(0.00f, 0.00f), 0.00f, 1.00f);
            iT1.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch2"), new Vector2(-2.999999f,-1.7f), 0.5f));
            iT1.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch4"), new Vector2(-2.799999f,-1.8f), 0.51f));
            iT1.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone6"), new Vector2(3.909998f,-2.47f), 0.52f));
            this.Elements.Add(iT1);

            PhysicTexture iW16 = TikiEngine.Setup.CreateWiggleIsland(@"islandO2a", new Vector2(326.57f, 33.94f), 1.00f, 0.00f);
            this.Elements.Add(iW16);

            PhysicTexture iW17 = TikiEngine.Setup.CreateWiggleIsland(@"islandF1", new Vector2(344.11f, 17.23f), 1.00f, 0.00f);
            this.Elements.Add(iW17);

            PhysicTexture p5m1 = TikiEngine.Setup.CreatePipe(@"pipe_middle", new Vector2(351.49f, 18.02f), 20.00f);
            p5m1.Body.IgnoreCollisionWith(iW17.Body);
            this.Elements.Add(p5m1);

            PhysicTexture p5m2 = TikiEngine.Setup.CreatePipe(@"pipe_middle", new Vector2(360.01f, 21.16f), 20.00f);
            this.Elements.Add(p5m2);

            PhysicTexture p5e = TikiEngine.Setup.CreatePipe(@"pipe_broken_right", new Vector2(366.32f, 23.50f), 20.00f);
            this.Elements.Add(p5e);

            PhysicTexture iW18 = TikiEngine.Setup.CreateWiggleIsland(@"island2", new Vector2(381.10f, 27.06f), 1.00f, 0.00f);
            iW18.AddAttachedElement(new AttachedElement(new CheckPoint(), new Vector2(0, -4.05f), LayerDepthEnum.IslandBackground));
            this.Elements.Add(iW18);

            PhysicTextureBreakable iD6 = TikiEngine.Setup.CreateBreakableCountdown(@"islandD1b_p", new Vector2(392.31f, 25.35f), 1000, 400);
            this.Elements.Add(iD6);

            PhysicTexture iW19 = TikiEngine.Setup.CreateWiggleIsland(@"islandF1", new Vector2(401.35f, 23.51f), 1.00f, 0.00f);
            this.Elements.Add(iW19);

            PhysicTexture iH1 = TikiEngine.Setup.CreateWiggleIsland("island8", new Vector2(445.35f, 22.51f), 20f, 0);
            this.Elements.Add(iH1);
            PhysicTexture iH2 = TikiEngine.Setup.CreateWiggleIsland("island10", new Vector2(425.35f, 22.51f), 20f, 0);
            this.Elements.Add(iH2);
            this.Elements.Add(new NewtonIsland(new Vector2(425.35f, 23.51f), 20f, 13));

            PhysicTexture n2 = TikiEngine.Setup.CreateWiggleIsland(@"movingPlatform", new Vector2(435.3f, 36.51f), 1.00f, 0.00f);
            this.Elements.Add(n2);

            PhysicTexture p6m1 = TikiEngine.Setup.CreatePipe(@"pipe_middle", new Vector2(470.84f, 25.65f), 20.00f); //475.84f, 20.65f
            this.Elements.Add(p6m1);

            PhysicTexture p6m2 = TikiEngine.Setup.CreatePipe(@"pipe_middle", new Vector2(479.38f, 28.79f), 20.00f);
            this.Elements.Add(p6m2);

            PhysicTexture p6e = TikiEngine.Setup.CreatePipe(@"pipe_broken_right", new Vector2(485.71f, 31.12f), 20.00f);
            this.Elements.Add(p6e);

            PhysicTexture p7s = TikiEngine.Setup.CreatePipe(@"pipe_broken_left", new Vector2(496.68f, 35.28f), 20.00f);
            this.Elements.Add(p7s);

            PhysicTexture p7e = TikiEngine.Setup.CreatePipe(@"pipe_broken_right", new Vector2(500.38f, 36.60f), 20.00f);
            this.Elements.Add(p7e);

            PhysicTexture p8s = TikiEngine.Setup.CreatePipe(@"pipe_broken_left", new Vector2(510.97f, 40.60f), 20.00f);
            this.Elements.Add(p8s);

            PhysicTexture p8m = TikiEngine.Setup.CreatePipe(@"pipe_middle", new Vector2(516.86f, 42.73f), 20.00f);
            this.Elements.Add(p8m);

            PhysicTexture p8e = TikiEngine.Setup.CreatePipe(@"pipe_broken_right", new Vector2(523.17f, 45.05f), 20.00f);
            this.Elements.Add(p8e);

            PhysicTexture iW20 = TikiEngine.Setup.CreateWiggleIsland(@"island10", new Vector2(543.17f, 55.05f), 1.00f, 0.00f);
            iW20.AddAttachedElement(new AttachedElement(new Sprite("Elements/levelstone"), new Vector2(1.119999f, -4.390007f), 0.5f));
            this.Elements.Add(iW20);
            #endregion

            #region Trigger
            _tp1a = new TutorialPoint(
                "Checking movement abilities.\n" +
                "Try moving to the right by pressing D.",
                "move",
                new Vector2(3, 3)
            );
            _tp1a.Started += (sender, e) =>
            {
                Setup.Robo.AllowMove = false;
            };
            _tp1a.Finished += (sender, e) => {
                Setup.Robo.AllowMove = true;
            };
            this.Elements.Add(_tp1a);

            _tp1b = new TutorialPoint(
                "Try moving to the left by pressing A.",
                "move",
                new Vector2(3, 3)
            );
            _tp1b.Started += (sender, e) =>
            {
                Setup.Robo.AllowMove = false;
            };
            _tp1b.Finished += (sender, e) =>
            {
                Setup.Robo.AllowMove = true;
            };
            this.Elements.Add(_tp1b);

            _tp2a = new TutorialPoint(
                "Try to jump by pressing Space.",
                "jump",
                new Vector2(3, 3)
            );
            this.Elements.Add(_tp2a);

            _tp2b = new TutorialPoint(
                "Movement abilities checked and active.",
                "jump",
                new Vector2(3, 3)
            );
            this.Elements.Add(_tp2b);

            _tp3 = new TutorialPoint(
                "Push this Cube by walking against it",
                "cube",
                new Vector2(3, 3)
            );
            this.Elements.Add(_tp3);

            _tpNext = new TutorialPoint(
                "Blueprint Scanned.\n" +
                "Great you found one part of the ancient Worldblueprints.\n" +
                "Find the other 3 and you are able to repair the World.",
                "end",
                new Vector2(3, 3)
            );
            _tpNext.Started += (sender, e) => {
                Setup.Robo.AllowMove = false;
            };
            _tpNext.Finished += (sender, e) => { 
                Program.Game.ChangeGameState<stateLevel>().SetLevel<level2>();
            };
            this.Elements.Add(_tpNext);

            FickDichTrigger trigger = new FickDichTrigger(iW1.Body);
            this.Events.Add(
                new DoWhatIWantEvent(
                    trigger,
                    _tp1a.Start,
                    true
                )
            );

            trigger = new FickDichTrigger(iB1.FirstBody);
            this.Events.Add(
                new DoWhatIWantEvent(
                    trigger,
                    _tp1b.Start,
                    true
                )
            );

            trigger = new FickDichTrigger(iW2.Body);
            this.Events.Add(
                new DoWhatIWantEvent(
                    trigger,
                    _tp2a.Start,
                    true
                )
            );

            trigger = new FickDichTrigger(iW4.Body);
            this.Events.Add(
                new DoWhatIWantEvent(
                    trigger,
                    _tp2b.Start,
                    true
                )
            );

            trigger = new FickDichTrigger(p1m2.Body);
            this.Events.Add(
                new VoiceGameEvent(
                    trigger,
                    "Oh, did I forget to mention that some of them could break?",
                    true
                )
            );

            trigger = new FickDichTrigger(iW11.Body);
            this.Events.Add(
                new DoWhatIWantEvent(
                    trigger,
                    _tp3.Start,
                    true
                )
            );

            trigger = new FickDichTrigger(iW20.Body);
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

        #region Private Member
        private Vector2 _psVec(int x, int y)
        {
            float factor = 16f;
            pos.X += x;
            pos.Y += y;
            return new Vector2((pos.X) / factor, (pos.Y + PS_START_Y) / factor);
        }

#if DEBUG
        private PhysicTexture _debugCircle;

        public override void Update(GameTime gameTime)
        {
            var f = GI.World.TestPoint(_debugCircle.CurrentPosition);

            if (f != null)
            {
                GI.GameVars["Debug"] = f.Body.Position;
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
        #endregion
    }
}
