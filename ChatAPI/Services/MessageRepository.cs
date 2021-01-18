using ChatAPI.Data;
using ChatAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAPI.Services
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ChatContext _context;
        public MessageRepository(ChatContext context)
        {
            _context = context;
        }

        public void Add(Message message)
        {
            _context.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Remove(message);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        public async Task<Message> GetMessageById(int id)
        {
            return await _context.Messages.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<Message> GetMessageForUser()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Message>> GetMessageThreads(int userId, int recipientId)
        {

            var messages = await _context.Messages
               .Include(u => u.Sender)
               .Include(u => u.Recipient)
               .Where(m => m.RecipientId == userId && m.RecipientDeleted == false
                   && m.SenderId == recipientId
                   || m.RecipientId == recipientId && m.SenderId == userId
                   && m.SenderDeleted == false)
               //.(m => m.MessageSent)
               .ToListAsync();

            return messages;

        }

        public async Task<User> GetUserById(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
