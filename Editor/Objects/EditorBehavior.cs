using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TikiEngine.Elements.Physic;

namespace TikiEngine.Editor
{
    class EditorBehavior
    {
        #region Vars
        private Behavior _behavior;
        #endregion

        #region Init
        public EditorBehavior()
        { 
        }
        #endregion

        #region Properies
        public Behavior Behavior
        {
            get { return _behavior; }
            set { _behavior = value; }
        }
        #endregion
    }
}
