using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatAPI.Data;
using ChatAPI.Models;
using ChatAPI.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ChatAPI.Resourses;
using Microsoft.AspNetCore.SignalR;

namespace ChatAPI.Controllers
{
    [Authorize]
    [Route("api/user/{userId}/[controller]")]
    [ApiController]

    public class MessagesController : ControllerBase
    {
        private readonly IMessageRepository _repo;
        private readonly IMapper _mapper;
        private readonly IHubContext<BroadcastHub> _hubContext;

        public MessagesController(IMessageRepository repo, IMapper mapper, IHubContext<BroadcastHub> hubContext)
        {
            _repo = repo;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        // GET: api/user/1/Messages/1
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
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messageFromRepo = await _repo.GetMessageThreads(userId, recipientId);
            var messages = _mapper.Map<IEnumerable<MessageForReturnDto>>(messageFromRepo);
            // await _hubContext.Clients.All.SendAsync("receivedMessag", messages);
            return Ok(messages);
        }


        [HttpPost]
        public async Task<ActionResult> CreateMessage(int userId, MessageForCreationDto messageForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            messageForCreationDto.SenderId = userId;
            var recipinet = await _repo.GetUserById(messageForCreationDto.RecipientId);
            if (recipinet == null)
                return BadRequest("Receiver not found");

            var message = _mapper.Map<Message>(messageForCreationDto);
            _repo.Add(message);
            var messageToReturn = _mapper.Map<MessageForCreationDto>(message);
            if (await _repo.SaveAll())
            {
                // todo
                // await _hubContext.Clients.All.SendAsync("receivedMessage", messageToReturn);

                // await _hubContext.Clients.User(message.SenderId.ToString()).RecieveMessageAsync(messageToReturn);
                // await _hubContext.Clients.User(message.RecipientId.ToString()).RecieveMessageAsync(messageToReturn);

                //await _hubContext.Clients.Users(messageToReturn.SenderId.ToString(), messageToReturn.RecipientId.ToString()).SendAsync("receivedMessage", message);
                await _hubContext.Clients.All.SendAsync("receivedMessage", messageToReturn);

                // await _hubContext.Clients.Client(userId.ToString()).SendAsync("receivedMessage", messageToReturn);
                return CreatedAtAction("GetMessage", new { userId, id = message.Id }, messageToReturn);
            }

            throw new Exception($"Message could not save");

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messageFromRepo = await _repo.GetMessageById(id);

            if (messageFromRepo.SenderId == userId)
                messageFromRepo.SenderDeleted = true;

            if (messageFromRepo.RecipientId == userId)
                messageFromRepo.RecipientDeleted = true;

            if (messageFromRepo.SenderDeleted && messageFromRepo.RecipientDeleted)
                _repo.DeleteMessage(messageFromRepo);

            if (await _repo.SaveAll())
                await _hubContext.Clients.All.SendAsync("messageDelete");
            return NoContent();

            throw new Exception("Error deleting the message");
        }
    }
}
