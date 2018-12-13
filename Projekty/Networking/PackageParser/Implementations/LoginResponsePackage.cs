using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Networking.PackageParser.Implementations
{
    [PackageTypeAttribute(PackageType.LOGIN_RESPONSE)]
    public class LoginResponsePackage : PackageBase
    {
        public bool IsValid { get; set; }

        public LoginResponsePackage() : base(PackageType.LOGIN_RESPONSE)
        {

        }

        public override void DeserializeFromStream(BinaryReader reader)
        {
            IsValid = reader.ReadBoolean();
        }

        public override void SerializeToStream(BinaryWriter writer)
        {
            writer.Write((UInt32)Id);
            writer.Write(IsValid);
        }
    }
}
