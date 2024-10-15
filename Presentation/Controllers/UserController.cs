using Application.Dtos;
using Application.Services;
using Domain.Enums;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Presentation.Controllers
{
    /// <summary>    /// 
    /// Controlador con los métodos CRUD para la administración de los usuarios.
    /// </summary>
    [ApiController]
    [Route("api/user")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDto userDTO)
        {
            if (userDTO == null)
                return BadRequest("User model is null");

            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return BadRequest(errors);
                }
                var res = await _userService.CreateUserAsync(userDTO);
                if (res.Success)
                {
                    return Ok(res.Result);
                }
                return StatusCode(500, "Error saving User in database");
            }
            catch (Exception ex)
            {
                return BadRequest("Internal Server Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var claims = GetUserAuth();
            string username = claims.Item1;
            int role = int.Parse(claims.Item2);
            try
            {
                if (role == (int)UserRole.Admin)
                {
                    var res = await _userService.GetAllUsersAsync();
                    if (res.Success)
                        return Ok(res.Result);
                    return NoContent();
                }
                else
                    return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest("Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            if (id == 0)
                return BadRequest("Id is null");

            var claims = GetUserAuth();
            try
            {
                var res = await _userService.GetUserByIdAsync(id);
                if (res.Success)
                    return Ok(res.Result);

                return StatusCode(404, "User not found");
            }
            catch (Exception ex)
            {
                return BadRequest("Internal Server Error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDto userDTO)
        {
            if (userDTO == null)
                return BadRequest("User model is null");

            if (id != userDTO.Id)
                return BadRequest("Id parameter is not equal to the Id parameter in the model.");

            try
            {

                var res = await _userService.UpdateUserAsync(id, userDTO);
                if (res.Success)
                    return Ok(res.Result);

                if (res.StatusCode == 400)
                    return StatusCode(400, res.Message);

                return StatusCode(500, "Internal Server Error");
            }
            catch (Exception ex)
            {
                return BadRequest("Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var claims = GetUserAuth();
            try
            {
                if (id == 0)
                    return BadRequest("Id is null");

                var res = await _userService.DeleteUserAsync(id);
                if (res.Success)
                    return Ok();

                return StatusCode(500, "Internal Server Error");
            }
            catch (Exception ex)
            {
                return BadRequest("Internal Server Error");
            }
        }

        private (string, string) GetUserAuth()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            return JwtHelpers.GetUserByToken(token);
        }
    }
}
