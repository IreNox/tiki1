using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TikiEngine.Editor.Controls;
using TikiEngine.Elements;

namespace TikiEngine.Editor.Modes
{
    class modeObject : EditorMode
    {
        #region Vars
        private EditorBaseObject _object;
        #endregion

        #region Init
        public override void Init()
        {
            this.UseCamera = true;

            _object = new EditorBaseObject();

            this.AddTabPage<ucObjectPropertys>("Propertys");
        }
        #endregion

        #region Member
        public void CreateBaseObject(Type type)
        {
            _object.CreateBaseObject(type);
        }

        public override void Activate()
        {
            GI.Camera.ResetPosition();
        }
        #endregion

        #region Member - XNA
        public override void Draw(GameTime gameTime)
        {
            if (_object.Ready)
            {
                _object.Draw(gameTime);
            }
        }

        public override void Update(GameTime gameTime)
        {
            GI.World.Step(0);

            if (_object.Ready)
            {
                _object.Update(gameTime);
            }
        }
        #endregion

        #region Member - File
        public override void New()
        {
            _object.BaseObject = null;

            this.RaiseObjectChanged();
        }

        public override void Open(string name)
        {
            _object.BaseObject = DataManager.LoadObject<NameObject>(name, true);

            this.RaiseObjectChanged();
        }

        public override void Save()
        {
            DataManager.SetObject<NameObject>(_object.BaseObject);
        }

        public override void SaveAs(string name)
        {
            _object.BaseObject.Name = name;
            DataManager.SetObject<NameObject>(_object.BaseObject);
        }

        public override void ShowPreferences()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Propertys
        public override Type ObjectType
        {
            get { return typeof(NameObject); }
        }

        public Type CurrentObjectType
        {
            get { return (_object.BaseObject != null ? _object.BaseObject.GetType() : null); }
        }        

        public override NameObject CurrentObject
        {
            get { return this.CurrentBaseObject; }
        }

        public NameObject CurrentBaseObject
        {
            get { return _object.BaseObject; }
            set
            {
                if (value != null)
                {
                    _object.BaseObject = value;

                    this.RaiseObjectChanged();
                }
            }
        }

        public override string ObjectName
        {
            get { return (_object.BaseObject == null ? null : _object.BaseObject.Name);  }
            set
            {
                if (_object.BaseObject != null) _object.BaseObject.Name = value;
            }
        }
        #endregion
    }
}
