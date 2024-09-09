using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetByEmail(string Email, CancellationToken cancellationToken);
        Task<User> GetById(Guid Id, CancellationToken cancellationToken);
    }
}
