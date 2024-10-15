using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Repositorio CRUD para el histórico de posiciones
    /// </summary>
    public class PositionHistoryRepository: IPositionHistoryRepository
    {
        private readonly EmployeeDbContext _context;
        public PositionHistoryRepository(EmployeeDbContext context) {  _context = context; }

        public async Task<PositionHistory> GetByIdAsync(int id)
        {
            return await _context.PositionHistories.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<PositionHistory>> GetListByEmployeeId(int employeeId)
        {
            return await _context.PositionHistories.Where(e => e.EmployeeId == employeeId).ToListAsync();
        }

        public async Task<IEnumerable<PositionHistory>> GetAllAsync()
        {
            return await _context.PositionHistories.ToListAsync();
        }

        public async Task AddAsync(PositionHistory positionHistory)
        {
            await _context.PositionHistories.AddAsync(positionHistory);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PositionHistory positionHistory)
        {
            var existingPositionHistory = await _context.PositionHistories.FirstOrDefaultAsync(x => x.Id == positionHistory.Id);
            if (existingPositionHistory != null)
            {
                existingPositionHistory.EmployeeId = positionHistory.EmployeeId;
                existingPositionHistory.Position = positionHistory.Position;
                existingPositionHistory.StartDate = positionHistory.StartDate;
                existingPositionHistory.EndDate = positionHistory.EndDate;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int positionHistoryId)
        {
            var positionHistory = await _context.PositionHistories.FindAsync(positionHistoryId);
            if (positionHistory != null)
            {
                _context.PositionHistories.Remove(positionHistory);
                await _context.SaveChangesAsync();
            }
        }
    }
}
