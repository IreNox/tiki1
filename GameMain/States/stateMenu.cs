using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TikiEngine.Components;
using TikiEngine.Elements.Controls;
using TikiEngine.Elements.Graphics;
using TikiEngine.Levels;
using Microsoft.Xna.Framework.Audio;

namespace TikiEngine.States
{
    internal class stateMenu : GameState
    {
        #region Vars
        private Circle _circleBig;
        private Sprite _spriteRobo;

        private List<MenuItem> _menuItems = new List<MenuItem>();
        private Dictionary<string, List<MenuItem>> _menus = new Dictionary<string, List<MenuItem>>();

        private float _alpha = 1f;
        private Texture2D _textureWhite;
        #endregion

        #region Init
        public stateMenu()
        {
            var tmp = Setup.defaultIslandDensity;
        }

        public override void Initialize()
        {
            base.Initialize();

            this.BackgroundStretch = false;
            this.LoadBackground("menu/menu_background");

            _textureWhite = new Texture2D(GI.Device, 1, 1);
            _textureWhite.SetData<Color>(new Color[] { Color.White });

            GI.Sound.AddLoopingSound(TikiSound.Atmo, 0.25f);

            _spriteRobo = new Sprite() {
                TextureFile = "menu/menu_robo",
                Origin = Vector2.Zero,
                Scale = 1f
            };

            _circleBig = new Circle(
                new Circle.CircleElement("circle/circle_b_0", 0),
                new Circle.CircleElement("circle/circle_b_1", 0),
                new Circle.CircleElement("circle/circle_b_2", 0.0002f),
                new Circle.CircleElement("menu/menu_admiral", 0, 0.1f, 0.95f),
                new Circle.CircleElement("menu/menu_connection", 0),
                new Circle.CircleElement("menu/menu_connection", 0),
                new Circle.CircleElement("menu/menu_connection", 0),
                new Circle.CircleElement("menu/menu_connection", 0),
                new Circle.CircleElement("menu/menu_connection", 0)
            );
            _circleBig.Elements[8].Opacity = 0;

            _initItemsMain();
            _initItemsLevels();
            _initItemsOptions();

            this.ResetScreen();
        }
        #endregion

        #region Init - Items
        private void _initItemsMain()
        {
            List<MenuItem> items = new List<MenuItem>();

            items.Add(
                new MenuItem(
                    this,
                    "settings",
                    0.70f,
                    delegate(object sender, EventArgs e)
                    {
                        this.SelectMenu("settings");
                    }
                )
            );

            items.Add(
                new MenuItem(
                    this,
                    "play",
                    0.5f,
                    delegate(object sender, EventArgs e)
                    {
                        this.SelectMenu("levels");
                    }
                )
            );

            items.Add(
                 new MenuItem(
                     this,
                     "exit",
                     0.30f,
                     delegate(object sender, EventArgs e)
                     {
                         game.Exit();
                     }
                 )
             );

            items.Add(
                  new MenuItem(
                      this,
                      "back",
                      1.5f,
                      delegate(object sender, EventArgs e)
                      {
                      }
                  )
            );

            items.Add(
                new MenuItem(
                    this,
                    "resume",
                    -1,
                    delegate(object sender, EventArgs e)
                    {
                        game.ChangeGameState<stateLevel>();
                    }
                )
            );
            items[4].Enabled = false;

            _menus["main"] = items;
            _menuItems = items;
        }

        private void _initItemsLevels()
        {
            List<MenuItem> items = new List<MenuItem>();

            items.Add(
                new MenuItem(
                    this,
                    "level1",
                    0.75f,
                    delegate(object sender, EventArgs e)
                    {
                        game.ChangeGameState<stateLevel>().SetLevel<level1>();
                    }
                )
            );

            items.Add(
                new MenuItem(
                    this,
                    "level2",
                    0.585f,
                    delegate(object sender, EventArgs e)
                    {
                        game.ChangeGameState<stateLevel>().SetLevel<level2>();
                    }
                )
            );

            items.Add(
                new MenuItem(
                    this,
                    "level3",
                    0.415f,
                    delegate(object sender, EventArgs e)
                    {
                        game.ChangeGameState<stateLevel>().SetLevel<level3>();
                    }
                )
            );

            items.Add(
                new MenuItem(
                    this,
                    "level4",
                    0.25f,
                    delegate(object sender, EventArgs e)
                    {
                        game.ChangeGameState<stateLevel>().SetLevel<level4>();
                    }
                )
            );

            items.Add(
                new MenuItem(
                    this,
                    "back",
                    -1,
                    delegate(object sender, EventArgs e)
                    {
                        this.SelectMenu("main");
                    }
                )
            );
            items[4].Circle.CurrentPosition = new Vector2(7, 1);

            _menus["levels"] = items;
        }

        private void _initItemsOptions()
        {
            List<MenuItem> items = new List<MenuItem>();

            items.Add(
                new MenuItem(
                    this,
                    "audio",
                    0.75f,
                    delegate(object sender, EventArgs e)
                    {
                        game.ChangeGameState<stateOptionAudio>();
                    }
                )
            );

            items.Add(
                new MenuItem(
                    this,
                    "video",
                    0.585f,
                    delegate(object sender, EventArgs e)
                    {
                        game.ChangeGameState<stateOptionGraphics>();
                    }
                )
            );

            items.Add(
                new MenuItem(
                    this,
                    "team",
                    0.415f,
                    delegate(object sender, EventArgs e)
                    {
                        game.ChangeGameState<stateCredits>();
                    }
                )
            );

            items.Add(
                new MenuItem(
                    this,
                    "back",
                    0.25f,
                    delegate(object sender, EventArgs e)
                    {
                        this.SelectMenu("main");
                    }
                )
            );

            _menus["settings"] = items;
        }
        #endregion

        #region Member
        public override void OnActivate()
        {
            GI.Camera.RealZoom = 1.0f;
            GI.Camera.TrackingBody = null;
            GI.Camera.CurrentPosition = Vector2.Zero;
            GI.Mouse.CurrentMouse = MouseCursor.Default;

            this.SelectMenu("main");
        }

        public void SelectMenu(string key)
        {
            _alpha = 1;
            _menuItems = _menus[key];
        }

        public override void ResetScreen()
        {
            _circleBig.CurrentPosition = ConvertUnits.ToSimUnits(
                new Vector2(0, GI.Camera.ScreenCenter.Y)
            );

            _spriteRobo.CurrentPosition = new Vector2(
                ConvertUnits.ToSimUnits(GI.Camera.ScreenSize.X - _spriteRobo.TextureSize.X),
                0.32f
            );
            _spriteRobo.Scale = GI.Camera.ScreenSize.Y / _spriteRobo.TextureSize.Y;

            _menus["main"][4].Circle.CurrentPosition = new Vector2(8, _circleBig.CurrentPosition.Y);
        }
        #endregion

        #region Member - Xna - Draw
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _circleBig.Draw(gameTime);
            _spriteRobo.Draw(gameTime);

            foreach (MenuItem item in _menuItems)
            {
                item.Draw(gameTime);
            }

            GI.SpriteBatch.Draw(
                _textureWhite,
                GI.Camera.ViewDisplayRectangle,
                new Color(_alpha, _alpha, _alpha, _alpha)
            );
        }
        #endregion

        #region Member - Xna - Update
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Vector2 mousePos = GI.Control.MouseDisplayPoint().ToVector2() / 500;

            _circleBig.Update(gameTime);
            _circleBig.Elements[0].Angle = mousePos.X;
            _circleBig.Elements[1].Angle = mousePos.Y;

            int i = 0;
            foreach (MenuItem item in _menuItems)
            {
                item.Update(gameTime);

                Vector2 diff = _circleBig.CurrentPosition - item.Circle.CurrentPosition;
                float dot = (float)Math.Atan2(diff.X, diff.Y) - (MathHelper.Pi * 0.5f);
                Circle.CircleElement element = _circleBig.Elements[i + 4];

                element.Scale = 1;
                element.Angle = MathHelper.Pi - dot;

                i++;
            }

            for (; i < 4; i++)
            {
                _circleBig.Elements[i + 4].Scale = 0;
            }

            GI.Sound.SetVolume(
                TikiSound.Atmo,
                0.6f - (_menuItems.Min(e => e.Distance) / 3)
            );


            if (_alpha > 0) _alpha -= 0.001f * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

#if DEBUG
            if (GI.Control.KeyboardPressed(Keys.Enter)) game.ChangeGameState<stateLevel>().SetLevel<level1>();
            if (GI.Control.KeyboardPressed(Keys.RightShift)) game.ChangeGameState<stateIntro>();
#endif
            if (GI.Control.KeyboardPressed(Keys.Escape)) game.Exit();
        }
        #endregion

        #region Properties
        public Dictionary<string, List<MenuItem>> Menus
        {
            get { return _menus; }
        }
        #endregion

        #region Class - MenuItem
        internal class MenuItem
        {
            #region Vars
            private stateMenu _owner;

            private Circle _circle;
            private float _positionTranslation = 0;

            private float _distance;

            private bool _enabled = true;

            public event EventHandler Click;
            #endregion

            #region Init
            public MenuItem(stateMenu owner, string name, float translation, EventHandler onClick)
            {
                _owner = owner;
                _positionTranslation = translation;

                if (onClick != null) this.Click += onClick;

                _circle = new Circle(
                    _createCircleElement(0),
                    _createCircleElement(1),
                    _createCircleElement(2),
                    new Circle.CircleElement("menu/item_" + name + "0", 0f),
                    new Circle.CircleElement("menu/item_" + name + "1", 0f),
                    new Circle.CircleElement("menu/item_" + name + "_text", 0.0002f, -0.4f, 0.1f)
                )
                {
                    Scale = 0.75f
                };
            }
            #endregion

            #region Private Member
            private Circle.CircleElement _createCircleElement(int size)
            {
                int type = Functions.GetRandom(0, 1);
                int angle = Functions.GetRandom(0, 1);
                string name = String.Format("circle/circle_m{0}_{1}", type, size);

                if (angle == 1)
                {
                    return new Circle.CircleElement(
                        name,
                        0,
                        Functions.GetRandom(0f, 1f),
                        Functions.GetRandom(0f, 1f)
                    );
                }
                else
                {
                    return new Circle.CircleElement(
                        name,
                        Functions.GetRandom(-0.001f, 0.001f)
                    );
                }
            }
            #endregion

            #region Member
            public void Draw(GameTime gameTime)
            {
                if (_enabled) _circle.Draw(gameTime);
            }

            public void Update(GameTime gameTime)
            {
                _circle.Update(gameTime);

                _distance = Vector2.Distance(_circle.CurrentPosition, GI.Control.MouseSimVector()) - 0.5f;
                if (_distance > 1)
                {
                    _distance = 1;
                }
                else if (GI.Control.MouseClick(MouseButtons.Left) && _enabled)
                {
                    GI.Sound.PlaySFX(TikiSound.Menu_Click, 0.7f);

                    if (this.Click != null) this.Click(this, EventArgs.Empty);
                }
                if (_distance < 0) _distance = 0;

                _circle.Elements[3].Opacity = _distance;
                _circle.Elements[4].Opacity = 1 - _distance;

                float time = Math.Max(0, _owner._alpha - 0.5f);
                float a = MathHelper.Pi * (_positionTranslation - time);
                // Maus beeinflust Position
                //a += (GI.Camera.ScreenCenter.Y - GI.Control.MouseCurrentState.Y) / (GI.Device.Viewport.Height * 10);

                if (_positionTranslation != -1)
                {
                    _circle.CurrentPosition = new Vector2(
                        (float)Math.Sin(a),
                        (float)Math.Cos(a)
                    ) * 5f + new Vector2(0f, GI.Camera.ScreenCenter.Y / 100);
                }
            }
            #endregion

            #region Properties
            public Circle Circle
            {
                get { return _circle; }
            }

            public bool Enabled
            {
                get { return _enabled; }
                set { _enabled = value; }
            }

            public float Distance
            {
                get { return _distance; }
            }

            public float PositionTranslation
            {
                get { return _positionTranslation; }
            }
            #endregion
        }
        #endregion
    }
}
