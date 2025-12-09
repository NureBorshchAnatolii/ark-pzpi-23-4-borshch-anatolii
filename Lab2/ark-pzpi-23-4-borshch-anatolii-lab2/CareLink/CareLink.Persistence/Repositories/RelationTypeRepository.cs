using CareLink.Application.Contracts.Repositories;
using CareLink.Domain.Entities.SubEntities;
using CareLink.Persistence.DbContext;

namespace CareLink.Persistence.Repositories
{
    public class RelationTypeRepository : GenericRepository<RelationType>, IRelationTypeRepository
    {
        public RelationTypeRepository(CareLinkDbContext context) : base(context)
        {
        }
    }
}