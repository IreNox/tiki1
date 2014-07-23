#if DESIGNER
using System;
using System.Linq;
using System.Collections.Generic;
using TikiEngine.Elements;
using TikiEngine.Levels.Designer;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics;
using TikiEngine.Elements.Physic;

namespace TikiEngine
{
    internal class LevelDesigner : Level
    {
        #region Vars
        private bool _designMode = false;

        private GameElement _currentElement;

        private List<GameElementIsland> _listIslands = new List<GameElementIsland>();
        #endregion

        #region Init
        public LevelDesigner()
        {
            this.Name = this.GetType().Name;

            LevelSave save = DataManager.LoadObject<LevelSave>(this.Name, true);

            if (save != null)
            {
                save.LoadLevel(this);
            }
        }

        public LevelDesigner(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Member
        public virtual void OnActivate()
        {
            Program.Form.Designer.Level = this;
        }

        public void DrawElement()
        {
            if (_currentElement == null) return;

            if (_currentElement.Ready)
            {
                Program.Game.RenderToImage.Add(_currentElement);

                if (Program.Game.RenderToImageOutput.ContainsKey(_currentElement) && Program.Game.RenderToImageOutput[_currentElement] != null)
                {
                    Program.Game.RenderToImageOutput[_currentElement].Dispose();
                }
            }

            Program.Game.RenderToImageOutput[_currentElement] = null;
        }
        #endregion

        #region Member - Xna - Draw
        public override void Draw(GameTime gameTime)
        {
            if (_designMode)
            {
                foreach (GameElementIsland island in _listIslands)
                {
                    if (island.Ready) island.Draw(gameTime);
                }

                if (_currentElement != null)
                {
                    Vector2 pos = _currentElement.CurrentPosition - (_currentElement.Size / 2);
                    Vector2 lv = pos;
                    Vector2[] arr = new[] {
                    new Vector2(_currentElement.Size.X, 0),
                    new Vector2(_currentElement.Size.X, _currentElement.Size.Y),
                    new Vector2(0, _currentElement.Size.Y),
                    Vector2.Zero
                };

                    foreach (Vector2 v in arr)
                    {
                        Vector2 v2 = pos + v;

                        GI.PolygonBatch.DrawLine(lv, v2, Color.Red, 1.0f);

                        lv = v2;
                    }
                }
            }
            else
            {
                base.Draw(gameTime);
            }
        }
        #endregion

        #region Member - Xna - Update
        public override void Update(GameTime gameTime)
        {
            if (GI.Control.KeyboardPressed(Keys.F6)) this.DesignMode = !_designMode;

            foreach (GameElementIsland island in _listIslands)
            {
                if (island.Ready) island.Update(gameTime);
            }
            
            if (GI.Control.MouseDown(MouseButtons.Right))
            {
                Vector2 dis = GI.Control.MouseDistanceDisplay();

                GI.Camera.CurrentPosition -= dis;
            }

            Vector2 mouse = GI.Control.MouseSimVector();

            if (GI.Control.MouseClick(MouseButtons.Left) && GI.Camera.ViewSimVector.Contains(mouse))
            {
                var q = _listIslands.FirstOrDefault(i => i.Rectangle.Contains(mouse));

                if (q != null)
                {
                    this.CurrentElement = q;
                }
            }

            if (_currentElement != null)
            {
                if (Program.Game.RenderToImageOutput[_currentElement] != Program.Form.Designer.Image)
                {
                    Program.Form.Designer.SetImage(
                        Program.Game.RenderToImageOutput[_currentElement]
                    );
                }

                _currentElement.StartPosition += new Vector2(
                    (GI.Control.KeyboardDown(Keys.Left) ? -1 : 0) + (GI.Control.KeyboardDown(Keys.Right) ? 1 : 0),
                    (GI.Control.KeyboardDown(Keys.Up) ? -1 : 0) + (GI.Control.KeyboardDown(Keys.Down) ? 1 : 0)
                ) / (GI.Control.KeyboardDown(Keys.LeftShift) || GI.Control.KeyboardDown(Keys.RightShift) ? 10 : 100);
            }

            if (!_designMode) base.Update(gameTime);
        }
        #endregion

        #region Properties
        public List<GameElementIsland> Islands
        {
            get { return _listIslands; }
            internal set { _listIslands = value; }
        }

        public GameElement CurrentElement
        {
            get { return _currentElement; }
            set
            {
                _currentElement = value;

                this.DrawElement();

                Program.Form.Invoke(
                    new Action<NameObject>(Program.Form.Designer.LoadObject),
                    value
                );
            }
        }

        public bool DesignMode
        {
            get { return _designMode; }
            set
            {
                _designMode = value;

                Program.Form.DesignShow = value;
                GI.Camera.TrackingBody = (value ? null : Setup.Robo.Body);
            }
        }
        #endregion
    }
}
#endif