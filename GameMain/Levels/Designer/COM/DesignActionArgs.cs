#if DESIGNER
using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TikiEngine.Levels.Designer
{
    [Serializable]
    internal abstract class DesignActionArgs : DesignAction
    {
        #region Vars
        private List<DesignAction> _args = new List<DesignAction>();
        #endregion

        #region Init
        public DesignActionArgs()
        { 
        }

        public DesignActionArgs(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Member
        public string GenerateCodeArgs()
        {
            StringBuilder sb = new StringBuilder();

            foreach (DesignAction da in _args)
            {
                if (sb.Length != 0) sb.Append(", ");
                sb.Append(da.GenerateCode());
            }

            return sb.ToString();
        }
        #endregion

        #region Properties
        public List<DesignAction> Args
        {
            get { return _args; }
            set { _args = value; }
        }
        #endregion
    }
}
#endif