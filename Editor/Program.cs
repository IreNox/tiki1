using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TikiEngine.Editor
{
    internal static class Program
    {
        #region Vars
        internal static formMain FormMain;
        internal static gameMain GameMain;
        //internal static GraphicsDeviceControl Device;
        #endregion

        #region Main
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Program.GameMain = new gameMain();
            Program.GameMain.Run();

            //Program.FormMain = new formMain();
            //Program.FormMain.Show();
            //Program.FormMain.Init();

            //Application.Run(
            //    Program.FormMain
            //);
        }
        #endregion
    }
}
