using AuthServer.Models;
using System.Collections.Generic;

namespace AuthServer.Business
{
    public interface IUserRepository
    {
        User Get(int id);
        List<User> Get();
        void Add(User user);
        void Delete(int id);
        void Update(User user);
        bool Exists(string username);

        (bool success, IEnumerable<Character> characters) GetCharacters(int id);
        (bool, int) PasswordOK(string username, string password);
    }
}
