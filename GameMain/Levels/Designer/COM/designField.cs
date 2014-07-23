#if DESIGNER
using System;
using System.Runtime.Serialization;

namespace TikiEngine.Levels.Designer
{
    [Serializable]
    internal class designField : DesignActionVar
    {
        #region Vars
        private Type _type;
        private string _varName = "";
        #endregion

        #region Init
        public designField(Type type, string varName)
        {
            if (type == null)
            { 
                throw new ArgumentException("", "type");
            }

            _type = type;
            _varName = varName;
        }

        public designField(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Member
        public override string GenerateCode()
        {
            return _varName;
        }
        #endregion

        #region Properties
        public string VarName
        {
            get { return _varName; }
            set { _varName = value; }
        }

        public override Type VarType
        {
            get { return _type; }
            set { _type = value; }
        }
        #endregion
    }
}
#endif