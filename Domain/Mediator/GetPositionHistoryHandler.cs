using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Mediator
{
    /// <summary>
    /// Manejador para la solicitud de obtener el historial de posiciones.
    /// </summary>
    public class GetPositionHistoryHandler : IRequestHandler<GetPositionHistoryRequest, PositionHistory>
    {
        private readonly IPositionHistoryRepository _repository;
        public GetPositionHistoryHandler(IPositionHistoryRepository repository)
        {
            _repository = repository;
        }
        public async Task<PositionHistory> Handle(GetPositionHistoryRequest request)
        {
            return await _repository.GetByIdAsync(request.PositionHistoryId);
        }
    }
}
