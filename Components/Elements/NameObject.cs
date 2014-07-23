using System;
using System.Reflection;
using System.ComponentModel;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using FarseerPhysics.Common;

namespace TikiEngine.Elements
{
    [Serializable]
    public abstract class NameObject : ISerializable, IDisposable
    {
        #region Vars
        private string _name;

        protected Vector2 positionStart;
        protected Vector2 positionCurrent;
        #endregion

        #region Init
        public NameObject()
        {
            _name = "";
        }

        public NameObject(string name)
        {
            _name = name;
        }
        #endregion

        #region Member
        protected abstract void ApplyChanges();

        public abstract void Draw(GameTime gameTime);
        public abstract void Update(GameTime gameTime);

        public abstract void Dispose();

        public virtual void Reset()
        { 
        }

        public NameObject Clone()
        {
            return (NameObject)this.MemberwiseClone();
        }
        #endregion

        #region Member - Serialization
        public NameObject(SerializationInfo info, StreamingContext context)
        {
            Serialization.ObjectDeserialize(this, info);

            this.ApplyChanges();
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Serialization.ObjectSerialize(this, info);
        }
        #endregion

        #region Properties
        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public virtual Vector2 StartPosition
        {
            get { return positionStart; }
            set
            {
                positionStart = value;
                this.CurrentPosition = value;
            }
        }

        [NonSerializedTiki]
        public virtual Vector2 CurrentPosition
        {
            get { return positionCurrent; }
            set { positionCurrent = value; }
        }
        #endregion

        #region Properties - Abstract
        [Browsable(false)]
        [NonSerializedTiki]
        public abstract bool Ready { get; }
        #endregion
    }
}
