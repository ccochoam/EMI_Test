using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Helpers
{
    /// <summary>
    /// Clase donde se maneja la lógica del token. Se crea y se valida en esta clase.
    /// </summary>
    public class JwtHelpers
    {
        public static string GetToken(string username, string role, string secretKey, string issuer, string audience, string expiration)
        {
            var userClaims = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, username), new Claim(ClaimTypes.Role, role) });

            var token = GenerateToken(secretKey, issuer, audience, userClaims, DateTime.UtcNow.AddHours(Convert.ToDouble(expiration)));
            return token;
        }
        public static string GenerateToken(string secretKey, string issuer, string audience, ClaimsIdentity claimsIdentity, DateTime expiration)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = issuer,
                Audience = audience,
                Subject = claimsIdentity,
                Expires = expiration,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static (string, string) GetUserByToken(string token)
        {
            if (!string.IsNullOrEmpty(token) && (token.StartsWith("Bearer ") || token.StartsWith("bearer ")))
                token = token.Substring("Bearer ".Length);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("uBtAbOs/RgDuGK7+NnzRgHvX5Gt6lgg/OG5UYnfwJmw=");

            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

                // Obtener el correo del token
                var emailClaim = principal.FindFirst(ClaimTypes.Email)?.Value;
                var roleClaim = principal.FindFirst(ClaimTypes.Role)?.Value;
                if (emailClaim != null && roleClaim != null)
                    return (emailClaim, roleClaim);
                return (null, null);
            }
            catch (Exception ex)
            {
                // Manejar errores de validación del token
                Console.WriteLine($"Error al validar el token: {ex.Message}");
                return (ex.Message, null);
            }
        }

        public static ClaimsPrincipal ValidateToken(string token, string secretKey, string issuer, string audience)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidIssuer = issuer,
                ValidAudience = audience
            };

            try
            {
                SecurityToken validatedToken;
                return tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            }
            catch (Exception ex)
            {
                // Manejo de la excepción en caso de que la validación del token falle.
                // Podrías registrar el error, devolver un valor predeterminado o lanzar una excepción personalizada según tus necesidades.
                return null;
            }
        }
    }
}
