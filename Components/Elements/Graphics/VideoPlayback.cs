using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;

namespace TikiEngine.Elements.Graphics
{
    public class VideoPlayback : NameObjectGraphics
    {
        #region Vars
        private Video _video;
        private string _videoFile;

        private Vector2 _origin;

        private VideoPlayer _player;
        #endregion

        #region Init
        public VideoPlayback()
        {
            _player = new VideoPlayer();
        }
        #endregion

        #region Member
        public void Play()
        {
            if (_player.State == MediaState.Stopped)
            {
                _player.Play(_video);
            }
        }

        public void Stop()
        {
            if (_player.State != MediaState.Stopped)
            {
                _player.Stop();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (_player.State != MediaState.Stopped)
            {
                Texture2D tex = _player.GetTexture();
                
                spriteBatch.Draw(
                    tex,
                    positionCurrent,
                    null,
                    Color.White,
                    0.0f,
                    _origin,
                    1.0f,
                    SpriteEffects.None,
                    layerDepth
                );
            }
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Dispose()
        {            
        }

        protected override void ApplyChanges()
        {
        }
        #endregion

        #region Properties
        public override bool Ready
        {
            get { return _video != null; }
        }

        public bool IsLooped
        {
            get { return _player.IsLooped; }
            set { _player.IsLooped = value; }
        }

        public Video Video
        {
            get { return _video; }
            set
            {
                _video = value;

                _origin = new Vector2(_video.Width, _video.Height) / 2;
            }
        }

        public string VideoFile
        {
            get { return _videoFile; }
            set
            {
                _videoFile = value;

                this.Video = GI.Content.Load<Video>(value);
            }
        }
        #endregion
    }
}
