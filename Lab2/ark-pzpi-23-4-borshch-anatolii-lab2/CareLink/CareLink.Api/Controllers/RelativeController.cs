using CareLink.Api.Models.Requests;
using CareLink.Application.Contracts.Security;
using CareLink.Application.Contracts.Services;
using CareLink.Application.Dtos.Relatives;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareLink.Api.Controllers
{
    [ApiController]
    [Route("api/relative")]
    [Authorize]
    public class RelativeController : ControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IRelativeService _relativeService;

        public RelativeController(IUserContext userContext, IRelativeService relativeService)
        {
            _userContext = userContext;
            _relativeService = relativeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRelatives()
        {
            var userId = _userContext.GetApplicationUserId();
            
            var relative = await _relativeService.GetAllRelativesAsync(userId);
            return Ok(relative);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRelativeAsync([FromBody] CreateRelativeRequest request)
        {
            var userId = _userContext.GetApplicationUserId();

            var command = new RelativeCreateCommand(userId, request.RelativeId, request.RelationTypeId);
            await _relativeService.CreateRelativeAsync(command);
            
            return Ok();
        }

        [HttpDelete("{relativeId:long}")]
        public async Task<IActionResult> DeleteRelative(long relativeId)
        {
            var userId = _userContext.GetApplicationUserId();
            
            var command = new RelativeDeleteCommand(userId, relativeId);
            await _relativeService.DeleteRelativeAsync(command);
            
            return Ok();
        }
    }
}