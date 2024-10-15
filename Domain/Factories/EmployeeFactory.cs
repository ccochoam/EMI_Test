using Domain.Entities;
using Domain.Enums;
using Domain.Strategies;

namespace Domain.Factories
{
    /// <summary>
    /// clase "fábrica" para encapsular la creación de objetos. Se pretende permitir la inversión de contro e inyección de dependencias al tener centralizado la creación del objeto según su posición (cargo).
    /// </summary>
    public class EmployeeFactory
    {
        public static Employee CreateEmployee(int id, string name, int positionId, decimal salary, int employeeType = 0)
        {
            // Lógica basada en el cargo del empleado
            if (employeeType == (int)EmployeeType.ManagerEmployee)  // ID de Gerente
            {
                return new ManagerEmployee
                {
                    Id = id,
                    Name = name,
                    Salary = salary,
                    CurrentPosition = positionId
                };
            }
            else if (employeeType == (int)EmployeeType.RegularEmployee)  // ID de Empleado regular
            {
                return new RegularEmployee
                {
                    Id = id,
                    Name = name,
                    Salary = salary,
                    CurrentPosition = positionId
                };
            }
            else  // Para cualquier otro caso, devuelve un empleado por defecto
            {
                return new Employee
                {
                    Name = name,
                    Salary = salary,
                    CurrentPosition = null  // O puedes dejar esta propiedad sin asignar
                };
            }
        }
    }
}
