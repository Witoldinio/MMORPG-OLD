using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Networking.PackageParser.Implementations
{
    [PackageTypeAttribute(PackageType.KEEP_ALLIVE)]
    public class KeepAllivePackage : PackageBase
    {
        public KeepAllivePackage() : base(PackageType.KEEP_ALLIVE)
        {

        }

        public override void DeserializeFromStream(BinaryReader reader)
        {
        }

        public override void SerializeToStream(BinaryWriter writer)
        {
            writer.Write((UInt32)Id);
        }
    }
}
