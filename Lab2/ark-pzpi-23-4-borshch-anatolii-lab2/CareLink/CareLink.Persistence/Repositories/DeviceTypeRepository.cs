using CareLink.Application.Contracts.Repositories;
using CareLink.Domain.Entities.SubEntities;
using CareLink.Persistence.DbContext;

namespace CareLink.Persistence.Repositories
{
    public class DeviceTypeRepository : GenericRepository<DeviceType>, IDeviceTypeRepository
    {
        public DeviceTypeRepository(CareLinkDbContext context) : base(context)
        {
        }
    }
}