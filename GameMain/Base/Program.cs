using System;
using System.Threading;

namespace TikiEngine
{
#if WINDOWS || XBOX
    internal static class Program
    {
        #region Vars
        private static Config _config;
        private static GameMain _game;
#if DESIGNER
        private static TikiEngine.Levels.Designer.formMain _form;
#endif
        #endregion

        #region Main
        [STAThread]
        private static void Main(string[] args)
        {
            _config = DataManager.LoadObject<Config>("default", true);
            if (_config == null)
            {
                _config = new Config();
            }

            _game = new GameMain();
            _game.Run();
            _game.Dispose();
        }
        #endregion

        #region Properties
        public static GameMain Game
        {
            get { return _game; }
        }

        public static Config Config
        {
            get { return _config; }
        }

#if DESIGNER
        public static TikiEngine.Levels.Designer.formMain Form
        {
            get { return _form; }
            set { _form = value; }
        }
#endif
        #endregion
    }
#endif
}

