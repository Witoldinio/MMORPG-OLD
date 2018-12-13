using System;

namespace Networking
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PackageHandlerAttribute : Attribute
    {
        public PackageType Type { get; }

        public PackageHandlerAttribute(PackageType type)
        {
            Type = type;
        }
    }
}