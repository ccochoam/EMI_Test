using Application.Dtos;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Domain.Mediator;
using Infrastructure.Helpers;
using Infrastructure.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    /// <summary>
    /// Controlador con los métodos CRUD para la administración de los empleados.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly EmployeeService _employeeService;
        private readonly IRequestHandler<GetEmployeeRequest, Employee> _getEmployeeHandler;

        public EmployeeController(EmployeeService employeeService, IRequestHandler<GetEmployeeRequest, Employee> getEmployeeHandler)
        {
            _employeeService = employeeService;
            _getEmployeeHandler = getEmployeeHandler;
        }

        // GET: api/employee
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        // GET: api/employee/{id}
        //[Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var claims = GetUserAuth();
            int role = int.Parse(claims.Item2);

            if (role == (int)UserRole.Admin)
            {
                var employee = await _employeeService.GetEmployeeByIdAsync(id);
                if (employee == null)
                    return NotFound();

                return Ok(employee);
            }
            else
                return Unauthorized();

        }

        // POST: api/employee
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeDto employeeDto)
        {
            var claims = GetUserAuth();
            int role = int.Parse(claims.Item2);

            if (role == (int)UserRole.Admin)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var employee = await _employeeService.CreateEmployeeAsync(employeeDto.Name, employeeDto.CurrentPosition, employeeDto.Salary);
                return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, employee);
            }
            else
                return Unauthorized();
        }

        // PUT: api/employee/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeDto employeeDto)
        {
            var claims = GetUserAuth();
            int role = int.Parse(claims.Item2);

            if (role == (int)UserRole.Admin)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedEmployee = await _employeeService.UpdateEmployeeAsync(id, employeeDto);

                if (updatedEmployee == null)
                    return NotFound();

                return Ok(updatedEmployee);
            }
            else
                return Unauthorized();
        }

        // DELETE: api/employee/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var claims = GetUserAuth();
            int role = int.Parse(claims.Item2);

            if (role == (int)UserRole.Admin)
            {
                var deleted = await _employeeService.DeleteEmployeeAsync(id);
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
