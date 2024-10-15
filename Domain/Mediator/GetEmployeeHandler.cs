using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Mediator
{
    /// <summary>
    /// Manejador para la solicitud de obtener un empleado.
    /// </summary>
    public class GetEmployeeHandler : IRequestHandler<GetEmployeeRequest, Employee>
    {
        private readonly IEmployeeRepository _repository;

        public GetEmployeeHandler(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<Employee> Handle(GetEmployeeRequest request)
        {
            return await _repository.GetByIdAsync(request.EmployeeId);
        }
    }
}
