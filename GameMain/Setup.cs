using Microsoft.Xna.Framework;
using TikiEngine.Elements.Graphics;

namespace TikiEngine
{
    internal static partial class Setup
    {
        #region Init
        static Setup()
        {
            _initBreakable();
        }
        #endregion

        #region Member - Draw/Update
        public static void Draw(GameTime gameTime)
        {
            _drawGame(gameTime);
        }

        public static void Update(GameTime gameTime)
        {
            _updateGame(gameTime);
            _updateBreakable(gameTime);
        }
        #endregion
    }
}
