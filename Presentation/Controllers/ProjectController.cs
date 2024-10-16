using Application.Dtos;
using Application.Services;
using Domain.Enums;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace Presentation.Controllers
{
    /// <summary>
    /// Controlador con los métodos CRUD para la administración de los proyectos.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly ProjectService _projectService;

        public ProjectController(ProjectService projectService) 
        {
            _projectService = projectService;
        }

        // GET: api/project
        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            return Ok(projects);
        }

        // GET: api/project/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var claims = GetUserAuth();
            int role = int.Parse(claims.Item2);

            if (role == (int)UserRole.Admin)
            {
                var project = await _projectService.GetProjectByIdAsync(id);
                if (project == null)
                    return NotFound();

                return Ok(project);
            }
            else
                return Unauthorized();
        }


        // POST: api/project
        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] ProjectDto projectDto)
        {
            var claims = GetUserAuth();
            int role = int.Parse(claims.Item2);

            if (role == (int)UserRole.Admin)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var project = await _projectService.CreateProjectAsync(projectDto);
                return Ok(project);
            }
            else
                return Unauthorized();
        }

        // PUT: api/project/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] ProjectDto projectDto)
        {
            var claims = GetUserAuth();
            int role = int.Parse(claims.Item2);

            if (role == (int)UserRole.Admin)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedProject = await _projectService.UpdateProjectAsync(id, projectDto);

                if (updatedProject == null)
                    return NotFound();

                return Ok(updatedProject);
            }
            else
                return Unauthorized();
        }

        // DELETE: api/project/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var claims = GetUserAuth();
            int role = int.Parse(claims.Item2);

            if (role == (int)UserRole.Admin)
            {
                var deleted = await _projectService.DeleteProjectAsync(id);
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
