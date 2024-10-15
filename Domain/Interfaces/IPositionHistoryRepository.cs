using Domain.Entities;

namespace Domain.Interfaces
{
    /// <summary>
    /// interface para el CRUD de PositionHistories. Hereda de la clase PositionHistory
    /// </summary>
    public interface IPositionHistoryRepository: IRepository<PositionHistory>
    {
        Task<IEnumerable<PositionHistory>> GetListByEmployeeId(int employeeId);

    }
}
