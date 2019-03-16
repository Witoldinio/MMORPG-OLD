namespace Networking
{
    public class Encryption
    {
        public static int PASSWORD_BCRYPT_COST = 13;
        public static string PASSWORD_SALT = "/8Wncr26eAmxD1l6cAF9F8";
        public static string Salt = "$2a$" + PASSWORD_BCRYPT_COST + "$" + PASSWORD_SALT;

        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, Encryption.Salt);
        }

        public static bool ValidatePassword(string password, string correctHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, correctHash);
        }
    }
}
