using Chat.Core;
using Chat.Core.Interfaces;
using Chat.Core.Resourses;
using Chat.Core.Services;
using Chat.Data.Entities;
using ChatAPI.Healper;
using ChatAPI.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChatAPI.Controllers
{

    [Authorize]
    [Route("api/[controller]/{userId}")]
    [ApiController]
    [ServiceFilter(typeof(UserActivity))]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _repo;
        private readonly IHubContext<BroadcastHub> _hubContext;
        private readonly IUnitOfWork _unitOfWork;

        public MessagesController(IMessageService repo, IHubContext<BroadcastHub> hubContext, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _hubContext = hubContext;
            _unitOfWork = unitOfWork;
        }

        //GET: api/user/1/Messages/1
        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<ActionResult<Message>> GetMessage(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messageFromRepo = await _repo.GetMessageById(id);
            if (messageFromRepo == null)
                return NotFound();
            return Ok(messageFromRepo);
        }

        [HttpGet("thread/{recipientId}")]
        public async Task<ActionResult> GetMessageThread(int userId, int recipientId)
        {
            if (!CheckUserAuthenticate(userId))
                return Unauthorized();
            var messageFromRepo = await _repo.GetMessageThreads(userId, recipientId);
            var messages = Mapping.Mapper.Map<IEnumerable<MessageForReturnDto>>(messageFromRepo);
            return Ok(messages);
        }


        [HttpPost]
        public async Task<ActionResult> CreateMessage(int userId, MessageForCreationDto messageForCreationDto)
        {
            if (!CheckUserAuthenticate(userId))
                return Unauthorized();

            messageForCreationDto.SenderId = userId;
            var recipinet = await _repo.GetUserById(messageForCreationDto.RecipientId);
            if (recipinet == null)
                return BadRequest("Receiver not found");

            var message = Mapping.Mapper.Map<Message>(messageForCreationDto);
            _repo.Add(message);
            var messageToReturn = Mapping.Mapper.Map<MessageForCreationDto>(message);
            if (_unitOfWork.Complete() > 0)
            {
                await _hubContext.Clients.All.SendAsync("receivedMessage", messageToReturn);
                return CreatedAtAction("GetMessage", new { userId, id = message.Id }, messageToReturn);
            }

            throw new Exception($"Message could not save");

        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteMessageOnlyMe(int id, int userId)
        {
            if (!CheckUserAuthenticate(userId))
                return Unauthorized();

            await _repo.DeleteMessageOnlyMe(id, userId);
            if (_unitOfWork.Complete() > 0)
                await _hubContext.Clients.All.SendAsync("messageDelete");
            return NoContent();

            throw new Exception("Error deleting the message");
        }

        [HttpDelete("delete/both/{id}")]
        public async Task<IActionResult> DeleteMessageForAll(int id, int userId)
        {

            if (!CheckUserAuthenticate(userId))
                return Unauthorized();
            var messageFromRepo = await _repo.GetMessageById(id);
            _repo.Remove(messageFromRepo);

            if (_unitOfWork.Complete() > 0)
                await _hubContext.Clients.All.SendAsync("messageDelete");
            return NoContent();

            throw new Exception("Error deleting the message");
        }

        [HttpDelete("delete/all/{recipientId}")]
        public async Task<IActionResult> DeleteConversation(int userId, int recipientId)
        {
            if (!CheckUserAuthenticate(userId))
                return Unauthorized();

            await _repo.DeleteConversation(userId, recipientId);

            if (_unitOfWork.Complete() > 0)
                await _hubContext.Clients.All.SendAsync("messageDelete");
            return NoContent();

            throw new Exception("Error deleting the message");
        }


        private bool CheckUserAuthenticate(int userId)
        {
            if (userId == int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return true;
            return false;
        }

    }
}
