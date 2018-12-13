using System;

namespace Networking
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PackageTypeAttributeAttribute : Attribute
    {
        public PackageType Type { get; }
        public PackageTypeAttributeAttribute(PackageType type)
        {
            Type = type;
        }
    }
}
