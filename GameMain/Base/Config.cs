using System;
using System.Runtime.Serialization;
using TikiEngine.Elements;

namespace TikiEngine
{
    [Serializable]
    public class Config : NameObject
    {
        #region Vars
        private int _screenWidth = 1024;
        private int _screenHeight = 768;
        private bool _screenWindowed = true;

        private float _audioMusic = 1f;
        private float _audioSound = 1f;
        private float _audioVoice = 1f;
        #endregion

        #region Init
        public Config()
        {
        }

        public Config(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { 
        }
        #endregion

        #region Member
        public void Apply()
        {
            GI.Voice.Volume = _audioVoice;
            GI.Sound.MusicVolume = _audioMusic;
            GI.Sound.SoundVolume = _audioSound;

            if (GI.Device.Viewport.Width != _screenWidth ||
                GI.Device.Viewport.Height != _screenHeight ||
                GI.Device.PresentationParameters.IsFullScreen != !_screenWindowed
            ) {
                Program.Game.Reset(
                    _screenWidth,
                    _screenHeight,
                    !_screenWindowed
                );
            }

            DataManager.SetObject(this);
        }
        #endregion

        #region Properties
        public override string Name
        {
            get { return "default"; }
            set { }
        }
        #endregion

        #region Properties - Config
        public int ScreenWidth
        {
            get { return _screenWidth; }
            set { _screenWidth = value; }
        }

        public int ScreenHeight
        {
            get { return _screenHeight; }
            set { _screenHeight = value; }
        }

        public bool ScreenWindowed
        {
            get { return _screenWindowed; }
            set { _screenWindowed = value; }
        }

        public float AudioMusic
        {
            get { return _audioMusic; }
            set { _audioMusic = value; }
        }

        public float AudioSound
        {
            get { return _audioSound; }
            set { _audioSound = value; }
        }

        public float AudioVoice
        {
            get { return _audioVoice; }
            set { _audioVoice = value; }
        }
        #endregion

        #region NameObject
        protected override void ApplyChanges()
        {
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
        }

        public override void Dispose()
        {
        }

        public override bool Ready
        {
            get { return true; }
        }
        #endregion
    }
}
