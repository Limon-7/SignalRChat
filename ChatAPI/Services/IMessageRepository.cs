using ChatAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAPI.Services
{
    public interface IMessageRepository
    {
        Task<User> GetUserById(int id);
        void Add(Message message);
        void DeleteMessage(Message message);
        Task<bool> SaveAll();
        Task<Message> GetMessageById(int id);
        Task<Message> GetMessageForUser();
        Task<IEnumerable<Message>> GetMessageThreads(int userId, int recipientid);
        Task<IEnumerable<User>> GetAllUsers();

    }
}
