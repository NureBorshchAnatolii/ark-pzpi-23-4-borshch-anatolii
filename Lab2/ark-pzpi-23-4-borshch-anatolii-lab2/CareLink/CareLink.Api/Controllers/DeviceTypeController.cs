using CareLink.Application.Contracts.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareLink.Api.Controllers
{
    [ApiController]
    [Route("api/device-type")]
    [Authorize]
    public class DeviceTypeController : ControllerBase
    {
        private readonly IDeviceTypeRepository _deviceTypeRepository;

        public DeviceTypeController(IDeviceTypeRepository deviceTypeRepository)
        {
            _deviceTypeRepository = deviceTypeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDeviceTypesAsync()
        {
            var types = await _deviceTypeRepository.GetAllAsync();
            return Ok(types);
        }
    }
}