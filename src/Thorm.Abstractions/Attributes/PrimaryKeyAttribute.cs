using System;

namespace Thorm
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class PrimaryKeyAttribute : Attribute
    {
        public string Name { get; set; }

        public PrimaryKeyAttribute() { }
    }
}