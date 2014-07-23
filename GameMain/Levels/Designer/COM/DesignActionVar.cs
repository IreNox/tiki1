#if DESIGNER
using System;
using System.Runtime.Serialization;

namespace TikiEngine.Levels.Designer
{
    [Serializable]
    internal abstract class DesignActionVar : DesignAction
    {
        #region Init
        public DesignActionVar()
        { 
        }

        public DesignActionVar(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Properties
        public abstract Type VarType { get; set; }
        #endregion
    }
}
#endif