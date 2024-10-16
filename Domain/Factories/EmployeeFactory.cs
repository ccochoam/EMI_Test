using Domain.Entities;
using Domain.Enums;

namespace Domain.Factories
{
    /// <summary>
    /// clase "fábrica" para encapsular la creación de objetos. Se pretende permitir la inversión de contro e inyección de dependencias al tener centralizado la creación del objeto según su posición (cargo).
    /// </summary>
    public class EmployeeFactory
    {
        public static Employee CreateEmployee(int id, string name, int positionId, decimal salary, int? departmentId, int employeeType = 0)
        {
            // Lógica basada en el cargo del empleado
            if (employeeType == (int)EmployeeType.ManagerEmployee)  // ID de Gerente
            {
                return new ManagerEmployee
                {
                    Id = id,
                    Name = name,
                    Salary = salary,
                    CurrentPosition = positionId,
                    DepartmentId = departmentId
                };
            }
            else if (employeeType == (int)EmployeeType.RegularEmployee)  // ID de Empleado regular
            {
                return new RegularEmployee
                {
                    Id = id,
                    Name = name,
                    Salary = salary,
                    CurrentPosition = positionId,
                    DepartmentId = departmentId
                };
            }
            else  // Para cualquier otro caso, devuelve un empleado por defecto
            {
                return new Employee
                {
                    Id = id == 0 ? 0 : id,
                    Name = name,
                    Salary = salary,
                    CurrentPosition = positionId == 0 ? null : positionId, // O puedes dejar esta propiedad sin asignar
                    DepartmentId = departmentId
                };
            }
        }
    }
}
