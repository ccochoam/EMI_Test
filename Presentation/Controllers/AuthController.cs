using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly AuthService _authService;
        private readonly IConfiguration _configuration;
        public AuthController(AuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }

        /// <summary>
        /// Endpoint para validar el inicio de sesión de un usuario. Genera token jwt cuando el usuario y la contraseña son correctas.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDto user)
        {
            if (user == null)
                return BadRequest("User model is null");

            try
            {
                var result = await _authService.Authenticate(user.Username, user.Password);

                if (result.Success)
                {
                    var token = Infrastructure.Helpers.JwtHelpers.GetToken(result.Result.Username, result.Result.Role.ToString(), _configuration["Jwt:SecretKey"], _configuration["Jwt:Issuer"],
                            _configuration["Jwt:Audience"], _configuration["Jwt:ExpirationHours"]);
                    var response = new { Token = token };
                    return Ok(response);
                }
                return StatusCode(401, "Usuario y/o contraseña incorrecta");
            }
            catch (Exception ex)
            {
                return BadRequest("Internal Server Error");
            }
        }
    }
}
