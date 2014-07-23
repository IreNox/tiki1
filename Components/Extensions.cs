using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;

namespace TikiEngine
{
    public static class Extensions
    {
        #region Collections
        public static TValue Add<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key, TValue value)
        {
            source.Add(
                key,
                value
            );

            return value;
        }

        public static void AddRange<TKey, TValue>(this Dictionary<TKey, TValue> source, Dictionary<TKey, TValue> value)
        {
            foreach (var kvp in value)
            {
                source.Add(
                    kvp.Key,
                    kvp.Value
                );
            }
        }

        public static void AddRange<TKey, TValue>(this Dictionary<TKey, TValue> source, params KeyValuePair<TKey, TValue>[] value)
        {
            foreach (var kvp in value)
            {
                source.Add(
                    kvp.Key,
                    kvp.Value
                );
            }
        }
        #endregion

        #region Vector2
        public static Point ToPoint(this Vector2 source)
        {
            return new Point((int)source.X, (int)source.Y);
        }

        public static Vector2 ToVector2(this Point source)
        {
            return new Vector2(source.X, source.Y);
        }

        public static bool IsReady(this Vector2 source)
        {
            return source.X != 0 || source.Y != 0;
        }

        public static Vector2 CalculateSize(this IEnumerable<Vector2> source)
        {
            return new Vector2(
                source.Max(v => v.X) - source.Min(v => v.X),
                source.Max(v => v.Y) - source.Min(v => v.Y)
            );
        }

        public static Vertices SelectAll(this List<Vertices> source)
        {
            return new Vertices(
                source.SelectMany(v => v).Distinct().ToArray()
            );
        }

        public static Vector2 GetCenter(this Texture2D source)
        {
            return new Vector2(source.Width, source.Height) / 2;
        }
        #endregion

        #region Vector4
        public static Color ToColor(this Vector4 v)
        {
            return new Color(v.X, v.Y, v.Z, v.W);
        }

        public static Rectangle ToRectangle(this Vector4 source)
        {
            return new Rectangle(
                (int)source.X,
                (int)source.Y,
                (int)source.Z,
                (int)source.W
            );
        }

        public static bool Contains(this Vector4 source, Vector2 point)
        {
            return (point.X > source.X) &&
                   (point.Y > source.Y) &&
                   (point.X < source.X + source.Z) &&
                   (point.Y < source.Y + source.W);
        }
        #endregion

        #region Physic
        public static bool IsReady(this Body source)
        {
            return !source.IsDisposed && GI.World.BodyList.Contains(source) && source.FixtureList != null;
        }
        #endregion
    }
}
