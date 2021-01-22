using ChatAPI.Models;
using ChatAPI.Resourses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAPI.Services
{
    // [Authorize]
    public class BroadcastHub : Hub
    {

        public async Task SendMessageToUser(MessageForReturnDto message)
        {
            var senderId = message.SenderId.ToString();
            var recipientId = message.RecipientId.ToString();
            await Clients.Users(senderId, recipientId).SendAsync("broadCast", message);
        }

    }
}
