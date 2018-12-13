using AuthServer.Business;
using AuthServer.Services.Models;
using Microsoft.Extensions.Logging;
using Networking;
using Networking.PackageParser;
using Networking.PackageParser.Implementations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthServer
{
    public class ServerConnectionHandler : ConnectionHandlerBase<ClientConnection>
    {
        private readonly ILogger<ServerConnectionHandler> _logger;
        private readonly IPackageParser _packageParser;
        private readonly IUserRepository _userRepository;

        public ServerConnectionHandler(ILogger<ServerConnectionHandler> logger, IPackageParser packageParser, IUserRepository userRepository)
        {
            _logger = logger;
            _packageParser = packageParser;
            _userRepository = userRepository;
        }

        protected override void HandleUnknownPackage(ClientConnection connection, object parsedData, PackageType type)
        {
            throw new NotImplementedException();
        }

        [PackageHandler(PackageType.LOGIN_REQUEST)]
        public void HandleLogin(ClientConnection connection, LoginRequestPackage parseObjectData)
        {
            _logger.LogInformation("Login Request");
            _logger.LogDebug($"Login data Username: {parseObjectData.Username}, Password: {parseObjectData.Password}");

            if (!_userRepository.Exists(parseObjectData.Username))
            {
                return;
            }

            (var passwordOK, int userId) = _userRepository.PasswordOK(parseObjectData.Username, parseObjectData.Password);
            if (!passwordOK)
            {
                _logger.LogInformation("Wrong password.");
                return;
            }

            _packageParser.ParsePackageToStream(new LoginResponsePackage { IsValid = true }, connection.Writer);
        }

        [PackageHandler(PackageType.ERROR)]
        public void HandleError(ClientConnection connection, object parseObjectData)
        {
            // handle error
        }
    }
}
