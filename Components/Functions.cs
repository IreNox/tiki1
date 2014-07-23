using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TikiEngine
{
    public static class Functions
    {
        #region Vars
        private static Random _random;
        #endregion

        #region Init
        static Functions()
        {
            _random = new Random();
        }
        #endregion

        #region Member - Vector
        public static Vector2 AngleToVector(float angle)
        {
            return new Vector2(
                (float)Math.Sin(angle),
                (float)Math.Cos(angle)
            );
        }
        #endregion

        #region Member - Random
        public static int GetRandom(int min, int max)
        {
            return _random.Next(min, max + 1);
        }

        public static float GetRandom(float min, float max)
        {
            return ((float)_random.Next(
                (int)(min * 1000000),
                (int)(max * 1000000)
            )) / 1000000.0f;
        }

        public static Texture2D GetTexture(string fileName)
        {
            Stream stream = null;

            try
            {
                stream = File.OpenRead(@"Data\Graphics\" + fileName.Replace('\\', '/') + ".png");

                return Texture2D.FromStream(GI.Device, stream);
            }
            catch
            {
                return null;
            }
            finally
            {
                if (stream != null) stream.Dispose();
            }
        }
        #endregion

        #region Properties
        public static Random Random
        {
            get { return _random; }
        }
        #endregion
    }
}
