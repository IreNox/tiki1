using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TikiEngine.Elements;

namespace TikiEngine.Editor
{
    class EditorBaseObject : NameObject
    {
        #region Vars
        private NameObject _baseObject;
        #endregion

        #region Init
        public EditorBaseObject()
        {
        }

        public EditorBaseObject(NameObject baseObject)
        {
            _baseObject = baseObject;
        }
        #endregion

        #region Member
        public void CreateBaseObject(Type type)
        {
            _baseObject = (NameObject)Activator.CreateInstance(type);
        }

        public void CreateBaseObject<T>()
            where T : NameObject, new()
        {
            _baseObject = new T();
        }

        public override void Dispose()
        {
            _baseObject.Dispose();
        }
        #endregion

        #region Member - XNA
        public override void Draw(GameTime gameTime)
        {
            if (this.Ready) _baseObject.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (this.Ready) _baseObject.Update(gameTime);
        }
        #endregion

        #region Member - Protected
        protected override void ApplyChanges()
        {
        }
        #endregion

        #region Properties
        public override string Name
        {
            get { return _baseObject.Name; }
            set { _baseObject.Name = value; }
        }

        public override bool Ready
        {
            get { return (_baseObject != null ? _baseObject.Ready : false); }
        }

        public Type BaseType
        {
            get
            {
                return (_baseObject == null ? null : _baseObject.GetType());
            }
        }

        public NameObject BaseObject
        {
            get { return _baseObject; }
            set { _baseObject = value; }
        }
        #endregion
    }
}
