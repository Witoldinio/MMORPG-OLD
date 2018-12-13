using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using AuthServer.Services.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Networking.PackageParser;

namespace AuthServer.Services
{
    public class PackageDispatcher : IPackageDispatcher
    {
        private readonly ILogger<PackageDispatcher> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IPackageParser _packageParser;
        private BlockingCollection<Tuple<ClientConnection, PackageBase>> queue = new BlockingCollection<Tuple<ClientConnection, PackageBase>>();
        private Thread _dispatcherThread;

        public bool Running { get; private set; }


        public PackageDispatcher(ILogger<PackageDispatcher> logger, IServiceProvider serviceProvider, IPackageParser packageParser)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _packageParser = packageParser ?? throw new ArgumentNullException(nameof(packageParser));
        }

        public void DispatchPackage(PackageBase package, ClientConnection connection)
        {
            queue.Add(new Tuple<ClientConnection, PackageBase>(connection, package));
        }

        public void Start()
        {
            _dispatcherThread = new Thread(() =>
            {
                _logger.LogInformation("PackageDispatcher started ...");
                Running = true;
                try
                {
                    while (Running)
                    {
                        if (queue.TryTake(out var item))
                        {
                            using (var scope = _serviceProvider.CreateScope())
                            {
                                var (connection, package) = item;
                                var connHandler = scope.ServiceProvider.GetRequiredService<ServerConnectionHandler>();
                                connHandler.InvokeAction(connection, package, package.Id);
                            }
                        }
                    }
                }
                finally
                {
                    _logger.LogError("PackageDispatcher stopped.");
                }
            })
            {
                IsBackground = true,
            };
            _dispatcherThread.Start();
        }
    }
}
