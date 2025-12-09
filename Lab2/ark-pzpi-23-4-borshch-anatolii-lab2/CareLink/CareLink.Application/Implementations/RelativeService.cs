using CareLink.Application.Contracts.Repositories;
using CareLink.Application.Contracts.Services;
using CareLink.Application.Dtos.Relatives;
using CareLink.Domain.Entities;

namespace CareLink.Application.Implementations
{
    public class RelativeService : IRelativeService
    {
        private readonly IRelativeRepository _relativeRepository;
        private readonly IUserRepository _userRepository;


        public RelativeService(IRelativeRepository relativeRepository, IUserRepository userRepository)
        {
            _relativeRepository = relativeRepository;
            _userRepository = userRepository;
        }
        
        public async Task<IEnumerable<RelativeDto>> GetAllRelativesAsync(long guardianUserId)
        {
            var relatives = await _relativeRepository.GetAllIncludedRelatives();
            var guardiansRelatives = relatives.Where(r => r.GuardianUserId == guardianUserId);
            var mappedRelatives = guardiansRelatives.Select(MapToRelativeDto);
            
            return mappedRelatives;
        }

        public async Task CreateRelativeAsync(RelativeCreateCommand request)
        {
            await ValidateUsersForCreate(request);

            await EnsureRelativeNotAssigned(request);

            var entity = new Relatives
            {
                GuardianUserId = request.GuardianUserId,
                RelativeUserId = request.RelativeUserId,
                RelationTypeId = request.RelationTypeId,
                AddedAt = DateTime.UtcNow
            };

            await _relativeRepository.AddAsync(entity);
        }

        public async Task DeleteRelativeAsync(RelativeDeleteCommand request)
        {
            var relatives = await _relativeRepository.GetAllIncludedRelatives();
            var relative = relatives.FirstOrDefault(x => x.Id == request.RelativeId)
                           ?? throw new ArgumentException("Relative user not found");;

            EnsureGuardianOwnership(request.GuardianUserId, relative);

            await _relativeRepository.DeleteAsync(relative);
        }

        private async Task ValidateUsersForCreate(RelativeCreateCommand request)
        {
            var relativeUser = await _userRepository.GetByIdAsync(request.RelativeUserId)
                               ?? throw new ArgumentException("Relative user not found");

            _ = await _userRepository.GetByIdAsync(request.GuardianUserId)
                ?? throw new ArgumentException("Guardian user not found");

            if (relativeUser.RoleId != 5)
                throw new InvalidOperationException("User does not have the 'Relative' role");
        }

        private async Task EnsureRelativeNotAssigned(RelativeCreateCommand request)
        {
            var existing = await _relativeRepository.ExistItemAsync(r =>
                r.GuardianUserId == request.GuardianUserId &&
                r.RelativeUserId == request.RelativeUserId);

            if (!existing)
                throw new InvalidOperationException("This relative is already assigned to the guardian");
        }

        private void EnsureGuardianOwnership(long guardianId, Relatives relative)
        {
            if (relative.GuardianUserId != guardianId)
                throw new UnauthorizedAccessException("User is not the guardian of this relative");
        }

        private RelativeDto MapToRelativeDto(Relatives relative)
        {
            return new RelativeDto()
            {
                Id = relative.Id,
                RelationType = relative.RelationType.Name,
                GuardianId = relative.GuardianUserId,
                GuardianFullName = relative.GuardianUser.FirstName + " " + relative.GuardianUser.LastName,
                AddedAt = relative.AddedAt,
                RelativeId = relative.RelativeUserId,
                RelativeFullName = relative.RelativeUser.FirstName + " " + relative.RelativeUser.LastName
            };
        }
    }
}