using Application.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Factories;
using Domain.Interfaces;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class DepartmentService
    {
        public readonly IDepartmentRepository _departmentRepository;
        public readonly IMapper _mapper;
        public DepartmentService(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }


        /// <summary>
        /// Devuelve todos los departamentos
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Department>> GetAllDeparmentsAsync()
        {
            var departments = await _departmentRepository.GetAllAsync();
            return departments;
        }

        /// <summary>
        /// Devuelve un departamento por id único de ese departamento.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Department> GetDepartmentsByIdAsync(int id)
        {
            // Obtener el empleado desde el repositorio
            var department = await _departmentRepository.GetByIdAsync(id);

            if (department == null)
                throw new Exception("Department not found.");

            return department;
        }

        /// <summary>
        /// Crea un empleado nuevo
        /// </summary>
        /// <param name="name"></param>
        /// <param name="currentPosition"></param>
        /// <param name="salary"></param>
        /// <returns></returns>
        public async Task<Department> CreateDepartmentAsync(DepartmentDto departmentDto)
        {
            var department = _mapper.Map<Department>(departmentDto);

            // Agrega el departamento a la base de datos
            await _departmentRepository.AddAsync(department);

            return department;
        }

        public async Task<Department> UpdateDepartmentAsync(int id, DepartmentDto departmentDto)
        {
            var department = _mapper.Map<Department>(departmentDto);

            // Actualiza el departamento en la base de datos
            await _departmentRepository.UpdateAsync(department);

            return department;
        }

        // borra el departamento del id
        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null)
                return false;

            await _departmentRepository.DeleteAsync(id);
            return true;
        }
    }
}
