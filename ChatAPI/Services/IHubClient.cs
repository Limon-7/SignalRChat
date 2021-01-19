using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatAPI.Models;
using ChatAPI.Resourses;

namespace ChatAPI.Services
{
    public interface IHubClient
    {
        Task RecieveMessageAsync(object message);
    }
}
