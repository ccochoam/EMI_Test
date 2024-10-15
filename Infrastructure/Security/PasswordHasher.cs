using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Security
{
    /// <summary>
    /// clase donde se encripta y donde se aplica el hashing a la contraseña del usuario
    /// </summary>
    public class PasswordHasher
    {
        public static string EncryptKey(string key, string salt)
        {
            string hashedPassword = HashPassword(key, salt);
            return hashedPassword;
        }

        private static string HashPassword(string password, string salt)
        {
            var passwordHasher = new PasswordHasher<object>();
            return passwordHasher.HashPassword(salt, password);
        }
    }
}
