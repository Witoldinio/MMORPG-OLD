using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AuthServer.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Networking;

namespace AuthServer.Business
{
    public class UserRepository : IUserRepository
    {
        private IConfigurationRoot _configuration;
        private IDbConnection _connection { get => new MySqlConnection(_configuration.GetConnectionString("MySQL")); }
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(ILogger<UserRepository> logger, IConfigurationRoot configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public void Add(User user)
        {
            using (IDbConnection connection = _connection)
            {
                string query = @"INSERT INTO Users (Username, Password, Email) VALUES (@Username, @Password, @Email);";
                var result = connection.Execute(query, new { Username = user.Username, Password = user.Password, Email = user.Email });
            }
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string username)
        {
            using (IDbConnection connection = _connection)
            {
                string query = @"SELECT * FROM Users WHERE Username = @username AND IsActive = 1;";

                connection.Open();
                User user = connection.Query<User>(query, new { username }).FirstOrDefault();

                if (null != user)
                {
                    _logger.LogInformation($"User {user.Username} exists.");
                    return true;
                }

                _logger.LogInformation($"User {user.Username} does not exists.");
                return false;
            }
        }

        public User Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<User> Get()
        {
            throw new NotImplementedException();
        }

        public (bool success, IEnumerable<Character> characters) GetCharacters(int id)
        {
            throw new NotImplementedException();
        }

        public (bool, int) PasswordOK(string username, string password)
        {
            using (IDbConnection connection = _connection)
            {
                string query = @"SELECT * FROM Users WHERE Username = @username AND IsActive = 1;";

                connection.Open();
                User user = connection.Query<User>(query, new { username = username}).FirstOrDefault();
                
                if (Encryption.ValidatePassword(password, user.Password))
                {
                    _logger.LogInformation($"Password for user {user.Username} is correct.");
                    return (true, user.Id);
                }

                _logger.LogInformation($"Password for user {user.Username} is incorrect.");
                return (false, -1);
            }
        }

        public void Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}
