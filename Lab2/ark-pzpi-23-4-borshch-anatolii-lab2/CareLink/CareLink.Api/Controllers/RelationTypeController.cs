using CareLink.Application.Contracts.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareLink.Api.Controllers
{
    [ApiController]
    [Route("api/relation-type")]
    [Authorize]
    public class RelationTypeController : ControllerBase
    {
        private readonly IRelationTypeRepository _relationTypeRepository;

        public RelationTypeController(IRelationTypeRepository relationTypeRepository)
        {
            _relationTypeRepository = relationTypeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRelationTypes()
        {
            var types = await _relationTypeRepository.GetAllAsync();
            return Ok(types);
        }
    }
}