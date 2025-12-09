using CareLink.Api.Models.Requests;
using CareLink.Application.Contracts.Security;
using CareLink.Application.Contracts.Services;
using CareLink.Application.Dtos;
using CareLink.Application.Dtos.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareLink.Api.Controllers
{
    [ApiController]
    [Route("api/message")]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IUserContext _userContext;
        
        public MessageController(IMessageService messageService, IUserContext userContext)
        {
            _messageService = messageService;
            _userContext = userContext;
        }

        [HttpGet("user/{receiverId:long}")]
        public async Task<IActionResult> GetUsersMessagesAsync(long receiverId)
        {
            var userId = _userContext.GetApplicationUserId();

            var messages = await _messageService.GetUserMessagesAsync(userId, receiverId);
            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessageAsync([FromBody] CreateMessageRequest messageDto)
        {
            var senderId = _userContext.GetApplicationUserId();

            var request = new MessageCreateRequest(messageDto.Content, senderId, messageDto.ReceiverId);

            var message = await _messageService.CreateMessageAsync(request);
            return Ok(message);
        }

        [HttpPut("{messageId:long}")]
        public async Task<IActionResult> UpdateMessageAsync(long messageId, [FromBody] UpdateMessageRequest messageDto)
        {
            var userId = _userContext.GetApplicationUserId();

            var request = new MessageUpdateRequest(messageId, messageDto.NewContent);

            var updated = await _messageService.UpdateMessageAsync(request);
            return Ok(updated);
        }

        [HttpDelete("{messageId:long}")]
        public async Task<IActionResult> DeleteMessageAsync(long messageId)
        {
            var userId = _userContext.GetApplicationUserId();

            var command = new MessageDeleteRequest(messageId, userId);

            await _messageService.DeleteMessageAsync(command);

            return Ok("Message successfully deleted");
        }
    }
}