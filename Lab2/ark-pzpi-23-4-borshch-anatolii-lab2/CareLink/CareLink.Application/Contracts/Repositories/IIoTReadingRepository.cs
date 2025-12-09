using CareLink.Domain.Entities;

namespace CareLink.Application.Contracts.Repositories
{
    public interface IIoTReadingRepository : IGenericRepository<IoTReading>
    {
        Task<IEnumerable<IoTReading>> GetByDeviceIdAsync(long deviceId);
        Task<IEnumerable<IoTReading>> GetLatestReadingsAsync(long deviceId, int count);
        Task<IEnumerable<IoTReading>> GetReadingsInRangeAsync(long deviceId, DateTime from, DateTime to);
    }
}