using CareLink.Api.Models.Requests;
using CareLink.Application.Contracts.Security;
using CareLink.Application.Contracts.Services;
using CareLink.Application.Dtos.IoTDevices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareLink.Api.Controllers
{
    [ApiController]
    [Route("api/iot-readings")]
    [Authorize]
    public class IoTDeviceReadingsController : ControllerBase
    {
        private readonly IIoTReadingService _readingService;
        private readonly IUserContext _userContext;

        public IoTDeviceReadingsController(
            IIoTReadingService readingService,
            IUserContext userContext)
        {
            _readingService = readingService;
            _userContext = userContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReadingAsync([FromBody] CreateIoTReadingRequest request)
        {
            var userId = _userContext.GetApplicationUserId();

            var command = new IoTReadingCreateRequest(request.ReadDateTime, request.Pulse, 
                request.ActivityLevel, request.Temperature, request.DeviceId, userId);
            var result = await _readingService.CreateReadingAsync(command);

            return Ok(result);
        }
        
        [HttpGet("{deviceId:long}")]
        public async Task<IActionResult> GetAllDeviceReadingsAsync(long deviceId)
        {
            var userId = _userContext.GetApplicationUserId();

            var result = await _readingService.GetAllDeviceReadingsAsync(deviceId, userId);

            return Ok(result);
        }

        [HttpGet("device/{deviceId:long}/latest")]
        public async Task<IActionResult> GetLatestReadingsAsync(long deviceId, [FromQuery] int count)
        {
            var userId = _userContext.GetApplicationUserId();
            
            var result = await _readingService.GetLatestReadingsAsync(deviceId, count, userId);

            return Ok(result);
        }

        [HttpGet("{deviceId:long}/range")]
        public async Task<IActionResult> GetReadingsInRangeAsync(
            long deviceId,
            [FromQuery] DateTime from,
            [FromQuery] DateTime to)
        {
            var userId = _userContext.GetApplicationUserId();

            var result = await _readingService.GetReadingsRangeAsync(deviceId, from, to, userId);

            return Ok(result);
        }
    }
}