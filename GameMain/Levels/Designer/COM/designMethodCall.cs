#if DESIGNER
using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace TikiEngine.Levels.Designer
{
    [Serializable]
    internal class designMethodCall : DesignActionArgs
    {
        #region Vars
        private MethodInfo _info;

        private DesignActionVar _obj;
        #endregion

        #region Init
        public designMethodCall(DesignActionVar obj, MethodInfo info)
        {
            if (info == null)
            {
                throw new ArgumentException("", "info");
            }

            _obj = obj;
            _info = info;
        }

        public designMethodCall(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Member
        public override string GenerateCode()
        {
            return String.Format(
                "{0}.{1}({2})",
                (_obj == null ? _info.DeclaringType.FullName : _obj.GenerateCode()),
                _info.Name,
                this.GenerateCodeArgs()
            );
        }
        #endregion

        #region Properties
        public MethodInfo Info
        {
            get { return _info; }
            set { _info = value; }
        }

        public DesignActionVar Object
        {
            get { return _obj; }
            set { _obj = value; }
        }
        #endregion
    }
}
#endif