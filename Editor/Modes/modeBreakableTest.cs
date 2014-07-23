using System;
using System.Linq;
using System.Collections.Generic;
using FarseerPhysics.Common;
using TikiEngine.Elements;
using TikiEngine.Elements.Physic;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using TikiEngine.Editor.Controls;

namespace TikiEngine.Editor.Modes
{
    internal class modeBreakableTest : EditorMode
    {
        #region Vars
        private modeBreakable _modeBreak;

        private PhysicBox _physicBox;
        private PhysicBodyBreakable _breakBody;
        #endregion

        #region Init
        public modeBreakableTest()
        {
            this.UseCamera = true;
            this.UsePreferences = true;
            this.BackgroundColor = Color.AliceBlue;
        }

        public override void Init()
        {
            this.AddTabPage<ucMainProperties>("Properties");

            _modeBreak = Program.GameMain.GetMode<modeBreakable>();

            GI.World = Program.GameMain.EditorModes[_modeBreak];
            Program.GameMain.EditorModes[this] = GI.World;

            _physicBox = new PhysicBox(
                new Vector2(20, 0.1f),
                new Vector2(0, 5),
                0,
                "background"
            ) { 
                BodyType = BodyType.Static
            };
        }
        #endregion

        #region Member
        public override void Activate()
        {
            foreach (Body body in GI.World.BodyList)
            {
                if (body != _physicBox.Body && body != _breakBody.Body) body.Dispose();
            }

            this.ShowPreferences();
        }
        #endregion

        #region Member - File
        public override void New()
        {
            _modeBreak.New();
        }

        public override void Open(string name)
        {
            _modeBreak.Open(name);
        }

        public override void Save()
        {
            _modeBreak.Save();
        }

        public override void SaveAs(string name)
        {
            _modeBreak.SaveAs(name);
        }

        public override void ShowPreferences()
        {
            this.GetTabControl<ucMainProperties>().SelectedObject = _modeBreak;
        }
        #endregion

        #region Member - Xna - Draw
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            _physicBox.Draw(gameTime);
            if (_breakBody != null) _breakBody.Draw(gameTime);
        }
        #endregion

        #region Member - Xna - Update
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            GI.World.Step(
                Math.Min((float)gameTime.ElapsedGameTime.TotalMilliseconds, (1f / 30f))
            );

            _physicBox.Update(gameTime);

            if (_breakBody != null)
            {
                _breakBody.Update(gameTime);

                if (GI.Control.MouseDown(MouseButtons.Left) && _breakBody.Body != null)
                {
                    _breakBody.Body.ApplyLinearImpulse(GI.Control.MouseDistanceSim());
                }
            }
        }
        #endregion

        #region Properties
        public override Type ObjectType
        {
            get { return typeof(PhysicBodyBreakable); }
        }

        public override string ObjectName
        {
            get { return ""; }
            set {  }
        }

        public override NameObject CurrentObject
        {
            get { return null; }
        }

        public PhysicBodyBreakable BodyBreakable
        {
            get { return _breakBody; }
            set { _breakBody = value; }
        }
        #endregion
    }
}
