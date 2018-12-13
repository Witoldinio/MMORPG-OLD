using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using AuthServer.Services.Models;
using Networking.PackageParser;
using Microsoft.Extensions.Configuration;
using Networking.PackageParser.Implementations;

namespace AuthServer.Services
{
    internal class NetworkService
    {
        private static int _taskDelayTime = 100;
        private static int _port = 9876;

        private readonly object AddRemoveLocker = new object();
        private readonly ILogger<NetworkService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IPackageParser _packageParser;
        private readonly IPackageDispatcher _packageDispatcher;
        private List<ClientConnection> _clients = new List<ClientConnection>();
        private List<ClientConnection> _invalidConnections = new List<ClientConnection>();

        private TcpListener _tcpListener;
        private Thread _listenerThread;
        private Thread _receiverThread;

        private int _packageCounter = 0;

        public bool Running { get; set; }
        public event Action<TcpClient> Client;

        public NetworkService(IConfigurationRoot configuration, ILogger<NetworkService> logger, IServiceProvider serviceProvider, IPackageParser packageParser, IPackageDispatcher packageDispatcher)
        {
            _tcpListener = new TcpListener(IPAddress.Parse(configuration.GetValue<string>("host")), configuration.GetValue<int>("port"));
            Running = false;
            _logger = logger;
            _serviceProvider = serviceProvider;
            _packageParser = packageParser;
            _packageDispatcher = packageDispatcher;
        }

        public NetworkService(IPAddress address, int port, ILogger<NetworkService> logger, IServiceProvider serviceProvider, IPackageParser packageParser, IPackageDispatcher packageDispatcher)
        {
            _tcpListener = new TcpListener(address, port);
            Running = false;
            _logger = logger;
            _serviceProvider = serviceProvider;
            _packageParser = packageParser;
            _packageDispatcher = packageDispatcher;
        }

        public void Start()
        {
            if (null != _listenerThread && ThreadState.Running == _listenerThread.ThreadState)
            {
                return; //the server is already running
            }

            _listenerThread = new Thread(async () =>
            {
                Running = true;
                _tcpListener.Start();
                _logger.LogInformation("NetworkService successfully started.");
                try
                {
                    while (Running)
                    {
                        await Task.Delay(_taskDelayTime);
                        if (Running && _tcpListener.Pending())
                        {
                            OnClientConnected(await _tcpListener.AcceptTcpClientAsync());
                        }
                    }
                }
                finally
                {
                    _logger.LogError("Server stopped...");
                }
            })
            {
                IsBackground = true,
            };

            _listenerThread.Start();

            _receiverThread = new Thread(OnPackageReceive)
            {
                IsBackground = true,
            };

            _receiverThread.Start();
        }

        private async void OnPackageReceive(object obj)
        {
            try
            {
                _logger.LogInformation("NetworkService.ReceiveThread successfully started.");
                while (Running)
                {
                    await Task.Delay(1);
                    if (Running)
                    {
                        lock (AddRemoveLocker)
                        {
                            if (++_packageCounter == 1000)
                            {
                                _packageCounter = 0;
                                foreach (ClientConnection client in _clients)
                                {
                                    try
                                    {
                                        _packageParser.ParsePackageToStream(new KeepAllivePackage(), client.Writer);
                                    }
                                    catch (Exception e)
                                    {
                                        //_invalidConnections.Add(client);
                                        _clients.Remove(client);
                                        _logger.LogWarning($"NetworkServices.OnPackageReceive KEEP ALIVE Exception: {e.Message}");
                                    }
                                }
                            }

                            foreach (ClientConnection client in _clients)
                            {
                                if (0 < client.AvailableBytes)
                                {
                                    _logger.LogInformation($"Package from client {client.ConnectionId}");
                                    PackageBase package = _packageParser.ParsePackageFromStream(client.Reader);
                                    _packageDispatcher.DispatchPackage(package, client);
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                _logger.LogError("NetworkServices stopped.");
            }
        }

        public void Stop()
        {
            if (null == _listenerThread)
            {
                return; //the server is already stopped
            }

            Running = false;
            _listenerThread.Abort();
            _listenerThread = null;
            _receiverThread.Abort();
            _receiverThread = null;
            _tcpListener.Stop();
        }

        protected virtual void OnClientConnected(TcpClient connection)
        {
            Client?.Invoke(connection);
            ClientConnection client = new ClientConnection(connection, Guid.NewGuid());
            lock(AddRemoveLocker)
            {
                _clients.Add(client);
            }
            _logger.LogInformation($"New client connection GUID: {client.ConnectionId}");
        }
    }
}