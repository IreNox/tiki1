using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization.Formatters.Binary;

namespace TikiEngine
{
    #region Enum - SerializationType
    internal enum SerializationType
    {
        Xml,
        Soap,
        Binary
    }
    #endregion

    public static class Serialization
    {
        #region Vars
        private static IFormatter _formatter;

        private static SerializationType _type = SerializationType.Binary;
        #endregion

        #region Member - File Serialization
        internal static byte[] Serialize(object obj)
        {
            var stream = new MemoryStream();

            try
            {
                _formatter.Serialize(
                    stream,
                    obj
                );

                return stream.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                stream.Dispose();
            }
        }

        internal static T Deserialize<T>(byte[] data)
        {
            var stream = new MemoryStream(data);

            try
            {
                object obj = _formatter.Deserialize(
                    stream
                );

                return (T)obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                stream.Dispose();
            }
        }
        #endregion

        #region Member - Object Serialization
        public static void ObjectDeserialize(object obj, SerializationInfo info)
        { 
            Type type = obj.GetType();

            foreach (SerializationEntry entry in info)
            {
                PropertyInfo prop = type.GetProperty(entry.Name);

                if (prop != null)
                {
                    prop.SetValue(obj, entry.Value, null);
                }
            }
        }

        public static void ObjectSerialize(object obj, SerializationInfo info)
        {
            PropertyInfo[] infos = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in infos)
            {
                if (prop.GetCustomAttributes(typeof(NonSerializedTikiAttribute), true).Length == 0 && prop.CanRead && prop.CanWrite)
                {
                    info.AddValue(
                        prop.Name,
                        prop.GetValue(obj, null)
                    );
                }
            }
        }
        #endregion

        #region Properties
        internal static SerializationType SerializationType
        {
            get { return _type; }
            set
            {
                _type = value;

                switch (_type)
                {
                    case SerializationType.Xml:
                        _formatter = new XmlFormatter();
                        break;
                    case SerializationType.Soap:
                        _formatter = new SoapFormatter();
                        break;
                    case SerializationType.Binary:
                        _formatter = new BinaryFormatter();
                        break;
                }
            }
        }
        #endregion

        #region Class - XmlFormatter
        internal class XmlFormatter : IFormatter
        {
            #region Vars
            private Type _readType;

            private Dictionary<Type, XmlSerializer> _serializer = new Dictionary<Type, XmlSerializer>();
            #endregion

            #region Init
            public XmlFormatter()
            {
            }
            #endregion

            #region Private Member
            private XmlSerializer _getSerializer(Type type)
            {
                if (!_serializer.ContainsKey(type))
                {
                    _serializer[type] = new XmlSerializer(type);
                }

                return _serializer[type];
            }
            #endregion

            #region Member
            public object Deserialize(Stream stream)
            {
                XmlSerializer ser = _getSerializer(_readType);

                return ser.Deserialize(stream);
            }

            public void Serialize(Stream stream, object graph)
            {
                Type type = graph.GetType();
                XmlSerializer ser = _getSerializer(type);

                ser.Serialize(stream, graph);
            }
            #endregion

            #region Propertys
            public Type ReadType
            {
                get { return _readType; }
                set { _readType = value; }
            }

            public SerializationBinder Binder
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public StreamingContext Context
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public ISurrogateSelector SurrogateSelector
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }
            #endregion
        }
        #endregion
    }
}
