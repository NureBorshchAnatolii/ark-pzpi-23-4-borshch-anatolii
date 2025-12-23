using CareLink.Application.Dtos.Admin;

namespace CareLink.Application.Contracts.Services
{
    public interface IAdminService
    {
        Task<SystemStateDto> GetSystemStateAsync();

        Task ChangeUserRoleAsync(long userId, long roleId);

        Task DeleteUserAsync(long userId);

        Task<IEnumerable<SystemLogDto>> GetLogsAsync(
            DateTime? from,
            DateTime? to,
            string? level);
    }
}