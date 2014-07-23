#if DESIGNER
using System;
using System.Runtime.Serialization;

namespace TikiEngine.Levels.Designer
{
    [Serializable]
    internal class designDeclareField : designSet
    {
        #region Init
        public designDeclareField(DesignActionVar var, DesignAction value)
            : base(var, value)
        { 
        }

        public designDeclareField(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Member
        public override string GenerateCode()
        {
            string type = (this.Var.VarType == null ? "var" : this.Var.VarType.FullName);

            return type + " " + base.GenerateCode();
        }
        #endregion
    }
}
#endif