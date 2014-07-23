#if DESIGNER
using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace TikiEngine.Levels.Designer
{
    [Serializable]
    internal class designProperty : DesignActionVar
    {
        #region Vars
        private DesignActionVar _var;

        private PropertyInfo _info;
        #endregion

        #region Init
        public designProperty(DesignActionVar var, PropertyInfo info)
        {
            _var = var;
            _info = info;
        }

        public designProperty(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Member
        public override string GenerateCode()
        {
            return String.Format(
                "{0}.{1}",
                _var.GenerateCode(),
                _info.Name
            );
        }
        #endregion

        #region Properties
        public DesignActionVar Var
        {
            get { return _var; }
            set { _var = value; }
        }

        public PropertyInfo Info
        {
            get { return _info; }
            set { _info = value; }
        }

        public override Type VarType
        {
            get { return _info.PropertyType; }
            set { }
        }
        #endregion
    }
}
#endif