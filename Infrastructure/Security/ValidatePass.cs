using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Security
{
    /// <summary>
    /// Clase para validar la contraseña del usuario a la hora de hacer el inicio de sesión
    /// </summary>
    public class ValidatePass
    {
        public static bool ValidatePassword(string password, string hashedPassword, string hashKey)
        {
            var passwordHasher = new PasswordHasher<object>();
            var result = passwordHasher.VerifyHashedPassword(hashKey, hashedPassword, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
