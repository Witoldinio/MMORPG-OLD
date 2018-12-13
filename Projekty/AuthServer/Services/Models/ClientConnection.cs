using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace AuthServer.Services.Models
{
    public class ClientConnection
    {
        private TcpClient _tcpClient;
        private NetworkStream _stream;

        public Guid ConnectionId { get; private set; }
        public BinaryReader Reader { get; }
        public BinaryWriter Writer { get; }

        public int AvailableBytes => _tcpClient.Available;
        public bool IsConnected => _tcpClient.Connected;

        public ClientConnection(TcpClient tcpClient, Guid connectionId)
        {
            _tcpClient = tcpClient ?? throw new ArgumentNullException(nameof(tcpClient));
            ConnectionId = connectionId;
            _stream = _tcpClient.GetStream();
            Reader = new BinaryReader(_stream);
            Writer = new BinaryWriter(_stream);
        }
    }
}
