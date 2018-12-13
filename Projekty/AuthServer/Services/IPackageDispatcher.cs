using AuthServer.Services.Models;
using Networking.PackageParser;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthServer.Services
{
    public interface IPackageDispatcher
    {
        bool Running { get; }
        void DispatchPackage(PackageBase package, ClientConnection connection);
        void Start();
    }
}
