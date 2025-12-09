using CareLink.Domain.Entities.SubEntities;

namespace CareLink.Application.Contracts.Repositories
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<long> IsRoleValid(string role);
        Task<long> IsRoleValid(long role);
    }
}