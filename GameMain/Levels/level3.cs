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
using TikiEngine.States;

namespace TikiEngine.Levels
{
    internal class level3 : LevelGame
    {
        #region Vars
        public const int PS_START_X = 280;
        public const int PS_START_Y = 0;
        public const int CUBE_WIDTH = 2;
        public const int CUBE_HEIGHT = 2;
        public const int JUMP_WIDTH = 6;
        public const int JUMP_HEIGHT = 2;
        public Vector2 psPos = new Vector2(0, 0);
        public Vector2 pos = new Vector2(0, 0);

        private bool _musicChanged;

        private TutorialPoint _tp1;
        private TutorialPoint _tpNext;
        #endregion

        public level3()
        {
            this.Zoom = 0.4f;

            this.Wolken();

            this.Robo.StartPosition = new Vector2(0, -6);
            //this.Robo.StartPosition = new Vector2(85, -20);
            //this.Robo.StartPosition = new Vector2(20, -12);

            //this.Robo.StartPosition = new Vector2(248, -40); //Slidesoundtesting
            //this.Robo.StartPosition = new Vector2(452, -36);
#if DEBUG
            _debug();
#endif   
        }
        public override void Initialize()
        {
            GI.Sound.AddLoopingSound(TikiSound.music_01, 0.3f);
            //GI.Sound.AddLoopingSound(TikiSound.music_02, 1f);
            GI.Sound.AddLoopingSound(TikiSound.Atmo, 0.5f);

            #region island1
            PhysicTexture island1 = Setup.CreateWiggleIsland("island1", Vec(0, 0));
            island1.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\tree2"), new Vector2(-2.29f,-5.949991f), 0.5f));
            island1.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\fence1"), new Vector2(2.1f,-2.9f), 0.5f));
            island1.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone3"), new Vector2(2.1f,-2.899999f), 0.5f));
            this.Elements.Add(island1);
            #endregion

            #region island2
            PhysicTexture island2 = Setup.CreateWiggleIsland("island2", Vec(20, -CUBE_HEIGHT - JUMP_HEIGHT - 1));
            island2.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone1"), new Vector2(-2.3f,-3.199999f), 0.52f));
            island2.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch5"), new Vector2(0.0199997f,-2.43f), 0.53f));
            this.Elements.Add(island2);
            #endregion

            PhysicTextureBreakable islandD1a = Setup.CreateBreakableCountdown("islandD1a_p", Vec(JUMP_WIDTH + 7, 0), 1000, 400);
            this.Elements.Add(islandD1a);
            PhysicTextureBreakable islandD1b = Setup.CreateBreakableCountdown("islandD1b_p", Vec(JUMP_WIDTH + 3, 0), 1000, 400);
            this.Elements.Add(islandD1b);

            PhysicTexture island5_3 = Setup.CreateWiggleIsland("island5", Vec(10, 0));
            island5_3.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\tree1"), new Vector2(0.8000001f, -4.289996f), 0.52f));
            island5_3.AddAttachedElement(new AttachedElement(new CheckPoint(), new Vector2(-1.079999f, -3.919997f), 0.53f));
            this.Elements.Add(island5_3);

            #region ilandW1
            PhysicTexture islandW1 = Setup.CreateTeeterIsland("islandW1", Vec(JUMP_WIDTH + 12, 0), 2f, 0.2f);
            islandW1.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone6"), new Vector2(3.909998f, -2.47f), 0.52f));
            this.Elements.Add(islandW1);

            Vector2 p1 = Vec(0, 0);
            #endregion

            #region island7
            PhysicTexture island7 = Setup.CreateWiggleIsland("island7", Vec(20, CUBE_HEIGHT + JUMP_HEIGHT + 2));
            island7.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\fence2"), new Vector2(-2.6f,-2.6f), 0.71f));
            island7.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch3"), new Vector2(2.1f,-2.5f), 0.52f));
            island7.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\tree3"), new Vector2(3.399999f,-5.600002f), 0.53f));
            island7.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone4"), new Vector2(-6.399996f,-3.209999f), 0.54f));
            this.Elements.Add(island7);
            #endregion

            #region island2_2
            PhysicTexture island2_2 = Setup.CreateWiggleIsland("island2", Vec(JUMP_WIDTH + 13, JUMP_HEIGHT));
            island2_2.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\tree1"), new Vector2(2.6f,-4.399986f), 0.51f));
            island2_2.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone1"), new Vector2(-2.3f,-3.199999f), 0.52f));
            this.Elements.Add(island2_2);
            #endregion

            #region Island6
            PhysicTexture island6 = Setup.CreateWiggleIsland("island6", Vec(JUMP_WIDTH + 13, 0));
            island6.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch4"), new Vector2(-2.999999f, -2.4f), 0.52f));
            this.Elements.Add(island6);
            #endregion

            #region Island8
            PhysicTexture island8 = Setup.CreateWiggleIsland("island8", Vec(JUMP_WIDTH + 13, JUMP_HEIGHT));
            island8.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch5"), new Vector2(3.499999f,-2.9f), 0.51f));
            island8.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone3"), new Vector2(-2.9f,-2.3f), 0.52f));
            this.Elements.Add(island8);
            #endregion


            setVec(p1);
            #region Island5
            PhysicTexture island5 = Setup.CreateWiggleIsland("island5", Vec(15, -7));
            island5.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch2"), new Vector2(-1.1f, -2.04f), 0.51f));
            this.Elements.Add(island5);
            #endregion

            #region IslandM1
            PhysicTexture islandM1 = Setup.CreateMovingIsland("metal_platform_small", Vec(JUMP_WIDTH + 2, 0), Vec(7, 0), 20000);
            islandM1.AddAttachedElement(new AttachedElement(new Sprite(@"Elements/chain_4096"), new Vector2(-3.909998f, -21.60005f), LayerDepthEnum.Foreground));
            islandM1.AddAttachedElement(new AttachedElement(new Sprite(@"Elements/chain_4096"), new Vector2(3.879998f, -21.60005f), LayerDepthEnum.Foreground));
            this.Elements.Add(islandM1);
            #endregion

            #region Island02b

            PhysicTexture islandO2b = Setup.CreateWiggleIsland("islandO2b", Vec(-1, -11));

            this.Elements.Add(islandO2b);
            #endregion

            PhysicTextureBreakable islandO1_p = Setup.CreateBreakable("islandO1_p", Vec(10, 2),
            new Setup.AttachedGear(1, new Vector2(-1, 1)));


            PhysicTexture movingPlatformSmall = Setup.CreateWiggleIsland("movingPlatform", Vec(11.5f, 6));
            this.Elements.Add(movingPlatformSmall);

            this.Elements.Add(new Riddle1(Vec(JUMP_WIDTH + 13.5f, 0), new Vector2(17.5f + Vec(0, 0).X, Vec(0, 0).Y+0.8f)));

            #region island02a
            PhysicTexture islandO2a = Setup.CreateWiggleIsland("islandO2a", Vec(JUMP_WIDTH, 5.5f));
            this.Elements.Add(islandO2a);
            #endregion

            Vector2 p2 = Vec(0, 0);

            #region island7_2
            PhysicTexture island7_2 = Setup.CreateWiggleIsland("island7", Vec(JUMP_WIDTH + 27, -3.5f));
            island7_2.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\fence2"), new Vector2(-2.6f,-2.6f), 0.71f));
            island7_2.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch3"), new Vector2(2.1f,-2.5f), 0.52f));
            island7_2.AddAttachedElement(new AttachedElement(new CheckPoint(), new Vector2(-0.4199999f, -4.260003f), 0.53f));
            this.Elements.Add(island7_2);
            #endregion

            Vector2 p3 = Vec(0, 0);

            #region IslandM1_2

            PhysicTexture islandM1_2 = Setup.CreateWiggleIsland("islandM1", Vec(JUMP_WIDTH + 8, +JUMP_HEIGHT + CUBE_HEIGHT));
            islandM1_2.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch1"), new Vector2(-1f,-2.41f), 0.52f));
            this.Elements.Add(islandM1_2);

            #endregion

            #region IslandN0

            PhysicTexture islandN0 = Setup.CreateWiggleIsland("islandN0", Vec(JUMP_WIDTH + 8, +JUMP_HEIGHT + CUBE_HEIGHT));
            islandN0.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\fence2"), new Vector2(-2f,-2.47f), 0.71f));
            islandN0.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch2"), new Vector2(-1f,-2.3f), 0.51f));
            this.Elements.Add(islandN0);

            #endregion

            #region Island01

            PhysicTexture islandO1 = Setup.CreateWiggleIsland("islandO1", Vec(JUMP_WIDTH + 9, +JUMP_HEIGHT + CUBE_HEIGHT));
            this.Elements.Add(islandO1);

            #endregion

            setVec(p3);

            #region IslandF1

            PhysicTexture islandF1 = Setup.CreateWiggleIsland("islandF1", Vec(JUMP_WIDTH + 8, -JUMP_HEIGHT - CUBE_HEIGHT - 1));
            islandF1.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch2"), new Vector2(-1.9f,-2.1f), 0.51f));
            islandF1.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\fence3"), new Vector2(0.9f,-2.1f), 0.52f));
            this.Elements.Add(islandF1);

            #endregion

            #region islandW1_3

            PhysicTexture islandW1_3 = Setup.CreateTeeterIsland("islandW1", Vec(JUMP_WIDTH + 12, 0), 2f, 1f);
            islandW1_3.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch4"), new Vector2(-2.799999f, -1.8f), 0.51f));
            this.Elements.Add(islandW1_3);

            PhysicTexture islandM1_9 = Setup.CreateWiggleIsland("islandM1", Vec(JUMP_WIDTH + 9, -2));
            islandM1_9.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\tree4"), new Vector2(-2.7f, -4.840003f), 0.51f));
            this.Elements.Add(islandM1_9);

            Riddle4 riddle4 = new Riddle4(Vec(JUMP_WIDTH+0.5f, -1.5f), Vec(JUMP_WIDTH -0.5f, 0), 30, 30);
            this.Elements.Add(riddle4);

            #endregion

            #region Island2_3
            PhysicTexture island2_3 = Setup.CreateWiggleIsland("island2", Vec(JUMP_WIDTH + 6, -10));
            island2_3.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\tree1"), new Vector2(2.6f,-4.399986f), 0.51f));
            island2_3.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone1"), new Vector2(-2.3f, -3.199999f), 0.52f));
            island2_3.AddAttachedElement(new AttachedElement(new CheckPoint(), new Vector2(1.399999f, -3.999997f), 0.53f));

            this.Elements.Add(island2_3);

            #endregion

            PhysicTexture pipe_1_start = Setup.CreatePipe("pipe_start", Vec(15, 5), 0);
            this.Elements.Add(pipe_1_start);

            PhysicTexture pipe_1_1 = Setup.CreatePipe("pipe_middle", Vec(5.9f, 1.65f), 20);
            pipe_1_start.Body.IgnoreCollisionWith(pipe_1_1.Body);
            pipe_1_1.Body.IgnoreCollisionWith(pipe_1_start.Body);
            this.Elements.Add(pipe_1_1);

            PhysicTexture pipe_1_2 = Setup.CreatePipe("pipe_middle", Vec(8.5f, 3.13f), 20);
            this.Elements.Add(pipe_1_2);

            PhysicTexture pipe_1_3 = Setup.CreatePipe("pipe_middle", Vec(8.5f, 3.13f), 20);
            this.Elements.Add(pipe_1_3);

            //PhysicTexture pipe_1_end = Setup.CreatePipe("pipe_broken_left", Vec(6.2f, 2.5f), -160);
            //pipe_1_3.Body.IgnoreCollisionWith(pipe_1_end.Body);
            //pipe_1_end.Body.IgnoreCollisionWith(pipe_1_3.Body);
            //this.Elements.Add(pipe_1_end);

            PhysicTexture slide3 = Setup.CreatePipe("pipe_middle", Vec(20, 8), 20);
            this.Elements.Add(slide3);

            PhysicTexture slide4 = Setup.CreatePipe("pipe_middle", Vec(8.5f, 3.13f), 20);
            this.Elements.Add(slide4);

            #region Island1_5

            PhysicTexture island1_5 = Setup.CreateWiggleIsland("island1", Vec(16, 6));
            island1_5.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\tree2"), new Vector2(-2.29f,-5.949991f), 0.51f));
            island1_5.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\fence1"), new Vector2(2.1f,-2.9f), 0.71f));
            this.Elements.Add(island1_5);

            #endregion
            //PhysicTexture movingPlatformSmall_4 = Setup.CreateWhipIsland("movingPlatformSmall", Vec(15, -2), new Vector2(15, 0));
            this.Elements.Add(new NewtonIsland(Vec(28, -2), 10.5f, 13));

            Vector2 p4 = Vec(0, 0);

            #region Island7_3

            PhysicTexture island7_3 = Setup.CreateWiggleIsland("island7", Vec(-13, 16));
            island7_3.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone4"), new Vector2(-6.399996f,-3.209999f), 0.54f));
            this.Elements.Add(island7_3);

            #endregion


            PhysicTextureBreakable islandO1_p_2 = Setup.CreateBreakable("islandO1_p", Vec(-17, -2),

            new Setup.AttachedGear(1, new Vector2(1, -1))
            );


            setVec(p4);

            #region IslandN0_2

            PhysicTexture islandN0_2 = Setup.CreateWiggleIsland("islandN0", Vec(24, 16));
            islandN0_2.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch2"), new Vector2(-1f,-2.3f), 0.51f));
            islandN0_2.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone4"), new Vector2(5.399997f,-2.899999f), 0.52f));
            this.Elements.Add(islandN0_2);

            #endregion

            #region Island1_6

            PhysicTexture island1_6 = Setup.CreateWiggleIsland("island2", Vec(15, JUMP_HEIGHT));
            island1_6.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch1"), new Vector2(0.5f,-2.42f), 0.51f));
            this.Elements.Add(island1_6);

            #endregion

            #region IslandM1_3

            PhysicTexture islandM1_3 = Setup.CreateMovingIsland("islandM1", Vec(JUMP_WIDTH + 7, 0), Vec(5, 0), 15000);
            islandM1_3.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\tree4"), new Vector2(-2.7f, -4.840003f), 0.51f));
            this.Elements.Add(islandM1_3);

            #endregion

            PhysicTextureBreakable islandO2a_p = Setup.CreateBreakable("islandO2a_p", Vec(1, -9));


            #region Island02b_2

            PhysicTexture islandO2b_2 = Setup.CreateWiggleIsland("islandO2b", Vec(10, -2));
            this.Elements.Add(islandO2b_2);

            #endregion

            setVec(p4);
            //Vec(-20, 0);

            #region IslandF1_2

            PhysicTexture islandF1_2 = Setup.CreateWiggleIsland("islandF1", Vec(34, 0));
            islandF1_2.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch2"), new Vector2(-1.9f, -2.1f), 0.51f));
            this.Elements.Add(islandF1_2);

            #endregion

            #region Island5_2

            PhysicTexture island5_2 = Setup.CreateWiggleIsland("island8", Vec(15, -JUMP_HEIGHT + 2));
            CubeTrigger ct = new CubeTrigger();
            island5_2.AddAttachedElement(new AttachedElement(ct, new Vector2(5.09f, -2.53f), LayerDepthEnum.IslandBackground));
            island5_2.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone3"), new Vector2(-2.9f,-2.3f), 0.52f));
            this.Elements.Add(island5_2);

            #endregion

            Riddle2 riddle = new Riddle2ext(Vec(17, -JUMP_HEIGHT), -15);
            riddle.Trigger = ct;
            this.Elements.Add(riddle);

            #region Island5_15

            PhysicTexture island5_15 = Setup.CreateWiggleIsland("island5", Vec(13, -JUMP_HEIGHT - 15));
            this.Elements.Add(island5_15);

            #endregion

            #region Island7_4

            PhysicTexture island7_4 = Setup.CreateWiggleIsland("island7", Vec(15, -JUMP_HEIGHT));
            island7_4.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\tree3"), new Vector2(3.399999f,-5.600002f), 0.53f));
            island7_4.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone4"), new Vector2(-6.399996f,-3.209999f), 0.54f));
            island7_4.AddAttachedElement(new AttachedElement(new CheckPoint(), new Vector2(0.7800013f, -4.260003f), 0.55f));
            this.Elements.Add(island7_4);

            #endregion;

            PhysicTexture pipe_3_start = Setup.CreatePipe("pipe_start", Vec(7, 0), 0);
            pipe_3_start.LayerDepth = 0.5f;
            pipe_3_start.Body.IgnoreCollisionWith(island7_4.Body);
            this.Elements.Add(pipe_3_start);

            PhysicTexture pipe_3_1 = Setup.CreatePipe("pipe_middle", Vec(5.9f, 1.65f), 20);
            pipe_3_start.Body.IgnoreCollisionWith(pipe_3_1.Body);
            pipe_3_1.Body.IgnoreCollisionWith(pipe_3_start.Body);
            this.Elements.Add(pipe_3_1);

            PhysicTexture pipe_3_3 = Setup.CreatePipe("pipe_middle", Vec(8.5f, 3.13f), 20);
            this.Elements.Add(pipe_3_3);

            //PhysicTexture pipe_1_end = Setup.CreatePipe("pipe_broken_left", Vec(6.2f, 2.5f), -160);
            //pipe_1_3.Body.IgnoreCollisionWith(pipe_1_end.Body);
            //pipe_1_end.Body.IgnoreCollisionWith(pipe_1_3.Body);
            //this.Elements.Add(pipe_1_end);

            PhysicTexture pipe_4_1 = Setup.CreatePipe("pipe_middle", Vec(20, 8), 20);
            this.Elements.Add(pipe_4_1);

            PhysicTexture pipe_4_2 = Setup.CreatePipe("pipe_middle", Vec(8.5f, 3.13f), 20);
            this.Elements.Add(pipe_4_2);

            PhysicTexture pipe_4_3 = Setup.CreatePipe("pipe_middle", Vec(8.5f, 3.13f), 20);
            this.Elements.Add(pipe_4_3);

            PhysicTexture pipe_5_1 = Setup.CreatePipe("pipe_middle", Vec(25, 6), -20);
            this.Elements.Add(pipe_5_1);

            PhysicTexture pipe_5_2 = Setup.CreatePipe("pipe_middle", Vec(-8.5f, 3.13f), -20);
            this.Elements.Add(pipe_5_2);

            PhysicTexture pipe_5_3 = Setup.CreatePipe("pipe_middle", Vec(-8.5f, 3.13f), -20);
            this.Elements.Add(pipe_5_3);

            PhysicTexture pipe_6_1 = Setup.CreatePipe("pipe_middle", Vec(-21, 9), -20);
            this.Elements.Add(pipe_6_1);
            Vector2 p5 = Vec(0, 0);

            #region Island8_15

            PhysicTexture island8_15 = Setup.CreateWiggleIsland("island8", Vec(-20, 8));
            CubeTrigger ct2 = new CubeTrigger();
            island8_15.AddAttachedElement(new AttachedElement(ct2, new Vector2(5.09f, -2.53f), LayerDepthEnum.IslandBackground));
            island8_15.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch5"), new Vector2(3.499999f,-2.9f), 0.51f));
            island8_15.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone3"), new Vector2(-2.9f,-2.3f), 0.52f));
            this.Elements.Add(island8_15);

            #endregion

            Riddle2 riddle2_15 = new Riddle2ext(Vec(-2, -20), 16);
            riddle2_15.Trigger = ct2;
            this.Elements.Add(riddle2_15);

            #region Island1_15

            PhysicTexture island1_15 = Setup.CreateWiggleIsland("island7", Vec(JUMP_WIDTH + 9, 3));
            island1_15.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\fence2"), new Vector2(-2.6f,-2.6f), 0.71f));
            island1_15.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch3"), new Vector2(2.1f,-2.5f), 0.52f));
            this.Elements.Add(island1_15);

            #endregion


            PhysicTextureBreakable islandO2b_15 = Setup.CreateBreakable("islandO2b_p", Vec(-25, 5), new Setup.AttachedGear(2, new Vector2(0.6f, 1)));


            setVec(p5);

            PhysicTexture pipe_7_1 = Setup.CreatePipe("pipe_middle", Vec(10, 0), 20);
            this.Elements.Add(pipe_7_1);

            PhysicTexture pipe_7_2 = Setup.CreatePipe("pipe_middle", Vec(8.5f, 3.13f), 20);
            this.Elements.Add(pipe_7_2);

            #region Island7_15

            PhysicTexture island7_15 = Setup.CreateWiggleIsland("island7", Vec(15, 10));
            this.Elements.Add(island7_15);


            #endregion

            #region Island02b_16

            PhysicTexture islandO2b_16 = Setup.CreateWiggleIsland("islandO2b", Vec(JUMP_WIDTH + 10, JUMP_HEIGHT + CUBE_HEIGHT + 2));
            islandO2b_16.AddAttachedElement(new AttachedElement(new CheckPoint(), new Vector2(0.56f,-6.19f), LayerDepthEnum.IslandBackground));
            this.Elements.Add(islandO2b_16);

            #endregion

            #region move_16

            PhysicTexture move_16 = Setup.CreateMovingIsland("metal_platform_small", Vec(JUMP_WIDTH + 3, -2), new Vector2(Vec(0, 0).X + 7, Vec(0, 0).Y), 35000);
            move_16.AddAttachedElement(new AttachedElement(new Sprite(@"Elements/chain_4096"), new Vector2(-3.909998f, -21.60005f), LayerDepthEnum.Foreground));
            move_16.AddAttachedElement(new AttachedElement(new Sprite(@"Elements/chain_4096"), new Vector2(3.879998f, -21.60005f), LayerDepthEnum.Foreground));
            this.Elements.Add(move_16);

            #endregion

            #region island_5_16

            PhysicTexture island_5_16 = Setup.CreateWiggleIsland("island5", Vec(8, -8));
            this.Elements.Add(island_5_16);

            #endregion

            PhysicTextureBreakable islandO2c_16 = Setup.CreateBreakable("islandO2b_p", Vec(8, 0.7f), new Setup.AttachedGear(1, new Vector2(-0.6f, 1)));


            #region movingPlatform_16

            PhysicTexture movingPlatform_16 = Setup.CreateWiggleIsland("movingPlatform", Vec(10, 5.5f));
            this.Elements.Add(movingPlatform_16);

#endregion

            PhysicTextureBreakable islandD1a_16 = Setup.CreateBreakableCountdown("islandD1a_p", Vec(JUMP_WIDTH + 5, -1.5f), 1000, 1100);
            this.Elements.Add(islandD1a_16);

            PhysicTextureBreakable islandD1b_16 = Setup.CreateBreakableCountdown("islandD1b_p", Vec(JUMP_WIDTH + 5, 0), 1000, 1100);
            this.Elements.Add(islandD1b_16);

            PhysicTexture island_2_17 = Setup.CreateWiggleIsland("island2", Vec(JUMP_WIDTH + 29, 4));
            island_2_17.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch1"), new Vector2(0.5f, -2.42f), 0.51f));
            this.Elements.Add(island_2_17);

            Riddle4 riddle4_17 = new Riddle4(Vec(JUMP_WIDTH + 3, -1.5f), Vec(JUMP_WIDTH +8, 0), 20, 20);
            this.Elements.Add(riddle4_17);

            PhysicTexture island_5_17 = Setup.CreateWiggleIsland("island5", Vec(JUMP_WIDTH -13, 0));
            island_5_17.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch2"), new Vector2(-1.1f, -2.04f), 0.51f));
            this.Elements.Add(island_5_17);

            #region island_1_18

            PhysicTexture island_1_18 = Setup.CreateWiggleIsland("island1", Vec(JUMP_WIDTH +15, -7));
            island_1_18.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\tree2"), new Vector2(-2.29f, -5.949991f), 0.51f));
            island_1_18.AddAttachedElement(new AttachedElement(new CheckPoint(), new Vector2(0.4f, -4.589995f), 0.53f));
            this.Elements.Add(island_1_18);

            #endregion

            PhysicTextureBreakable islandD1c_18 = Setup.CreateBreakableCountdown("islandD1c_p", Vec(JUMP_WIDTH + 9, -JUMP_HEIGHT - 1), 1000, 400);
            this.Elements.Add(islandD1c_18);

            #region island_2_18

            PhysicTexture island_2_18 = Setup.CreateWiggleIsland("island2", Vec(JUMP_WIDTH + 9, -JUMP_HEIGHT));
            island_2_18.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone1"), new Vector2(-2.3f, -3.199999f), 0.52f));
            this.Elements.Add(island_2_18);

            #endregion

            PhysicTextureBreakable islandD1a_18 = Setup.CreateBreakableCountdown("islandD1a_p", Vec(JUMP_WIDTH + 8, -JUMP_HEIGHT), 1000, 400);
            this.Elements.Add(islandD1a_18);

            PhysicTexture pipe_8_start = Setup.CreatePipe("pipe_start", Vec(10, 0), 0);
            this.Elements.Add(pipe_8_start);

            PhysicTexture pipe_8_1 = Setup.CreatePipe("pipe_middle", Vec(5.9f, 1.65f), 20);
            pipe_8_start.Body.IgnoreCollisionWith(pipe_8_1.Body);
            pipe_8_1.Body.IgnoreCollisionWith(pipe_8_start.Body);
            this.Elements.Add(pipe_8_1);

            PhysicTexture pipe_8_3 = Setup.CreatePipe("pipe_middle", Vec(8.5f, 3.13f), 20);
            this.Elements.Add(pipe_8_3);

            PhysicTextureBreakable islandO1_19 = Setup.CreateBreakable(
                "islandO1_p",
                Vec(7, 9),
                new Setup.AttachedGear(10, new Vector2(1.6f, -6.5f), LayerDepthEnum.IslandBackground));

            PhysicTexture pipe_9_1 = Setup.CreatePipe("pipe_middle", Vec(6, 4), 20);
            this.Elements.Add(pipe_9_1);

            PhysicTexture pipe_9_2 = Setup.CreatePipe("pipe_middle", Vec(8.5f, 3.13f), 20);
            this.Elements.Add(pipe_9_2);

            #region islandN0_20

            PhysicTexture islandN0_20 = Setup.CreateWiggleIsland("islandN0", Vec(15, 3));
            islandN0_20.AddAttachedElement(new AttachedElement(new CheckPoint(), new Vector2(1.8f, -4.199998f), LayerDepthEnum.IslandBackground));
            this.Elements.Add(islandN0_20);

            #endregion

            #region islandW1_20

            PhysicTexture islandW1_20 = Setup.CreateTeeterIsland("islandW1", Vec(JUMP_WIDTH + 15, -1), 2f, 0.2f);
            islandW1_20.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\fence3"), new Vector2(2f,-1.73f), 0.6f));
            islandW1_20.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch2"), new Vector2(-2.999999f,-1.7f), 0.5f));
            this.Elements.Add(islandW1_20);

            #endregion

            Vector2 p6 = Vec(0, 0);

            #region Island8_20

            PhysicTexture island8_20 = Setup.CreateWiggleIsland("island8", Vec(10, 10));
            CubeTrigger ct3 = new CubeTrigger();
            island8_20.AddAttachedElement(new AttachedElement(ct3, new Vector2(5.09f, -2.53f), LayerDepthEnum.IslandBackground));
            island8_20.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\tree6"), new Vector2(-1.77f,-5.169997f), 0.5f));
            island8_20.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone3"), new Vector2(1.2f,-2.889999f), 0.53f));
            this.Elements.Add(island8_20);

            #endregion

            Riddle2 riddle2_20 = new Riddle2ext(Vec(15, 0), -13);
            riddle2_20.Trigger = ct3;
            this.Elements.Add(riddle2_20);

            PhysicTextureBreakable islandD1c_20 = Setup.CreateBreakable(
                "islandD1c_p",
                Vec(-30, -6.5f),
                new Setup.AttachedGear(1, new Vector2(-0.6f, 1)));

            setVec(p6);
            //Vec(0, 8); // Test

            #region Island7_21

            PhysicTexture island7_21 = Setup.CreateWiggleIsland("island7", Vec(20, -8));
            island7_21.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\fence2"), new Vector2(-2.6f,-2.6f), 0.71f));
            island7_21.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch3"), new Vector2(2.1f,-2.5f), 0.52f));
            this.Elements.Add(island7_21);

            #endregion

            PhysicTexture movingPlatform_21 = Setup.CreateWhipIsland("movingPlatformSmall", Vec(10, 0), new Vector2(13, 0));
            this.Elements.Add(movingPlatform_21);

            PhysicTextureBreakable islandO2a_21 = Setup.CreateBreakable(
                "islandO2a_p",
                Vec(13, 5),
                new Setup.AttachedGear(1, new Vector2(-1, 2)));

            ////Vec(-7, 5); // Test

            #region Island1_21

            PhysicTexture island1_21 = Setup.CreateWiggleIsland("island1", Vec(25, -6));
            island1_21.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\fence1"), new Vector2(2.1f,-2.9f), 0.71f));
            island1_21.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone3"), new Vector2(2.1f, -2.899999f), 0.52f));
            this.Elements.Add(island1_21);

            #endregion

            #region Island_N0_22

            PhysicTexture islandN0_22 = Setup.CreateWiggleIsland("islandN0", Vec(JUMP_WIDTH + 14, -JUMP_HEIGHT));
            islandN0_22.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch2"), new Vector2(-1f,-2.3f), 0.51f));
            islandN0_22.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone4"), new Vector2(5.399997f,-2.899999f), 0.52f));
            this.Elements.Add(islandN0_22);

            #endregion

            #region island8_23

            PhysicTexture island8_23 = Setup.CreateWiggleIsland("island8", Vec(14, -JUMP_HEIGHT - CUBE_HEIGHT));
            island8_23.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\stone3"), new Vector2(1.2f,-2.889999f), 0.53f));
            island8_23.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch5"), new Vector2(3.499999f,-2.9f), 0.51f));
            island8_23.AddAttachedElement(new AttachedElement(new Sprite("Elements/levelstone"), new Vector2(1.519999f, -4.390007f), 0.54f));
            this.Elements.Add(island8_23);

            #endregion

            //Vector2 p7 = Vec(0, 0);

            #region islandF1_23

            PhysicTexture islandF1_23 = Setup.CreateWiggleIsland("islandF1", Vec(-15, -JUMP_HEIGHT - CUBE_HEIGHT));
            islandF1_23.AddAttachedElement(new AttachedElement(new Sprite(@"Environment\busch2"), new Vector2(-1.9f,-2.1f), 0.51f));
            this.Elements.Add(islandF1_23);

            #endregion

            ////setVec(p7);

            //#region Island7

            //PhysicTexture island7 = Setup.CreateWiggleIsland("island7", Vec(20, -JUMP_HEIGHT - (CUBE_HEIGHT * 2)));


            //this.Elements.Add(island7);

            //#endregion


            //Relative Point Position : {X:7,569995 Y:-1,57}


            #region Trigger
            _tp1 = new TutorialPoint(
                "Buildsystem reactivated.\n" +
                "Rightclick near the character to build a cube.\n" + 
                "If you hold the right mouse button, you will see your buildradius.",
                "build",
                new Vector2(3, 3)
            );
            this.Elements.Add(_tp1);

            _tpNext = new TutorialPoint(
                "Blueprint Scanned.\n" +
                "Great. Only one part left to reconstruct the World.",
                "end",
                new Vector2(3, 3)
            );
            _tpNext.Started += (sender, e) =>
            {
                Setup.Robo.AllowMove = false;
            };
            _tpNext.Finished += (sender, e) =>
            {
                Program.Game.ChangeGameState<stateLevel>().SetLevel<level4>();
            };
            this.Elements.Add(_tpNext);

            FickDichTrigger trigger = new FickDichTrigger(island1.Body);
            this.Events.Add(
                new DoWhatIWantEvent(
                    trigger,
                    _tp1.Start,
                    true
                )
            );

            trigger = new FickDichTrigger(island8_23.Body);
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
            pos.X += x;
            pos.Y += y;
            return new Vector2(pos.X, pos.Y);
        }
        public Vector2 setVec(Vector2 vec)
        {
            return setVec(vec.X, vec.Y);
        }
        public Vector2 setVec(float x, float y)
        {
            pos.X = x;
            pos.Y = y;
            return new Vector2(pos.X, pos.Y);
        }

        public Vector2 psVec(int x, int y)
        {
            float factor = 16f;
            psPos.X += x;
            psPos.Y += y;
            return new Vector2((psPos.X) / factor, (psPos.Y + PS_START_Y) / factor);
        }
        public override void Update(GameTime gameTime)
        {
            if (!_musicChanged && gameTime.TotalGameTime.TotalSeconds > 15)
            {
                //GI.Sound.PlayMusic(SoundMusic.Music_02);
                _musicChanged = true;
            }

            base.Update(gameTime);
        }

#if DEBUG
        public void _debug()
        {
            PhysicTexture p = new PhysicTexture("Particle/dot");
            p.AddAttachedElement(new AttachedElement(new Animation("Elements/debug_circlewings",1,12,0,11,100,3,true,true),Vector2.Zero,LayerDepthEnum.Debug));
            p.AddBehavior<behaviorDebug>();

            Elements.Add(p);
        }
#endif
    }
}
