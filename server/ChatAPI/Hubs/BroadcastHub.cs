
using Chat.Core.Resourses;
using ChatAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAPI.Hubs
{
    // [Authorize]
    public class BroadcastHub : Hub
    {


        public Task SendMessageToAll(string message)
        {
            return Clients.All.SendAsync("ReceiveMessage", message);
        }
        public Task SendMessageToCaller(MessageForCreationDto message)
        {
            return Clients.Caller.SendAsync("receivedMessage", message);
        }

        public Task SendMessageToUser(string connectionId, MessageForCreationDto message)
        {
            return Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
        }

        public async override Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
            await base.OnConnectedAsync();
        }
        public async override Task OnDisconnectedAsync(Exception ex)
        {
            await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
            await base.OnDisconnectedAsync(ex);
        }


        // private readonly IUserConnection _userConnection;

        // public BroadcastHub(IUserConnection userConnection)
        // {
        //     _userConnection = userConnection;
        // }

        // public string GetConnectionId()
        // {
        //     var httpContext = this.Context.GetHttpContext();
        //     var userId = httpContext.Request.Query["userId"];
        //     _userConnection.KeepUserConnection(int.Parse(userId), Context.ConnectionId);

        //     return Context.ConnectionId;
        // }
        // public async override Task OnDisconnectedAsync(Exception exception)
        // {
        //     //get the connectionId
        //     var connectionId = Context.ConnectionId;
        //     _userConnection.RemoveUserConnection(connectionId);
        //     var value = await Task.FromResult(0);
        // }
        // public async Task SendMessageToUser(MessageForReturnDto message)
        // {
        //     var senderId = message.SenderId.ToString();
        //     var recipientId = message.RecipientId.ToString();
        //     await Clients.Users(senderId, recipientId).SendAsync("broadCast", message);
        // }

    }
}
