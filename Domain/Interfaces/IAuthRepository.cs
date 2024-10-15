using Domain.Entities;

namespace Domain.Interfaces
{
    /// <summary>
    /// Interface para la autenticación de los usuarios
    /// </summary>
    public interface IAuthRepository
    {
        Task<User> Authenticate(string username);
    }
}
