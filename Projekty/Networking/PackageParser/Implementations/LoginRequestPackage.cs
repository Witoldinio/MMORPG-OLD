using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Networking.PackageParser.Implementations
{
    [PackageTypeAttribute(PackageType.LOGIN_REQUEST)]
    public class LoginRequestPackage : PackageBase
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public LoginRequestPackage() : base(PackageType.LOGIN_REQUEST)
        {

        }

        public override void DeserializeFromStream(BinaryReader reader)
        {
            Username = reader.ReadString();
            Password = reader.ReadString();
        }

        public override void SerializeToStream(BinaryWriter writer)
        {
            writer.Write((UInt32)Id);
            writer.Write(Username);
            writer.Write(Password);
        }
    }
}
