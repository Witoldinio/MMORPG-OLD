using Microsoft.Extensions.DependencyInjection;
using Networking.PackageParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public static class GameContext
    {
        public static ConfigurationService ConfigurationService { get; }
        public static IServiceProvider ServiceProvider => ConfigurationService.ServiceProvider;

        static GameContext()
        {
            ConfigurationService = ConfigurationService.CreateInstance(s =>
            {
                s.AddSingleton<IPackageParser, PackageParser>();
                s.AddSingleton<ClientConnection>();
            });
        }

    }
}
