using CareLink.Application.Contracts.Repositories;
using CareLink.Application.Contracts.Services;
using CareLink.Application.Dtos.IoTDevices;
using CareLink.Domain.Entities;

namespace CareLink.Application.Implementations
{
    public class IoTReadingService : IIoTReadingService
    {
        private readonly IIoTReadingRepository _readingRepository;
        private readonly IIoTDeviceRepository _deviceRepository;

        public IoTReadingService(
            IIoTReadingRepository readingRepository,
            IIoTDeviceRepository deviceRepository)
        {
            _readingRepository = readingRepository;
            _deviceRepository = deviceRepository;
        }

        public async Task<IoTReadingDto> CreateReadingAsync(IoTReadingCreateRequest request)
        {
            await EnsureDeviceOwnedByUser(request.DeviceId, request.UserId);

            var reading = new IoTReading
            {
                ReadDateTime = request.ReadDateTime,
                Pulse = request.Pulse,
                ActivityLevel = request.ActivityLevel,
                Temperature = request.Temperature,
                DeviceId = request.DeviceId
            };

            await _readingRepository.AddAsync(reading);

            return MapToDto(reading);
        }

        public async Task<IEnumerable<IoTReadingDto>> GetAllDeviceReadingsAsync(long deviceId, long userId)
        {
            await EnsureDeviceOwnedByUser(deviceId, userId);

            var readings = await _readingRepository.GetByDeviceIdAsync(deviceId);

            return readings.Select(MapToDto);
        }

        public async Task<IEnumerable<IoTReadingDto>> GetLatestReadingsAsync(long deviceId, int count, long userId)
        {
            await EnsureDeviceOwnedByUser(deviceId, userId);

            var readings = await _readingRepository.GetLatestReadingsAsync(deviceId, count);

            return readings.Select(MapToDto);
        }

        public async Task<IEnumerable<IoTReadingDto>> GetReadingsRangeAsync(long deviceId, DateTime from, DateTime to,
            long userId)
        {
            await EnsureDeviceOwnedByUser(deviceId, userId);

            var readings = await _readingRepository.GetReadingsInRangeAsync(deviceId, from, to);

            return readings.Select(MapToDto);
        }

        private async Task EnsureDeviceOwnedByUser(long deviceId, long userId)
        {
            var device = await _deviceRepository.GetByIdAsync(deviceId);

            if (device == null)
                throw new Exception("Device not found.");

            if (device.UserId != userId)
                throw new UnauthorizedAccessException("User does not own this device.");
        }

        private IoTReadingDto MapToDto(IoTReading reading)
        {
            return new IoTReadingDto
            {
                Id = reading.Id,
                ReadDateTime = reading.ReadDateTime,
                Pulse = reading.Pulse,
                ActivityLevel = reading.ActivityLevel,
                Temperature = reading.Temperature
            };
        }
    }
}