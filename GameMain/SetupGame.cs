using TikiEngine.Elements;
using Microsoft.Xna.Framework;

namespace TikiEngine
{
    internal static partial class Setup
    {
        #region Vars
        private static Robo _robo;
        #endregion

        #region Member
        public static void Reset(LevelGame level)
        {
            _robo = level.Robo;

            GI.Level = level;
            GI.World = level.World;
            GI.Camera.TrackingBody = _robo.Body;
        }
        #endregion

        #region Member - Update
        public static void _drawGame(GameTime gameTime)
        {
            _robo.Draw(gameTime);
        }

        private static void _updateGame(GameTime gameTime)
        {
            _robo.Update(gameTime);
        }
        #endregion

        #region Properties
        public static Robo Robo
        {
            get { return _robo; }
            set { _robo = value; }
        }
        #endregion
    }
}
