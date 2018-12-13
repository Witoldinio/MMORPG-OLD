using AuthServer.Business;
using AuthServer.Services;
using Microsoft.Extensions.DependencyInjection;
using Networking.PackageParser;
using System;

namespace AuthServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting server...");
            var configurationServices = ConfigurationService.CreateInstance(serviceDescriptors =>
            {
                serviceDescriptors.AddTransient<IPackageParser, PackageParser>();
                serviceDescriptors.AddSingleton<IPackageDispatcher, PackageDispatcher>();
                serviceDescriptors.AddScoped<ServerConnectionHandler>();
                serviceDescriptors.AddSingleton<NetworkService>();
                serviceDescriptors.AddScoped<IUserRepository, UserRepository>();
            });

            configurationServices.ServiceProvider.GetRequiredService<NetworkService>().Start();
            configurationServices.ServiceProvider.GetRequiredService<IPackageDispatcher>().Start();

            Console.ReadLine();
        }
    }
}
