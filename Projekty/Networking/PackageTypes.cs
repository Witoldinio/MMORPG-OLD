using System;

namespace Networking
{
    public enum PackageType : UInt32
    {
        LOGIN_REQUEST = 0x0001,
        LOGIN_RESPONSE = 0x0002,

        KEEP_ALLIVE = 0xFFFE,
        ERROR = 0xFFFF,
    }
}
