using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using TikiEngine.Elements;
using System.Windows.Forms;


namespace TikiEngine
{
	public static class DataManager
    {
        #region Vars
        private static string _ext = "bin";

        private static Dictionary<Type, Dictionary<string, object>> _objects = new Dictionary<Type, Dictionary<string, object>>();
        #endregion

        #region Init
        static DataManager()
        {
            switch (_ext)
            { 
                case "xml":
                    Serialization.SerializationType = SerializationType.Xml;
                    break;
                case "bin":
                    Serialization.SerializationType = SerializationType.Binary;
                    break;
                case "soap":
                    Serialization.SerializationType = SerializationType.Soap;
                    break;
            }
        }
        #endregion

        #region Private Member
        private static string _getFilename(Type objectType, string name, string ext)
        {
            string dir;

            Environment.CurrentDirectory = Path.GetDirectoryName(Application.ExecutablePath);

            if (objectType == null)
            {
                string[] allFiles = _getFilenames(objectType, ext);
                
                dir = Path.GetDirectoryName(
                    allFiles.FirstOrDefault(f => Path.GetFileNameWithoutExtension(f) == name)
                ) + @"\";

                if (dir == @"\") return null;
            }
            else
            {
                dir = @"Data\" + objectType.FullName + @"\";

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }

            return dir + name + "." + ext;
        }

        private static string[] _getFilenames(Type objectType, string ext, string dir = null)
        {
            if (dir == null) dir = @"Data\";
            if (objectType != null) dir += objectType.FullName + @"\";

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            List<string> files = new List<string>();

            foreach (string subDir in Directory.GetDirectories(dir))
            { 
                files.AddRange(
                    _getFilenames(objectType, ext, subDir)
                );
            }

            files.AddRange(
                Directory.GetFiles(dir).Where(f => Path.GetExtension(f) == "." + ext)
            );

            return files.ToArray();
        }

        private static Dictionary<string, object> _getTypeData<T>()
            where T : NameObject
        {
            Type t = typeof(T);

            if (!_objects.ContainsKey(t))
            {
                _objects[t] = new Dictionary<string, object>();
            }

            return _objects[t];
        }
        #endregion

        #region Member - Get
        public static T GetObject<T>(string name)
            where T : NameObject
        {
            var list = _getTypeData<T>();

            if (list.ContainsKey(name))
            {
                return (T)list[name];
            }

            return default(T);
        }

        public static T[] GetRange<T>()
            where T : NameObject
        {
            return _getTypeData<T>().Values.Cast<T>().ToArray();
        }
        #endregion

        #region Member - Set
        public static void SetObjectNonGeneric(object value)
        {
            if (value != null)
            {
                Type t = value.GetType();

                if (t.IsSubclassOf(typeof(NameObject)))
                {
                    typeof(DataManager).GetMethod("SetObject").Invoke(
                        null,
                        new object[] { value }
                    );
                }
            }
        }

        public static void SetObject<T>(T value)
            where T : NameObject
        {
            var type = value.GetType();
            var list = _getTypeData<T>();

            list[value.Name] = value;

            string filename = _getFilename(type, value.Name, _ext);

            File.WriteAllBytes(
                filename,
                Serialization.Serialize(
                    value
                )
            );
        }

        public static void SetRange<T>(IEnumerable<T> values)
            where T : NameObject
        {
            foreach (T value in values)
            {
                SetObject(value);
            }
        }
        #endregion

        #region Member - Load
        public static T LoadObject<T>(string name, bool newLoad)
            where T : NameObject
        {
            var list = _getTypeData<T>();

            if (!list.ContainsKey(name) || newLoad)
            {
                string filename = _getFilename(null, name, _ext);

                if (!File.Exists(filename)) return default(T);

                Type type = Assembly.GetExecutingAssembly().GetType(
                    Path.GetFileName(Path.GetDirectoryName(filename))
                );

                //if (_formatter is XmlFormatter)
                //{
                //    ((XmlFormatter)_formatter).ReadType = type;
                //}

                list[name] = Serialization.Deserialize<T>(
                    File.ReadAllBytes(filename)
                );
            }

            return (T)list[name];
        }

        public static T[] LoadObjectType<T>()
            where T : NameObject
        { 
            Type t = typeof(T);
            if (t == typeof(NameObject)) t = null;
            List<T> list = new List<T>();

            foreach (string filename in _getFilenames(t, _ext))
            {
                list.Add(
                    LoadObject<T>(
                        Path.GetFileNameWithoutExtension(filename),
                        false
                    )
                );                
            }

            return list.ToArray();
        }

        public static NameObject[] LoadObjectTypeNonGeneric(Type type)
        {
            return (NameObject[])typeof(DataManager).GetMethod("LoadObjectType").MakeGenericMethod(type).Invoke(null, null);
        }
        #endregion
        
        #region Member - Remove
        public static void RemoveObject<T>(T value)
            where T : NameObject
        {
            string filename = _getFilename(null, value.Name, _ext);

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
        }

        public static void RemoveObjectNonGeneric(object value)
        {
            if (value != null)
            {
                Type t = value.GetType();

                if (t.IsSubclassOf(typeof(NameObject)))
                {
                    typeof(DataManager).GetMethod("RemoveObject").Invoke(
                        null,
                        new object[] { value }
                    );
                }
            }
        }
        #endregion
    }
}