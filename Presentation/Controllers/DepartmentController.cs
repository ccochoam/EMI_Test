using Application.Dtos;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly DepartmentService _departmentService;

        public DepartmentController(DepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        // GET: api/department
        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            var departments = await _departmentService.GetAllDeparmentsAsync();
            return Ok(departments);
        }

        // GET: api/department/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            var claims = GetUserAuth();
            int role = int.Parse(claims.Item2);

            if (role == (int)UserRole.Admin)
            {
                var department = await _departmentService.GetDepartmentsByIdAsync(id);
                if (department == null)
                    return NotFound();

                return Ok(department);
            }
            else
                return Unauthorized();
        }


        // POST: api/department
        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentDto departmentDto)
        {
            var claims = GetUserAuth();
            int role = int.Parse(claims.Item2);

            if (role == (int)UserRole.Admin)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var department = await _departmentService.CreateDepartmentAsync(departmentDto);
                return Ok(department);
            }
            else
                return Unauthorized();
        }

        // PUT: api/department/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] DepartmentDto departmentDto)
        {
            var claims = GetUserAuth();
            int role = int.Parse(claims.Item2);

            if (role == (int)UserRole.Admin)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedDepartment = await _departmentService.UpdateDepartmentAsync(id, departmentDto);

                if (updatedDepartment == null)
                    return NotFound();

                return Ok(updatedDepartment);
            }
            else
                return Unauthorized();
        }

        // DELETE: api/department/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var claims = GetUserAuth();
            int role = int.Parse(claims.Item2);

            if (role == (int)UserRole.Admin)
            {
                var deleted = await _departmentService.DeleteDepartmentAsync(id);
                if (!deleted)
                    return NotFound();

                return Ok();
            }
            else
                return Unauthorized();
        }


        private (string, string) GetUserAuth()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            return JwtHelpers.GetUserByToken(token);
        }
    }
}
