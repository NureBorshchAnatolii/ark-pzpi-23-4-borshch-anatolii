using CareLink.Application.Contracts.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CareLink.Api.Controllers
{
    [ApiController]
    [Route("api/notiffication-type")]
    public class NotificationTypeController : ControllerBase
    {
        private readonly INotificationTypeRepository _notifficationTypeRepository;

        public NotificationTypeController(INotificationTypeRepository notifficationTypeRepository)
        {
            _notifficationTypeRepository = notifficationTypeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRolesAsync()
        {
            var roles = await _notifficationTypeRepository.GetAllAsync();
            return Ok(roles);
        }
    }
}