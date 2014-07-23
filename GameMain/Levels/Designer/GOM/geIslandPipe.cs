#if DESIGNER
using System;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization;

namespace TikiEngine.Levels.Designer
{
    [Serializable]
    internal class geIslandPipe : GameElementIsland
    {
        #region Init
        public geIslandPipe()
        {
        }

        public geIslandPipe(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Member
        protected override designMethodCall CreateMethodCall(DesignActionVar var)
        {
            var info = typeof(Setup).GetMethod(
                "CreatePipe",
                new Type[] { typeof(string), typeof(Vector2), typeof(float) }
            );

            var callCom = new designMethodCall(var, info);
            callCom.Args.Add(textureCom);
            callCom.Args.Add(positionCom);
            callCom.Args.Add(rotationCom);

            return callCom;
        }
        #endregion
    }
}
#endif