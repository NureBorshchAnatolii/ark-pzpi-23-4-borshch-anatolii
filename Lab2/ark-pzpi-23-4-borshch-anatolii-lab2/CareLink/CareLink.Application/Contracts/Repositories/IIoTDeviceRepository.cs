using CareLink.Domain.Entities;

namespace CareLink.Application.Contracts.Repositories
{
    public interface IIoTDeviceRepository : IGenericRepository<IoTDevice>, IExistItemRepository<IoTDevice>
    {
        Task<IEnumerable<IoTDevice>> GetAllDevicesIncludedAsync();
        Task<IEnumerable<IoTDevice>> GetDevicesByUserIdAsync(long userId);
    }
}