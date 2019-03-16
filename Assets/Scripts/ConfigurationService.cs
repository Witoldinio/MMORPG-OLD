using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Assets.Scripts
{
    public class ConfigurationService
    {
        public ServiceProvider ServiceProvider { get; private set; }

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
            IServiceCollection serviceDescriptor = new ServiceCollection();
            serviceDescriptor.AddLogging(l => l.AddConsole());

            return serviceDescriptor;
        }
    }
}
