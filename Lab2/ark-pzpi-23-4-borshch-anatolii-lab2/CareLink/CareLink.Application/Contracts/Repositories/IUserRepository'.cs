using CareLink.Domain.Entities;

namespace CareLink.Application.Contracts.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
    }
}