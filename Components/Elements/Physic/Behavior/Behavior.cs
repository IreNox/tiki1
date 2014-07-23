using System;
using System.Runtime.Serialization;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace TikiEngine.Elements.Physic
{
    #region Enum - PhysicBehavior
    public enum PhysicBehavior
    {
        Wiggle,
        Static,
        Dynamic,
        Tracking,
        Collectable
    }
    #endregion

    [Serializable]
    public abstract class Behavior : ISerializable
    {
        #region Vars
        protected Body body;
        protected World world;
        protected NameObjectPhysic nop;
        #endregion

        #region Init
        public Behavior(NameObjectPhysic nop)
        {
            this.nop = nop;
            this.body = nop.Body;
            this.world = nop.World;
        }
        #endregion

        #region Member
        public abstract void ApplyChanges();

#if DEBUG
        public virtual void Draw(GameTime gameTime)
        {
        }
#endif

        public abstract void Update(GameTime gameTime);
        #endregion

        #region Member - Serialization
        public Behavior(SerializationInfo info, StreamingContext context)
        {
            Serialization.ObjectDeserialize(this, info);
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Serialization.ObjectSerialize(this, info);
        }
        #endregion

        #region Properties
        public NameObjectPhysic NameObjectPhysic
        {
            get { return nop; }
        }
        #endregion
    }
}
