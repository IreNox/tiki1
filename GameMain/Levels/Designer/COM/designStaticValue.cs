#if DESIGNER
using System;
using System.Runtime.Serialization;

namespace TikiEngine.Levels.Designer
{
    [Serializable]
    internal class designStaticValue : DesignActionVar
    {
        #region Vars
        private Type _type;

        private object _value;
        #endregion

        #region Init
        public designStaticValue(Type type, object value)
        {
            _type = type;
            _value = value;
        }

        public designStaticValue(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Member
        public override string GenerateCode()
        {
            if (_value == null) return "null";

            Type type = _value.GetType();

            switch (type.Name)
            {
                case "Byte":
                case "Int16":
                case "Int32":
                case "Int64":
                    return _value.ToString();
                case "Single":
                    return ((float)_value).ToString("F").Replace(',', '.') + 'f';
                case "Double":
                    return ((double)_value).ToString("F").Replace(',', '.') + 'd';
                case "Decimal":
                    return ((decimal)_value).ToString("F").Replace(',', '.') + 'm';
                case "String":
                    return "@\"" + _value + "\"";
                default:
                    throw new Exception();
            }
        }
        #endregion

        #region Properties
        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public override Type VarType
        {
            get { return _type; }
            set { _type = value; }
        }
        #endregion
    }
}
#endif