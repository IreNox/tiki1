#if DESIGNER
using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace TikiEngine.Levels.Designer
{
    [Serializable]
    internal class designConstructorCall : DesignActionArgs
    {
        #region Vars
        private ConstructorInfo _info;
        #endregion

        #region Init
        public designConstructorCall(ConstructorInfo info)
        {
            _info = info;
        }

        public designConstructorCall(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Member
        public override string GenerateCode()
        {
            return String.Format(
                "new {0}({1})",
                _info.ReflectedType.FullName,
                this.GenerateCodeArgs()
            );
        }
        #endregion

        #region Properties
        public ConstructorInfo Info
        {
            get { return _info; }
            set { _info = value; }
        }
        #endregion
    }
}
#endif