namespace Infrastructure.Security
{
    /// <summary>
    /// Clase para generar el SALT de para el hashing de las contraseñas
    /// </summary>
    public class SaltGenerator
    {
        public static string GenerateSalt()
        {
            int length = 16;
            // Caracteres posibles para el salt
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            // Usar el sistema de generación de números aleatorios seguro
            var random = new Random();

            // Generar el salt seleccionando caracteres aleatorios del conjunto
            var salt = new char[length];
            for (int i = 0; i < length; i++)
            {
                salt[i] = chars[random.Next(chars.Length)];
            }

            return new string(salt);
        }
    }
}
