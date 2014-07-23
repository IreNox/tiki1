using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using TikiEngine.Elements;
using TikiEngine.Components;
using TikiEngine.Elements.Physic;
using FarseerPhysics.Common;

namespace TikiEngine
{
    public static class GI
    {
        #region Vars
        private static Game _game;

        private static SpriteBatch _spriteBatch;
        private static SpriteBatch _spriteBatchParallax;
        private static SpriteBatch _spriteBatchInterface;

        private static PolygonBatch _polygonBatch;
        private static ParticleBatch _particleBatch;
        private static PostProcessingBatch _postProcessingBatch;

        private static GraphicsDevice _device;
        private static ContentManager _content;

        private static MouseComponent _mouse;
        private static SoundComponent _sound;
        private static CameraComponent _camera;
        private static ControlComponent _control;

        private static World _world;
        private static Level _level;

        private static RotatedRectangle roboBounding;

        private static Dictionary<string, object> _gameVars;

        private static MultiVoice voice;
        #endregion

        #region Init
        public static void Init(Game game)
        {
            _game = game;
            _content = game.Content;
            _device = game.GraphicsDevice;

            roboBounding = new RotatedRectangle(new Rectangle(0,0,160,240));

            _spriteBatch = new SpriteBatch(_device);
            _spriteBatchParallax = new SpriteBatch(_device);
            _spriteBatchInterface = new SpriteBatch(_device);

            _mouse = new MouseComponent();
            _sound = new SoundComponent();
            _camera = new CameraComponent();
            _control = new ControlComponent();

            _polygonBatch = new PolygonBatch(_device);
            _particleBatch = new ParticleBatch(_device);
            _postProcessingBatch = new PostProcessingBatch(_device);

            _gameVars = new Dictionary<string, object>();

            voice = new MultiVoice();

            _world = new World(
                new Vector2(0.0f, 9.81f)
            );
        }
        #endregion

        #region Properties
#if DEBUG
        public static bool DEBUG { get; set; }
#endif

        public static Game Game
        {
            get { return _game; }
        }

        public static MouseComponent Mouse
        {
            get { return _mouse; }
        }

        public static SoundComponent Sound
        {
            get { return _sound; }
        }

        public static CameraComponent Camera
        {
            get { return _camera; }
        }

        public static ControlComponent Control
        {
            get { return _control; }
        }
        
        public static GraphicsDevice Device
        {
            get { return _device; }
        }

        public static ContentManager Content
        {
            get { return _content; }
        }

        public static MultiVoice Voice
        {
            get { return GI.voice; }
        }
        #endregion

        #region Propertys - Game
        public static Level Level
        {
            get { return _level; }
            set { _level = value; }
        }

        public static World World
        {
            get { return _world; }
            set { _world = value; }
        }

        public static Dictionary<string, object> GameVars
        {
            get { return _gameVars; }
            set { _gameVars = value; }
        }

        public static RotatedRectangle RoboBounding
        {
            get { return roboBounding; }
            set { roboBounding = value; }
        }
        #endregion

        #region Properties - Batches
        public static SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
        }

        public static SpriteBatch SpriteBatchParallax
        {
            get { return _spriteBatchParallax; }
        }

        public static SpriteBatch SpriteBatchInterface
        {
            get { return _spriteBatchInterface; }
        }

        public static PolygonBatch PolygonBatch
        {
            get { return _polygonBatch; }
        }

        public static ParticleBatch ParticleBatch
        {
            get { return _particleBatch; }
        }

        public static PostProcessingBatch PostProcessingBatch
        {
            get { return _postProcessingBatch; }
        }
        #endregion
    }
}
