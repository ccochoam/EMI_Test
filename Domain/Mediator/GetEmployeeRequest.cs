using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mediator
{
    /// <summary>
    /// Solicitud para obtener un empleado por ID.
    /// </summary>
    public class GetEmployeeRequest
    {
        public int EmployeeId { get; }

        public GetEmployeeRequest(int employeeId)
        {
            EmployeeId = employeeId;
        }
    }
}
