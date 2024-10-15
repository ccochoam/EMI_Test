using Domain.Entities;

namespace Domain.Interfaces
{
    /// <summary>
    /// interface para el CRUD de Users. Hereda de la clase User
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {

    }
}
