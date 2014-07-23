#if DESIGNER
using System;
using System.Runtime.Serialization;

namespace TikiEngine.Levels.Designer
{
    [Serializable]
    internal class designSet : DesignAction
    {
        #region Vars
        private DesignActionVar _var;

        private DesignAction _value;
        #endregion

        #region Init
        public designSet(DesignActionVar var, DesignAction value)
        {
            _var = var;
            _value = value;
        }

        public designSet(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Member
        public override string GenerateCode()
        {
            return String.Format(
                "{0} = {1}",
                _var.GenerateCode(),
                _value.GenerateCode()
            );
        }
        #endregion

        #region Properties
        public DesignActionVar Var
        {
            get { return _var; }
            set { _var = value; }
        }

        public DesignAction Value
        {
            get { return _value; }
            set { _value = value; }
        }
        #endregion
    }
}
#endif