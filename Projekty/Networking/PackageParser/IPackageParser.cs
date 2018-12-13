using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Networking.PackageParser
{
    public interface IPackageParser
    {
        PackageBase ParsePackageFromStream(BinaryReader reader);
        void ParsePackageToStream(PackageBase package, BinaryWriter writer);
    }
}
