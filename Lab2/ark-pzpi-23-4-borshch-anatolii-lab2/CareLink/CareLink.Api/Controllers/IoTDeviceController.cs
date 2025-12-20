using CareLink.Api.Models.Requests;
using CareLink.Application.Contracts.Repositories;
using CareLink.Application.Contracts.Security;
using CareLink.Application.Contracts.Services;
using CareLink.Application.Dtos.IoTDevices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareLink.Api.Controllers
{
    [ApiController]
    [Route("api/iot-devices")]
    [Authorize]
    public class IoTDeviceController : ControllerBase
    {
        private readonly IIoTDeviceService _iotDeviceService;
        private readonly IUserContext _userContext;


        public IoTDeviceController(IIoTDeviceService iotDeviceService, IUserContext userContext)
        {
            _iotDeviceService = iotDeviceService;
            _userContext = userContext;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllUsersIotDevicesAsync()
        {
            var userId = _userContext.GetApplicationUserId();
            
            var devices = await _iotDeviceService.GetAllUserDevicesAsync(userId);

            return Ok(devices);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateIotDeviceAsync(CreateIoTDeviceRequest request)
        {
            var userId = _userContext.GetApplicationUserId();

            var command = new IoTDeviceCreateRequest(userId, request.SerialNumber, request.DeviceTypeId);
            await _iotDeviceService.CreateDeviceAsync(command);
            
            return Ok();
        }

        [HttpPut("{deviceId:long}")]
        public async Task<IActionResult> UpdateIotDeviceAsync(long deviceId, UpdateIoTDeviceRequest request)
        {
            var userId = _userContext.GetApplicationUserId();

            var command = new IoTDeviceCreateRequest(userId, request.SerialNumber, request.DeviceTypeId);
            await _iotDeviceService.CreateDeviceAsync(command);
            
            return Ok();
        }
    }
}