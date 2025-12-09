using CareLink.Application.Contracts.Repositories;
using CareLink.Domain.Entities;
using CareLink.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace CareLink.Persistence.Repositories
{
    public class IoTReadingRepository : GenericRepository<IoTReading>,IIoTReadingRepository
    {
        public IoTReadingRepository(CareLinkDbContext context) : base(context)
        {
        }
        
        public async Task<IEnumerable<IoTReading>> GetByDeviceIdAsync(long deviceId)
        {
            return await _context.IotReadings
                .AsNoTracking()
                .Where(r => r.DeviceId == deviceId)
                .OrderByDescending(r => r.ReadDateTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<IoTReading>> GetLatestReadingsAsync(long deviceId, int count)
        {
            return await _context.IotReadings
                .AsNoTracking()
                .Where(r => r.DeviceId == deviceId)
                .OrderByDescending(r => r.ReadDateTime)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<IoTReading>> GetReadingsInRangeAsync(long deviceId, DateTime from, DateTime to)
        {
            return await _context.IotReadings
                .AsNoTracking()
                .Where(r =>
                    r.DeviceId == deviceId &&
                    r.ReadDateTime >= from &&
                    r.ReadDateTime <= to)
                .OrderBy(r => r.ReadDateTime)
                .ToListAsync();
        }
    }
}