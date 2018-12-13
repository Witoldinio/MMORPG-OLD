using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Networking.PackageParser
{
    public class PackageParser : IPackageParser
    {
        private readonly ILogger<PackageParser> _logger;
        private Dictionary<PackageType, Type> _packageTypes = new Dictionary<PackageType, Type>();

        public PackageParser(ILogger<PackageParser> logger)
        {
            _logger = logger;
        }

        private void ResolvePackage()
        {
            var packageClasses = GetType().Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(PackageBase)));

            foreach (var @class in packageClasses)
            {
                var attribute = @class.GetCustomAttributes(typeof(PackageTypeAttributeAttribute), false);
                if (attribute.FirstOrDefault() is PackageTypeAttributeAttribute packageTypeAttribute)
                {
                    _packageTypes.Add(packageTypeAttribute.Type, @class);
                }
            }

            _logger.LogInformation($"Scanned {_packageTypes.Count} packages.");
        }

        public PackageBase ParsePackageFromStream(BinaryReader reader)
        {
            var packageType = (PackageType)reader.ReadUInt32();

            if (_packageTypes.TryGetValue(packageType, out var type))
            {
                var package = Activator.CreateInstance(type) as PackageBase;
                package.DeserializeFromStream(reader);
                _logger.LogInformation($"receiver package from stream type: {package.GetType()}.");

                return package;
            }

            throw new InvalidOperationException("Package type is unknown.");
        }

        public void ParsePackageToStream(PackageBase package, BinaryWriter writer)
        {
            _logger.LogInformation($"Write package to stream type: {package.GetType()}.");
            package.SerializeToStream(writer);
            writer.Flush();
        }
    }
}
