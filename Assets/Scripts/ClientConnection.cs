using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class ClientConnection
    {
        private TcpClient _client;

        public bool IsConnected => _client?.Connected ?? false;

        public BinaryWriter Writer { get; private set; }
        public BinaryReader Reader { get; private set; }

        public ClientConnection()
        {

        }

        public Task Connect(string host, int port)
        {
            _client = new TcpClient();
            _client.Connect(host, port);

            var stream = _client.GetStream();
            Writer = new BinaryWriter(stream);
            Reader = new BinaryReader(stream);

            return Task.CompletedTask;
        }

        public Task Disconnect()
        {
            _client.Close();
            Writer = null;
            Reader = null;

            return Task.CompletedTask;
        }
    }
}
