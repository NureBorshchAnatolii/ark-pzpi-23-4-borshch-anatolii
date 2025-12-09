using CareLink.Application.Dtos.IoTDevices;

namespace CareLink.Application.Contracts.Services
{
    public interface IIoTReadingService
    {
        Task<IoTReadingDto> CreateReadingAsync(IoTReadingCreateRequest request);
        Task<IEnumerable<IoTReadingDto>> GetAllDeviceReadingsAsync(long deviceId, long userId);
        Task<IEnumerable<IoTReadingDto>> GetLatestReadingsAsync(long deviceId, int count, long userId);
        Task<IEnumerable<IoTReadingDto>> GetReadingsRangeAsync(long deviceId, DateTime from, DateTime to, long userId);
    }
}