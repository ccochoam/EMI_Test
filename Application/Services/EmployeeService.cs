using Application.Dtos;
using Domain.Entities;
using Domain.Factories;
using Domain.Interfaces;

namespace Application.Services
{
    public class EmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPositionHistoryRepository _positionHistoryRepository;

        public EmployeeService(IEmployeeRepository employeeRepository, IPositionHistoryRepository positionHistoryRepository)
        {
            _employeeRepository = employeeRepository;
            _positionHistoryRepository = positionHistoryRepository;
        }

        /// <summary>
        /// Devuelve todos los empleados, con el histórico de posiciones que cada uno tiene asociadas
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            employees.ToList().ForEach(employee => employee.AssignBonusStrategy());
            return employees;
        }

        /// <summary>
        /// Devuelve un empleado por id único de ese empleado. Busca de forma directa en la tabla de PositionHistories la posición que ocupa actualmente según el valor de CurrentPosition y Id de PositionHistories
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            // Obtener el empleado desde el repositorio
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                throw new Exception("Employee not found.");

            employee.PositionHistories = new List<PositionHistory>();
            if (employee.CurrentPosition.HasValue)
                employee.PositionHistories.Add(await _positionHistoryRepository.GetByIdAsync(employee.CurrentPosition.Value));

            employee.AssignBonusStrategy();

            return employee;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesInDepartmentWithProjectsAsync(int departmentId)
        {
            var employee = await _employeeRepository.GetEmployeesInDepartmentWithProjectsAsync(departmentId);
            return employee;
        }

        /// <summary>
        /// Crea un empleado nuevo
        /// </summary>
        /// <param name="name"></param>
        /// <param name="currentPosition"></param>
        /// <param name="salary"></param>
        /// <returns></returns>
        public async Task<Employee> CreateEmployeeAsync(string name, int currentPosition, decimal salary, int? departmentId)
        {
            // Uso de Factory Pattern para crear un empleado según su posición en la empresa
            Employee employee = EmployeeFactory.CreateEmployee(0, name, currentPosition, salary, departmentId);

            //Luego de creado el objeto, se asigna una estrategia de cálculo del bonus según el porcentaje que corresponde a su cargo
            employee.AssignBonusStrategy();

            // Agrega el empleado a la base de datos
            await _employeeRepository.AddAsync(employee);

            return employee;
        }

        public async Task<Employee> UpdateEmployeeAsync(int id, EmployeeDto employeeDto)
        {
            // Uso de Factory Pattern para crear un empleado según su posición en la empresa
            Employee employee = EmployeeFactory.CreateEmployee(id, employeeDto.Name, employeeDto.CurrentPosition, employeeDto.Salary, employeeDto.departmentId);

            // Actualiza el empleado en la base de datos
            await _employeeRepository.UpdateAsync(employee);

            //Luego de creado el objeto, se asigna una estrategia de cálculo del bonus según el porcentaje que corresponde a su cargo. Automática
            employee.AssignBonusStrategy();

            return employee;
        }

        // Borra el empleado del Id
        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                return false;

            await _employeeRepository.DeleteAsync(id);
            return true;
        }
    }
}
