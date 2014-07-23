#if DESIGNER
using System;
using System.Runtime.Serialization;
using TikiEngine;

namespace TikiEngine.Levels.Designer
{
    [Serializable]
    internal abstract class DesignAction : ISerializable
    {
        #region Init
        public DesignAction()
        { 
        }

        public DesignAction(SerializationInfo info, StreamingContext context)
        {
            Serialization.ObjectDeserialize(this, info);
        }
        #endregion

        #region Member
        public abstract string GenerateCode();

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Serialization.ObjectSerialize(this, info);
        }
        #endregion
    }
}
#endif