using Application.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProjectService
    {
        public readonly IProjectRepository _projectRepository;
        public readonly IMapper _mapper;

        public ProjectService(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Devuelve todos los proyectos
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            var projects = await _projectRepository.GetAllAsync();
            return projects;
        }

        /// <summary>
        /// Devuelve un proyecto por id único de ese proyecto.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Project> GetProjectByIdAsync(int id)
        {
            // Obtener el empleado desde el repositorio
            var project = await _projectRepository.GetByIdAsync(id);

            if (project == null)
                throw new Exception("Project not found.");

            return project;
        }

        /// <summary>
        /// Crea un empleado nuevo
        /// </summary>
        /// <param name="name"></param>
        /// <param name="currentPosition"></param>
        /// <param name="salary"></param>
        /// <returns></returns>
        public async Task<Project> CreateProjectAsync(ProjectDto projectDto)
        {
            var project = _mapper.Map<Project>(projectDto);

            // Agrega el proyecto a la base de datos
            await _projectRepository.AddAsync(project);

            return project;
        }

        public async Task<Project> UpdateProjectAsync(int id, ProjectDto projectDto)
        {
            var project = _mapper.Map<Project>(projectDto);

            // Actualiza el proyecto en la base de datos
            await _projectRepository.UpdateAsync(project);

            return project;
        }

        /// <summary>
        /// Borra el proyecto del id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteProjectAsync(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null)
                return false;

            await _projectRepository.DeleteAsync(id);
            return true;
        }
    }
}
