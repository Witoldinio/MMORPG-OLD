using AuthServer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthServer
{
    public class ConfigurationService
    {
        public IConfigurationRoot Configuration;

        public ServiceProvider ServiceProvider { get; private set; }

        private static ConfigurationService _instance;
        public static ConfigurationService Instance { get => _instance ?? (_instance = new ConfigurationService()); }

        public static ConfigurationService CreateInstance()
        {
            return CreateInstance((s) => { });
        }

        public static ConfigurationService CreateInstance(Action<IServiceCollection> handler)
        {
            ConfigurationService instance = new ConfigurationService();
            IServiceCollection descriptors = CreateDefaultServiceDescriptors();
            handler(descriptors);

            instance.ServiceProvider = descriptors.BuildServiceProvider();
            return instance;
        }

        private static IServiceCollection CreateDefaultServiceDescriptors()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("config.json")             //from Networking project
                .Build();

            IServiceCollection serviceDescriptor = new ServiceCollection();
            serviceDescriptor.AddLogging(l => l.AddNLog());
            serviceDescriptor.AddSingleton<IConfigurationRoot>(configuration);

            return serviceDescriptor;
        }
    }
}
