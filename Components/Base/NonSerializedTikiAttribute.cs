using System;

namespace TikiEngine
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NonSerializedTikiAttribute : Attribute
    {
        public NonSerializedTikiAttribute()
        {
        }
    }
}
