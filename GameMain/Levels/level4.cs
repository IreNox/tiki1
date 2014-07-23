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
    internal class level4 : LevelGame
    {
        public const int CUBE_WIDTH = 2;
        public const int CUBE_HEIGHT = 2;
        public const int JUMP_WIDTH = 6;
        public const int JUMP_HEIGHT = 2;

        private Vector2 pos;

        private NameObjectPhysic tmpIsland;

        private TutorialPoint _tpNext;

        public level4()
        {
            this.Zoom = 0.4f;

            this.Wolken();

            this.Robo.StartPosition = new Vector2(0, -6);
            //this.Robo.StartPosition = new Vector2(290, 38);
            //this.Robo.StartPosition = new Vector2(598, 35);
            //this.Robo.StartPosition = new Vector2(861, 40);
            //this.Robo.StartPosition = new Vector2(325, 43);
            //this.Robo.StartPosition = new Vector2(389, 43);
            //this.Robo.StartPosition = new Vector2(492, 49);
            //this.Robo.StartPosition = new Vector2(594, 39);
            //this.Robo.StartPosition = new Vector2(703, 42);
            //this.Robo.StartPosition = new Vector2(747, 33);
            //this.Robo.StartPosition = new Vector2(855, 43);
            //this.Robo.StartPosition = new Vector2(1018, 40);

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

                #region island1
                PhysicTexture island1 = Setup.CreateWiggleIsland("island1", Vec(0, 0));
                island1.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\tree2"), new Vector2(-2.29f,-5.949991f), 0.51f));
                island1.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\fence1"), new Vector2(2.1f,-2.9f), 0.71f));
                island1.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone3"), new Vector2(2.1f,-2.899999f), 0.52f));
                this.Elements.Add(island1);
                #endregion

                #region islandN0
                PhysicTexture islandN0 = Setup.CreateWiggleIsland("islandN0", Vec(22, -1.5f));
                islandN0.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\tree3"), new Vector2(-0.4799998f,-5.509997f), 0.5f));
                islandN0.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\fence4"), new Vector2(-0.1999999f,-2.3f), 0.6f));
                islandN0.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone4"), new Vector2(5.399997f,-2.899999f), 0.52f));
                this.Elements.Add(islandN0);
                #endregion

                #region island7
                PhysicTexture island7 = Setup.CreateWiggleIsland("island7", 10, Vec(25, 0));
                island7.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\fence2"), new Vector2(-2.6f, -2.6f), 0.71f));
                island7.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch3"), new Vector2(2.1f, -2.5f), 0.52f));
                this.Elements.Add(island7);
                #endregion

                Setup.CreateBreakable("islandO1_p", nVec(-13, -3.5f),
                    new Setup.AttachedGear(1, new Vector2(-1, 1)));

                #region islandW1
                PhysicTexture islandW1 = Setup.CreateTeeterIsland("islandW1", Vec(21, -3.5f), 2f, 0.2f);
                islandW1.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\fence3"), new Vector2(2f,-1.73f), 0.6f));
                islandW1.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch2"), new Vector2(-2.999999f,-1.7f), 0.5f));
                this.Elements.Add(islandW1);
                #endregion

                #region island1b
                PhysicTexture event1island1 = Setup.CreateWiggleIsland("island1b", Vec(21,7f));
                event1island1.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone6"), new Vector2(-4.699998f, -3.799999f), 0.5f));
                event1island1.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch3"), new Vector2(-2.4f, -2.899999f), 0.51f));
                CubeTrigger event1trigger = new StoneTrigger();
                Setup.CreateCube(nVec(0, -6));
                event1island1.AddAttachedElement(new AttachedElement(event1trigger, new Vector2(5.39f,-3.37f), LayerDepthEnum.IslandBackground ));
                this.Elements.Add(event1island1);
                #endregion


                #region island6
                PhysicTexture island6 = Setup.CreateWiggleIsland("island6", 10, Vec(0,-15f));
                island6.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\tree4"), new Vector2(-2.35f,-5.220007f), 0.5f));
                island6.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch4"), new Vector2(-2.999999f,-2.4f), 0.52f));
                this.Elements.Add(island6);
                #endregion

                PhysicTexture event1moving = Setup.CreateMovingIsland("metal_platform_small", Vec(15, 0), Vec(4, 4), 9000);
                event1moving.GetBehavior<behaviorMoving>().Trigger = event1trigger;
                this.Elements.Add(event1moving);

                #region island7_2
                PhysicTexture island7_2 = Setup.CreateWiggleIsland("island7", 0, Vec(20, 10f));
                island7_2.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\tree3"), new Vector2(3.399999f,-5.600002f), 0.53f));
                island7_2.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone4"), new Vector2(-6.399996f,-3.209999f), 0.54f));
                island7_2.AddAttachedElement(new AttachedElement(new CheckPoint(), new Vector2(-2.6f,-4.22f), 0.5f));
                this.Elements.Add(island7_2);
                #endregion

                Setup.CreateBreakable("islandO2a_p", nVec(-9, -11f),
                        new Setup.AttachedGear(1, new Vector2(-1, 1)));

                this.Elements.Add(Setup.CreateWhipIsland("movingPlatformSmall", Vec(18, 0), new Vector2(10, 0)));

                #region islandW1_2
                PhysicTexture islandW1_2 = Setup.CreateTeeterIsland("islandW1", Vec(38, 2f), 2f, 0.2f);
                islandW1_2.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\fence3"), new Vector2(2f, -1.73f), 0.6f));
                islandW1_2.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch2"), new Vector2(-2.999999f, -1.7f), 0.5f));
                this.Elements.Add(islandW1_2);
                #endregion

                #region slide ps1
                //this.Elements.Add(Setup.CreatePipe("islandasian", Vec(12, 10), 180));

                this.Elements.Add(Setup.CreatePipe("pipe_middle", Vec(18, 10), 20));

                this.Elements.Add(Setup.CreatePipe("pipe_middle", Vec(8.3f, 3.04f), 20));

                this.Elements.Add(Setup.CreatePipe("pipe_middle", Vec(8.3f, 3.04f), 20));

                this.Elements.Add(Setup.CreatePipe("pipe_broken_right", Vec(6.1f, 2.2f), 20));

                Setup.CreateBreakable("islandO2a_p", nVec(6, -4f),
                            new Setup.AttachedGear(1, new Vector2(-1, 1)));

                this.Elements.Add(Setup.CreateBreakableCountdown("islandD1a_p", Vec(8, 6), 1000, 900));

                this.Elements.Add(Setup.CreatePipe("pipe_middle", Vec(18, 10), 20));

                this.Elements.Add(Setup.CreatePipe("pipe_middle", Vec(8.3f, 3.04f), 20));

                this.Elements.Add(Setup.CreatePipe("pipe_middle", Vec(8.3f, 3.04f), 20));

                this.Elements.Add(Setup.CreatePipe("pipe_broken_right", Vec(6.1f, 2.2f), 20));

                Setup.CreateBreakable("islandO2b_p", nVec(6, -2f),
                                new Setup.AttachedGear(1, new Vector2(-1, 1)));
                #endregion

                #region island8
                PhysicTexture island8 = Setup.CreateWiggleIsland("island8", 0, Vec(17, 3f));
                island8.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\tree6"), new Vector2(-1.77f,-5.169997f), 0.5f));
                island8.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone3"), new Vector2(1.2f,-2.889999f), 0.53f));
                island8.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch5"), new Vector2(3.499999f,-2.9f), 0.51f));
                island8.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\fence4"), new Vector2(2.2f,-2.5f), 0.71f));
                island8.AddAttachedElement(new AttachedElement(new CheckPoint(), new Vector2(7.5f,-4.41f), 0.5f));
                this.Elements.Add(island8);
                #endregion

                this.Elements.Add(Setup.CreateBreakableCountdown("islandO2b_p", Vec(17, 7), 1000, 1100));

                #region islandD2b
                PhysicTexture islandD2b = Setup.CreateWiggleIsland("islandD2b", Vec(8, -5f),2);
                this.Elements.Add(islandD2b);
                #endregion

                #region island4
                PhysicTexture island4 = Setup.CreateWiggleIsland("island4",  Vec(13, -3f), 10);
                island4.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch4"), new Vector2(-1.9f, -2.7f), 0.5f));
                island4.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\fence3"), new Vector2(-1.32f,-2.859999f), 0.6f));
                this.Elements.Add(island4);
                #endregion

                #region islandF1
                PhysicTexture islandF1 = Setup.CreateWiggleIsland("islandF1",  Vec(13, -4.5f),5);
                islandF1.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch2"), new Vector2(-1.9f,-2.1f), 0.51f));
                islandF1.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\fence3"), new Vector2(0.9f,-2.1f), 0.52f));
                Setup.CreateCube(nVec(0, -2));
                this.Elements.Add(islandF1);
                #endregion

                #region islandN0_2
                PhysicTexture islandN0_2 = Setup.CreateWiggleIsland("islandN0", 0, Vec(-32, -2f));
                islandN0_2.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\fence2"), new Vector2(-2f,-2.47f), 0.71f));
                islandN0_2.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch2"), new Vector2(-1f,-2.3f), 0.51f));
                islandN0_2.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone4"), new Vector2(5.399997f,-2.899999f), 0.52f));
                this.Elements.Add(islandN0_2);
                #endregion

                #region island8_2
                PhysicTexture island8_2 = Setup.CreateWiggleIsland("island8", 10, Vec(18, -5f));
                island8_2.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\tree6"), new Vector2(-1.77f,-5.169997f), 0.5f));
                island8_2.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone3"), new Vector2(1.2f,-2.889999f), 0.53f));
                this.Elements.Add(island8_2);
                #endregion

                #region slide ps2
                this.Elements.Add(Setup.CreateBreakableCountdown("islandD1c_p", Vec(16, -6), 1000, 1100));

                Setup.CreateBreakable("islandO1_p",  nVec(6, -5f),
                                new Setup.AttachedGear(1, new Vector2(-3,-5)));

                this.Elements.Add(Setup.CreatePipe("pipe_middle", Vec(12, 3), 20));

                this.Elements.Add(Setup.CreatePipe("pipe_middle", Vec(8.3f, 3.04f), 20));

                this.Elements.Add(Setup.CreatePipe("pipe_middle", Vec(8.3f, 3.04f), 20));

                this.Elements.Add(Setup.CreatePipe("pipe_broken_right", Vec(6.1f, 2.2f), 20));
                #endregion

                #region islandW1_3
                PhysicTexture islandW1_3 = Setup.CreateTeeterIsland("islandW1", Vec(15, 5f), 2f, 0.2f);
                islandW1_3.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\fence3"), new Vector2(2f, -1.73f), 0.6f));
                islandW1_3.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch2"), new Vector2(-2.999999f, -1.7f), 0.5f));
                this.Elements.Add(islandW1_3);
                #endregion

                this.Elements.Add(Setup.CreateBreakableCountdown("islandD1c_p", Vec(16, -6), 1000, 900));

                Setup.CreateBreakable("islandO1_p", nVec(4, -5f),
                                    new Setup.AttachedGear(1, new Vector2(-1, 1)));

                #region island1b
                PhysicTexture island1b = Setup.CreateWiggleIsland("island1b", Vec(15, -0f));
                island1b.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone6"), new Vector2(-4.699998f,-3.799999f), 0.5f));
                island1b.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch3"), new Vector2(-2.4f,-2.899999f), 0.51f));
                island1b.AddAttachedElement(new AttachedElement(new CheckPoint(), new Vector2(2.3f, -4.83f), 0.5f));

                this.Elements.Add(island1b);
                #endregion

                #region island5
                PhysicTexture island5 = Setup.CreateWiggleIsland("island5", Vec(20, -4f));
                island5.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch2"), new Vector2(-1.1f,-2.04f), 0.51f));
                island5.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\tree1"), new Vector2(0.8000001f,-4.289996f), 0.52f));
                this.Elements.Add(island5);
                #endregion

                #region slide ps3
                this.Elements.Add(Setup.CreatePipe("pipe_middle", Vec(12, 3), 20));

                this.Elements.Add(Setup.CreatePipe("pipe_middle", Vec(8.3f, 3.04f), 20));

                this.Elements.Add(Setup.CreatePipe("pipe_middle", Vec(8.3f, 3.04f), 20));

                this.Elements.Add(Setup.CreatePipe("pipe_broken_right", Vec(6.1f, 2.2f), 20));
                #endregion

                #region islandW1_4
                PhysicTexture islandW1_4 = Setup.CreateTeeterIsland("islandW1", Vec(15, 5f), 2f, 0.2f);
                islandW1_4.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\fence3"), new Vector2(2f, -1.73f), 0.6f));
                islandW1_4.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch2"), new Vector2(-2.999999f, -1.7f), 0.5f));
                this.Elements.Add(islandW1_4);
                #endregion

                this.Elements.Add(Setup.CreateMovingIsland("metal_platform_small", Vec(15, -4), Vec(10, 0), 18000));

                #region island5
                PhysicTexture island5_1 = Setup.CreateWiggleIsland("island5", Vec(6, -4.5f), 10, 30);
                island5_1.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch2"), new Vector2(-1.1f,-2.04f), 0.51f));
                //island5_1.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\tree1"), new Vector2(0.8000001f,-4.289996f), 0.52f));
                this.Elements.Add(island5_1);
                #endregion

                this.Elements.Add(Setup.CreateBreakableCountdown("islandD1a_p", Vec(11, -2), 1000, 800));

                this.Elements.Add(Setup.CreateBreakableCountdown("islandD1b_p", Vec(11, 0), 1000, 800));

                #region islandW1_5
                PhysicTexture islandW1_5 = Setup.CreateTeeterIsland("islandW1", Vec(28, 6f), 2f, 0.2f);
                islandW1_5.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\fence3"), new Vector2(2f, -1.73f), 0.6f));
                islandW1_5.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch2"), new Vector2(-2.999999f, -1.7f), 0.5f));
                this.Elements.Add(islandW1_5);
                #endregion

                #region island10
                PhysicTexture island10 = Setup.CreateWiggleIsland("island10", nVec(20, -5f), 10, 0);
                island10.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch5"), new Vector2(-1.73f, -2.74f), 0.51f));
                island10.AddAttachedElement(new AttachedElement(new CheckPoint(), new Vector2(2.3f, -4.47f), 0.5f));
                this.Elements.Add(island10);
                #endregion 

                this.Elements.Add(Setup.CreateBreakable("island8_p", nVec(10, -10f),
                                        new Setup.AttachedGear(2, new Vector2(-2, 1))));

                this.Elements.Add(Setup.CreateWhipIsland("movingPlatformSmall", Vec(35, -3.5f), new Vector2(10, 2)));

                Setup.CreateBreakable("islandO2a_p", nVec(11, -1f),
                                        new Setup.AttachedGear(5, new Vector2(-0.5f, 2f)));

                this.Elements.Add(Setup.CreateBreakableCountdown("islandD1c_p", Vec(32, 0), 1000, 800));


                this.Elements.Add(Setup.CreateWhipIsland("movingPlatformSmall", Vec(12, -1f), new Vector2(10, 2)));

                Setup.CreateBreakable("islandO1_p", nVec(11, -1f),
                                        new Setup.AttachedGear(1, new Vector2(-1, 1)));

                this.Elements.Add(Setup.CreateBreakableCountdown("islandD1c_p", nVec(32, 1), 1000, 800));

                #region islandW1_6
                PhysicTexture islandW1_6 = Setup.CreateTeeterIsland("islandW1", Vec(50, 2f), 2f, 0.2f);
                islandW1_6.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\fence3"), new Vector2(2f, -1.73f), 0.6f));
                islandW1_6.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch2"), new Vector2(-2.999999f, -1.7f), 0.5f));
                this.Elements.Add(islandW1_6);
                #endregion

                Setup.CreateBreakable("island8_p", Vec(20, -15f),
                                        new Setup.AttachedGear(2, new Vector2(-2, 1)));
                Setup.CreateCube(nVec(0, -2));

                #region island9
                PhysicTexture island9 = Setup.CreateWiggleIsland("island9", Vec(0, 10f), 10, 0);
                island9.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone2"), new Vector2(-2.899999f,-3.199999f), 0.5f));
                island9.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch1"), new Vector2(-2f,-2.24f), 0.51f));
                this.Elements.Add(island9);
                #endregion

                #region IslandW1_7
                PhysicTexture islandW1_7 = Setup.CreateTeeterIsland("islandW1", Vec(25, -5f), 2f, 0.2f);
                islandW1_7.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\fence3"), new Vector2(2f, -1.73f), 0.6f));
                islandW1_7.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch2"), new Vector2(-2.999999f, -1.7f), 0.5f));
                this.Elements.Add(islandW1_7);
                #endregion

                #region Slide ps4
                this.Elements.Add(Setup.CreateMovingIsland("metal_platform_small", nVec(18, -5), nVec(32, -5), 9000));

                this.Elements.Add(Setup.CreatePipe("pipe_middle", Vec(70, -1), 20));

                this.Elements.Add(Setup.CreatePipe("pipe_middle", Vec(8.3f, 3.04f), 20));

                this.Elements.Add(Setup.CreatePipe("pipe_middle", Vec(8.3f, 3.04f), 20));

                this.Elements.Add(Setup.CreatePipe("pipe_broken_right", Vec(6.1f, 2.2f), 20));
                #endregion

                #region island9_2
                PhysicTexture island9_2 = Setup.CreateWiggleIsland("island9", Vec(15, 4f), 20, 0);
                island9_2.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone2"), new Vector2(-2.899999f,-3.199999f), 0.5f));
                island9_2.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch1"), new Vector2(-2f,-2.24f), 0.51f));
                island9_2.AddAttachedElement(new AttachedElement(new CheckPoint(), new Vector2(1.97f,-3.97f), 0.5f));
                this.Elements.Add(island9_2);
                #endregion

                this.Elements.Add(Setup.CreateBreakableCountdown("islandD1c_p", Vec(13, -1), 1000, 500));

                this.Elements.Add(Setup.CreateBreakableCountdown("islandD1a_p", Vec(10, -1), 1000, 500));

                this.Elements.Add(Setup.CreateBreakableCountdown("islandO1_p", Vec(10, 5), 1000, 500));

                Setup.CreateBreakable("islandO2b_p", Vec(8, -10f),
                                        new Setup.AttachedGear(5, new Vector2(-0.5f, 2f)));

                #region island4_2
                PhysicTexture island4_2 = Setup.CreateWiggleIsland("island4", Vec(11, 4f), 20, 0);
                island4_2.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch4"), new Vector2(-1.9f, -2.7f), 0.5f));
                island4_2.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\fence3"), new Vector2(-1.32f,-2.859999f), 0.6f));
                this.Elements.Add(island4_2);
                #endregion

                #region islandW1_8
                PhysicTexture islandW1_8 = Setup.CreateTeeterIsland("islandW1", Vec(21, -3f), 2f, 0.2f);
                this.Elements.Add(islandW1_8);
                #endregion

                #region Slide ps5
                this.Elements.Add(Setup.CreatePipe("pipe_middle", Vec(20, -3), 20));

                this.Elements.Add(Setup.CreatePipe("pipe_middle", Vec(8.3f, 3.04f), 20));

                this.Elements.Add(Setup.CreatePipe("pipe_middle", Vec(8.3f, 3.04f), 20));

                this.Elements.Add(Setup.CreatePipe("pipe_broken_right", Vec(6.1f, 2.2f), 20));
                #endregion

                #region island10_3
                PhysicTexture island10_3 = Setup.CreateWiggleIsland("island10", Vec(13, 4f), 20, 0);
                island10_3.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\tree4"), new Vector2(-3.799999f,-5.299997f), 0.5f));
                island10_3.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch5"), new Vector2(-1.73f,-2.74f), 0.51f));
                this.Elements.Add(island10_3);
                #endregion

                this.Elements.Add(Setup.CreateBreakableCountdown("islandD1a_p", Vec(14, -1), 1000, 4000));

                this.Elements.Add(Setup.CreateBreakableCountdown("islandD1c_p", Vec(7, -4), 1000, 1500));

                #region island10_4
                PhysicTexture island10_4 = Setup.CreateWiggleIsland("island10", Vec(13, -1f), 20, 0);
                island10_4.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\tree2"), new Vector2(-0f,-5.85f), 0.5f));
                island10_4.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch5"), new Vector2(-3f,-2.74f), 0.51f));
                this.Elements.Add(island10_4);
                #endregion

                this.Elements.Add(
                    new SuspensionBridge(
                        Vec(8f, -2f),
                        Vec(19, 0)
                    )
                );

                PhysicTexture iW20 = TikiEngine.Setup.CreateWiggleIsland(@"island10", Vec(8f, 2f), 1.00f, 0.00f);
                iW20.AddAttachedElement(new AttachedElement(new Sprite("Elements/levelstone"), new Vector2(1.119999f, -4.390007f), 0.5f));
                this.Elements.Add(iW20);


                //var bla = Setup.CreateWiggleIsland("islandasian", Vec(5.5f, 1f), 20, 0);
                //this.Elements.Add(bla);

                _tpNext = new TutorialPoint(
                    "Blueprint Scanned.\n" +
                    "You just find the last Worldblueprint.\n" +
                    "I knew you would do the job.\n" +
                    "I'm proud of you.\n" +
                    "World recreation process started.\n" +
                    "This will take a few centurys.\n" +
                    "Please wait.",
                    "end",
                    new Vector2(3, 3)
                );
                _tpNext.Started += (sender, e) =>
                {
                    Setup.Robo.AllowMove = false;
                };
                _tpNext.Finished += (sender, e) =>
                {
                    Program.Game.ChangeGameState<stateCredits>();
                };
                this.Elements.Add(_tpNext);

                FickDichTrigger trigger = new FickDichTrigger(iW20.Body);
                this.Events.Add(
                    new DoWhatIWantEvent(
                        trigger,
                        _tpNext.Start,
                        true
                    )
                );



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


        public override void Update(GameTime gameTime)
        {
            if(GI.Control.KeyboardPressed(Keys.NumPad0))
            {
            }
            if (GI.Control.KeyboardPressed(Keys.LeftControl) && this.tmpIsland == null)
            {
                this.tmpIsland =

                    //Setup.CreatePipe("pipe_broken_right", nVec(6.1f, 2.2f), 20);
                    //Setup.CreateMovingIsland("metal_platform_small", nVec(18, -5), nVec(32, -5), 9000);
                    Setup.CreateWiggleIsland("island10", nVec(13, 4f), 20, 0);
                    //Setup.CreateTeeterIsland("islandW1", nVec(33, 0f), 2f, 0.2f);

                    //Setup.CreateWhipIsland("islandF1", nVec(35, -3.5f), new Vector2(10,2));
                    //Setup.CreateBreakableCountdown("islandD1a_p", nVec(20, 2), 1000, 500);
                    //Setup.CreateBreakable("islandO1_p", Vec(-13, -3.5f),
                    //new Setup.AttachedGear(1, new Vector2(-1, 1)));

                this.Elements.Add(tmpIsland);

                    //Setup.CreateWiggleIsland("island7", -10 ,nVec(25, 0));

                    


                //PhysicTextureBreakable islandO1_p = Setup.CreateBreakable("islandO1_p", Vec(10, 2),
                //    new Setup.AttachedGear(1, new Vector2(-1, 1)));
                //this.Elements.Add(islandO1_p);

            }
            if (GI.Control.KeyboardPressed(Keys.RightControl) && this.tmpIsland != null)
            {
                this.tmpIsland.Dispose();
                this.Elements.Remove(tmpIsland);
                tmpIsland = null;
            }

            base.Update(gameTime);
        }

#if DEBUG
        public void _debug()
        {
            PhysicTexture p = new PhysicTexture("Particle/dot");
            p.AddAttachedElement(new AttachedElement(new Animation("Elements/debug_circlewings", 1, 12, 0, 11, 100, 3, true, true), Vector2.Zero, LayerDepthEnum.Debug));
            p.AddBehavior<behaviorDebug>();

            Elements.Add(p);
        }
#endif
    }
}
