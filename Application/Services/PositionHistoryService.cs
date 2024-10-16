using Domain.Entities;
using Domain.Factories;
using Domain.Interfaces;

namespace Application.Services
{
    /// <summary>
    /// CRUD para la clase PositionHistory
    /// </summary>
    public class PositionHistoryService
    {
        private readonly IPositionHistoryRepository _positionHistoryRepository;
        public PositionHistoryService(IPositionHistoryRepository positionHistoryRepository)
        {
            _positionHistoryRepository = positionHistoryRepository;
        }
        
        public async Task<IEnumerable<PositionHistory>> GetAllPositionHistoryAsync()
        {
            return await _positionHistoryRepository.GetAllAsync();
        }
        
        public async Task<PositionHistory> GetById(int employeeId)
        {
            return await _positionHistoryRepository.GetByIdAsync(employeeId);
        }
        
        public async Task<IEnumerable<PositionHistory>> GetListByEmployeeId(int employeeId)
        {
            return await _positionHistoryRepository.GetListByEmployeeId(employeeId);
        }

        public async Task<PositionHistory> CreatePositionHistoryAsync(int employeeId, string position, string startDate, string endDate, int employeeType, int? departamentId)
        {
            // Uso de Factory Pattern para crear una PositionHistory, pensado para implementar varios tipos de gerente con diferentes porcentajes para el cálculo del bono
            var positionHistory = PositionHistoryFactory.CreatePositionHistory(0, employeeId, position, startDate, endDate, employeeType, departamentId);

            await _positionHistoryRepository.AddAsync(positionHistory);

            return positionHistory;
        }

        public async Task<PositionHistory> UpdateEmployeeAsync(int id, int employeeId, string position, string startDate, string endDate, int employeeType, int? deparmentId)
        {
            // Uso de Factory Pattern para crear una PositionHistory, pensado para implementar varios tipos de gerente con diferentes porcentajes para el cálculo del bono
            var positionHistory = PositionHistoryFactory.CreatePositionHistory(id, employeeId, position, startDate, endDate, employeeType, deparmentId);

            // Actualiza un histórico en la base de datos
            await _positionHistoryRepository.UpdateAsync(positionHistory);

            return positionHistory;
        }

        public async Task<bool> DeletePositionHistoryAsync(int id)
        {
            var employee = await _positionHistoryRepository.GetByIdAsync(id);
            if (employee == null)
                return false;

            await _positionHistoryRepository.DeleteAsync(id);
            return true;
        }
    }
}
