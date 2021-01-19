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
        // public async Task GetUsers(UserForReturnDto dto)
        // {
        //     await Clients.All.SendAsync("getUser", dto);
        // }
    }
}
