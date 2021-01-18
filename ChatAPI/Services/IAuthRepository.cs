using ChatAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAPI.Services
{
    public interface IAuthRepository
    {
        Task<User> Register(User user);
        Task<User> Login(string userName);

        Task<IEnumerable<User>> GetAllUser(int id);
        Task<User> GetUserById(int id);
    }
}
