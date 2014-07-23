using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace TikiEngine.Components
{
    public class MouseEventArgs : EventArgs
    {
        #region Vars
        private MouseState _mouseState;
        #endregion

        #region Init
        public MouseEventArgs(MouseState mouseState)
        {
            _mouseState = mouseState;
        }
        #endregion

        #region Properties
        public MouseState MouseState
        {
            get { return _mouseState; }
        }
        #endregion
    }
}
