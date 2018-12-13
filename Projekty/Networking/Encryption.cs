namespace Networking
{
    public class Encryption
    {
        public static int PASSWORD_BCRYPT_COST = 13;
        public static string PASSWORD_SALT = "wyjątkowo_trudne_hasło@%$@@!@%UUY^^*(OIUB";
        public static string salt = "$876758$" + PASSWORD_BCRYPT_COST + "$" + PASSWORD_SALT;

        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, Encryption.salt);
        }

        public static bool ValidatePassword(string password, string correctHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, correctHash);
        }
    }
}
