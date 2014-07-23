using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TikiEngine.States;
using TikiEngine.Elements;

namespace TikiEngine
{
    public class GameMain : Game
    {
        #region Vars
        private bool _isLoading = false;
        private bool _isFullscreen = false;
        private Point _resetPoint = Point.Zero;

        private SpriteBatch _spriteBatch;
        private SpriteBatch _spriteBatchParallax;
        private SpriteBatch _spriteBatchInternal;
        private SpriteBatch _spriteBatchInterface;

        private PolygonBatch _polygonBatch;
        private ParticleBatch _particleBatch;
        private PostProcessingBatch _postProcessingBatch;

        private RenderTarget2D _screen;
        private GraphicsDeviceManager _graphics;

        private GameState _currentGameState;
        private List<GameState> _stateInstances = new List<GameState>();

        private Effect _postProcessingShader;
        #endregion

        #region Init
        public GameMain()
        {
#if DESIGNER
            _initDesigner();
#else
            _initGame();
#endif

            this.Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            GI.Init(this);

            _spriteBatch = GI.SpriteBatch;
            _spriteBatchParallax = GI.SpriteBatchParallax;
            _spriteBatchInterface = GI.SpriteBatchInterface;
            _spriteBatchInternal = new SpriteBatch(this.GraphicsDevice);

            _polygonBatch = GI.PolygonBatch;
            _particleBatch = GI.ParticleBatch;
            _postProcessingBatch = GI.PostProcessingBatch;
            
            _screen = new RenderTarget2D(GI.Device, GI.Device.Viewport.Width, GI.Device.Viewport.Height);

            Program.Config.Apply();

#if DEBUG
            this.ChangeGameState<stateMenu>();
#else
            this.ChangeGameState<stateIntro>();
#endif


            base.Initialize();
        }
        #endregion

        #region Init - Game
        private void _initGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 768;
            _graphics.IsFullScreen = false;
            _graphics.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
            _graphics.ApplyChanges();
        }
        #endregion

        #region Init - Designer
#if DESIGNER
        private System.Windows.Forms.Control _window;
        private TikiEngine.Levels.Designer.formMain _formMain;

        private void _initDesigner()
        {
            _formMain = new TikiEngine.Levels.Designer.formMain();
            _formMain.Show();
            Program.Form = _formMain;

            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = _formMain.panelViewport.Width;
            _graphics.PreferredBackBufferHeight = _formMain.panelViewport.Height;
            _graphics.PreparingDeviceSettings += (sender, e) =>
            {
                e.GraphicsDeviceInformation.PresentationParameters.IsFullScreen = false;
                e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = _formMain.panelViewport.Handle;
                e.GraphicsDeviceInformation.GraphicsProfile = GraphicsProfile.HiDef;
            };
            _graphics.ApplyChanges();

            _window = System.Windows.Forms.Control.FromHandle(this.Window.Handle);
            _window.VisibleChanged += (sender, e) =>
            {
                if (_window.Visible) _window.Visible = false;
            };

            Mouse.WindowHandle = _formMain.Handle;        
        }
#endif
        #endregion
        
        #region Member
        public void Reset(int width, int height, bool isFullscreen)
        {
            _isFullscreen = isFullscreen;
            _resetPoint = new Point(width, height);
        }
        #endregion

        #region Member - GameState
        public T GetGameState<T>()
            where T : GameState, new()
        {
            T gs = _stateInstances.OfType<T>().FirstOrDefault();

            if (gs == null)
            {
                gs = new T();

                _stateInstances.Add(gs);
            }

            return gs;
        }

        public T ChangeGameState<T>()
            where T : GameState, new()
        {
            T state = this.GetGameState<T>();

            _isLoading = typeof(T) == typeof(stateLoading);
            _currentGameState = state;
            _currentGameState.OnActivate();
            _currentGameState.ResetScreen();

            return state;
        }
        #endregion
        
        #region Member - Xna - Draw
        protected override void Draw(GameTime gameTime)
        {
#if !DEBUG
            try
            {
#endif
                if (_currentGameState.UseCamera)
                {
                    _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, null, null, null, null, GI.Camera.ViewMatrix2D);
                }
                else
                {
                    _spriteBatch.Begin();
                }
                _polygonBatch.Begin();
                _particleBatch.Begin();
                _postProcessingBatch.Begin();
                _spriteBatchInterface.Begin();
                _spriteBatchParallax.Begin(SpriteSortMode.FrontToBack, null);

                // Aktuelle Scene zeichnen
                _currentGameState.Draw(gameTime);
                GI.Mouse.Draw(gameTime);

                // Eingene Batches auf 'spriteBatch' zeichnen
                _polygonBatch.End();
                _particleBatch.End();
                _postProcessingBatch.End();

                // Auf das RenderTarget '_screen' zeichnen
                this.GraphicsDevice.SetRenderTarget(_screen);
                this.GraphicsDevice.Clear(_currentGameState.BackgroundColor);
                _spriteBatchParallax.End();
                _spriteBatch.End();
                _spriteBatchInterface.End();

                // RenderTarget '_screen' auf den Bildschirm zeichen
                this.GraphicsDevice.SetRenderTarget(null);
                _spriteBatchInternal.Begin();
                _spriteBatchInternal.Draw(_screen, Vector2.Zero, Microsoft.Xna.Framework.Color.White);
                _spriteBatchInternal.End();
#if !DEBUG
            }
            catch
            {
            }
#endif

            base.Draw(gameTime);

#if DESIGNER
            _drawRenderToImage(gameTime);
#endif
        }
        #endregion

        #region Member - Xna - Draw - RenderToImage
#if DESIGNER
        private List<NameObject> _renderToImage = new List<NameObject>();
        private Dictionary<NameObject, System.Drawing.Image> _renderToImageOutput = new Dictionary<NameObject, System.Drawing.Image>();

        private void _drawRenderToImage(GameTime gameTime)
        {
            if (_renderToImage.Count == 0) return;

            Dictionary<NameObject, MemoryStream> streams = new Dictionary<NameObject, MemoryStream>();

            float zoom = GI.Camera.RealZoom;
            Vector2 camPos = GI.Camera.CurrentPosition;
            GI.Camera.Update(gameTime);

            GI.Camera.RealZoom = 0.4f;
            GI.Camera.CurrentPosition = Vector2.Zero;

            foreach (NameObject no in _renderToImage.ToArray())
            {
                Vector2 pos = no.CurrentPosition;
                no.CurrentPosition = Vector2.Zero;

                RenderTarget2D target = new RenderTarget2D(this.GraphicsDevice, this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
                this.GraphicsDevice.SetRenderTarget(target);
                this.GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Transparent);

                _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, null, null, null, null, GI.Camera.ViewMatrix2D);
                no.Draw(gameTime);
                _spriteBatch.End();

                this.GraphicsDevice.SetRenderTarget(null);

                MemoryStream stream = new MemoryStream();
                target.SaveAsPng(stream, this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);
                target.Dispose();

                streams.Add(no, stream);

                no.CurrentPosition = pos;

                _renderToImageOutput[no] = null;
            }
            _renderToImage.Clear();

            this.GraphicsDevice.SetRenderTarget(null);

            GI.Camera.RealZoom = zoom;
            GI.Camera.CurrentPosition = camPos;
            GI.Camera.Update(gameTime);

            new System.Threading.Thread(_drawRenderToImageProgressStreams).Start(streams);
        }

        private void _drawRenderToImageProgressStreams(object list)
        {
            Dictionary<NameObject, MemoryStream> streams = (Dictionary<NameObject, MemoryStream>)list;

            foreach (var kvp in streams)
            {
                var img = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromStream(kvp.Value);
                kvp.Value.Dispose();

                int top = img.Height;
                int left = img.Width;
                int right = 0;
                int bottom = 0;
                
                int x = 0;
                int y = 0;
                int line = 0;
                bool lineEmpty = true;
                bool topDedected = false;
                int count = img.Width * img.Height;
                System.Drawing.Color color = System.Drawing.Color.Transparent;
                System.Drawing.Color colorT = System.Drawing.Color.Transparent;

                for (int i = 0; i < count; i++)
                {
                    x = i % img.Width;
                    y = i / img.Width;

                    if (line != y)
                    {
                        if (!lineEmpty && !topDedected)
                        {
                            top = y;
                            topDedected = true;
                        }

                        if (lineEmpty && topDedected)
                        {
                            bottom = y;
                            break;
                        }

                        lineEmpty = true;
                        line = y;
                    }

                    color = img.GetPixel(x, y);

                    if (color.A != 0)
                    {
                        if (x < left && lineEmpty)
                        {
                            left = x;
                        }

                        lineEmpty = false;

                        if (x > right && !lineEmpty)
                        {
                            right = x;
                        }
                    }
                }

                if (bottom == 0) bottom = img.Height;

                var r = new System.Drawing.Rectangle(
                    left,
                    top,
                    right - left,
                    bottom - top
                );

                System.Drawing.Image img2;
                if (r.Width > 1 && r.Height > 1)
                {
                    img2 = img.Clone(r, img.PixelFormat);
                    img.Dispose();
                }
                else
                {
                    img2 = img;
                }

                _renderToImageOutput[kvp.Key] = img2;
            }
        }

        public List<NameObject> RenderToImage
        {
            get { return _renderToImage; }
        }

        public Dictionary<NameObject, System.Drawing.Image> RenderToImageOutput
        {
            get { return _renderToImageOutput; }
        }
#endif
        #endregion

        #region Member - Xna - Update
        protected override void Update(GameTime gameTime)
        {
            if (_resetPoint != Point.Zero)
            {
                _graphics.IsFullScreen = _isFullscreen;
                _graphics.PreferredBackBufferWidth = _resetPoint.X;
                _graphics.PreferredBackBufferHeight = _resetPoint.Y;
                _resetPoint = Point.Zero;
                _graphics.ApplyChanges();

                _polygonBatch.Reset();
                _particleBatch.Reset();
                _postProcessingBatch.Reset();

                _screen.Dispose();
                _screen = new RenderTarget2D(GI.Device, GI.Device.Viewport.Width, GI.Device.Viewport.Height);

                GI.Camera.ResetScreen();
                Setup.ResetScreen();
                _currentGameState.ResetScreen();
            }

            if (!_isLoading)
            {
                GI.Mouse.Update(gameTime);
                GI.Sound.Update(gameTime);
                GI.Camera.Update(gameTime);
                GI.Control.Update(gameTime);
            }

            _currentGameState.Update(gameTime);

            if (GI.Control.KeyboardPressed(Keys.F11))
            {
                _graphics.IsFullScreen = !_graphics.IsFullScreen;
                _graphics.ApplyChanges();
            }

            if (GI.Control.KeyboardPressed(Keys.F12))
            {
                using (var stream = File.OpenWrite(DateTime.Now.ToString("s").Replace(':', '-') + ".jpg"))
                {
                    _screen.SaveAsJpeg(stream, _screen.Width, _screen.Height);
                }
            }

            if (GI.Control.KeyboardPressed(Keys.Escape) && !_isLoading)
            {
                this.ChangeGameState<stateMenu>();
            }

            base.Update(gameTime);
        }
        #endregion

        #region Properties
        public GameState CurrentGameState
        {
            get { return _currentGameState; }
            set
            {
                _currentGameState = value;
                _currentGameState.OnActivate();
            }
        }

        public Effect PostProcessingShader
        {
            get { return _postProcessingShader; }
            set
            {
                _postProcessingShader = value;

                if (_postProcessingShader != null)
                {
                    Matrix projection = Matrix.CreateOrthographicOffCenter(0, this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height, 0, 0, 1);
                    Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);

                    _postProcessingShader.Parameters["World"].SetValue(Matrix.Identity);
                    _postProcessingShader.Parameters["View"].SetValue(Matrix.Identity);
                    _postProcessingShader.Parameters["Projection"].SetValue(halfPixelOffset * projection);
                }
            }
        }
        #endregion
    }
}
