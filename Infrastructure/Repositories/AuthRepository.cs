using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Repositorio para autenticar un usuario
    /// </summary>
    public class AuthRepository: IAuthRepository
    {
        private readonly EmployeeDbContext _context;

        public AuthRepository(EmployeeDbContext context)
        {
            _context = context;
        }

        public async Task<User> Authenticate(string username)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(e => e.Username == username);
        }
    }
}
