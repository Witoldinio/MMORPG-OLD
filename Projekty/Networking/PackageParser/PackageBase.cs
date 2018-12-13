using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Networking.PackageParser
{
    public abstract partial class PackageBase
    {
        public PackageType Id { get; }

        public PackageBase(PackageType id)
        {
            Id = id;
        }

        public abstract void SerializeToStream(BinaryWriter writer);

        public abstract void DeserializeFromStream(BinaryReader reader);
    }
}
