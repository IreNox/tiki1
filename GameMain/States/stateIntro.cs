using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TikiEngine.Elements.Controls;
using TikiEngine.Components;
using TikiEngine.Elements.Particle;
using TikiEngine.Elements.Graphics;

namespace TikiEngine.States
{
    class stateIntro : GameState
    {
        #region Vars
        private double _time;

        private bool _switch;

        private Sprite _spriteGA;
        private Sprite _spriteGame;
        private Sprite _spritePhysic;

        private modifierInterpolator _interGA;

        private VideoPlayback _video;
        #endregion

        #region Init
        public stateIntro()
        {            
            this.UseCamera = false;
            this.BackgroundColor = Color.Black;
        }

        public override void Initialize()
        {
            //effectMask effect = new effectMask();
            //effect.Size = (new Vector2(619, 1024) / 1024) * GI.Device.Viewport.Height;
            //effect.TextureMaskFile = "Base/tikitek_big";
            //this.SetComponent("tikitek", effect);

            effectMask effect = new effectMask();
            effect.Size = (new Vector2(1280, 1024) / 1024) * GI.Device.Viewport.Height;
            effect.TextureMaskFile = "Base/ga-logo";
            effect.IsAlive = false;
            this.SetComponent("ga", effect);

            _spriteGA = new Sprite("base/ga-logo2");
            this.SetComponent(_spriteGA);

            _interGA = new modifierInterpolator()
            {
                ValueInit = 0,
                ValueMiddle = 0.66f,
                ValueFinal = 0,
                MiddlePosition = 0.5f
            };            

            _video = new VideoPlayback();
            _video.VideoFile = "base/Intro2";
            _video.CurrentPosition = GI.Camera.ScreenCenter;
            _video.Play();
            this.SetComponent("tikitek", _video);

            _spriteGame = new Sprite("base/mechanica");
            this.SetComponent(_spriteGame);

            _spritePhysic = new Sprite("base/farseer");
            this.SetComponent(_spritePhysic);

            base.Initialize();

             this.ResetScreen();
        }
        #endregion

        #region Member
        public override void OnActivate()
        {
            _time = 0;
            _switch = false;

            GI.Camera.Zoom = 1;
            GI.Camera.CurrentPosition = Vector2.Zero;
            GI.Mouse.CurrentMouse = MouseCursor.None;
        }

        public override void ResetScreen()
        {
            _video.CurrentPosition = GI.Camera.ScreenCenter;

            _spriteGA.CurrentPosition = ConvertUnits.ToSimUnits(GI.Camera.ScreenCenter);
            _spriteGame.CurrentPosition = ConvertUnits.ToSimUnits(GI.Camera.ScreenCenter);
            _spritePhysic.CurrentPosition = ConvertUnits.ToSimUnits(GI.Camera.ScreenCenter);

            _spriteGA.Scale = (float)GI.Device.Viewport.Width / 1280;
            _spriteGame.Scale = (float)GI.Device.Viewport.Width / 1400;

            this.GetComponent<effectMask>("ga").Size = (new Vector2(1280, 1024) / 1280) * GI.Device.Viewport.Width;
        }
        #endregion

        #region Member - Xna
        public override void Update(GameTime gameTime)
        {
            _time += gameTime.ElapsedGameTime.TotalSeconds;

            if (_time > 10)
            {
                this.GetComponent<effectMask>("ga").IsAlive = false;
            }
            else if (_time > 5 && !_switch)
            {
                this.GetComponent<effectMask>("ga").IsAlive = true;
                this.GetComponent<VideoPlayback>("tikitek").Stop();

                _switch = true;
            }
            
            if (_time > 11 && _time < 12)
            {
                float c = (float)(_time - 11);

                this.BackgroundColor = new Color(c, c, c, 1.0f);
            }

            if (_time < 15)
            {
                float opacity = (float)_time - 12;
                opacity = (opacity < 0 ? 0 : (float)Math.Sin(opacity * MathHelper.PiOver2));

                _spritePhysic.Opacity = opacity;
            }

            float opacity2 = (float)_time - 15;
            opacity2 = (opacity2 < 0 ? 0 : (float)Math.Sin(opacity2 * MathHelper.PiOver2));

            _spriteGA.Opacity = _interGA.GetValue((float)(_time - 6) / 2);
            _spriteGame.Opacity = opacity2;
#if DEBUG
            if (GI.Control.KeyboardPressed(Keys.Enter))
            {
                _time = 0;
                _switch = false;

                this.GetComponent<effectMask>("ga").IsAlive = false;
                this.GetComponent<VideoPlayback>("tikitek").Play();
            }
#else            
            if (_time > 17f)
            {
                game.ChangeGameState<stateMenu>();
            }
#endif

            base.Update(gameTime);
        }
        #endregion
    }
}
