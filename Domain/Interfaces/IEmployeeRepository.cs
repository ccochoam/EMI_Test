using Domain.Entities;

namespace Domain.Interfaces
{
    /// <summary>
    /// Interface para el CRUD de Employees. Hereda de la clase Employee
    /// </summary>
    public interface IEmployeeRepository : IRepository<Employee>
    {

    }
}
