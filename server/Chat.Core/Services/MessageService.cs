using AutoMapper;
using Chat.Core.Interfaces;
using Chat.Data.Context;
using Chat.Data.Data_Repositories;
using Chat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Core.Services
{
	public class MessageService : BaseRepository<Message>,IMessageService
	{
		private readonly ChatContext _context;
        public MessageService(ChatContext context):base(context)
        {
            _context = context;
        }

        public void AddMessage(Message message)
        {
            _context.Add(message);
        }

        #region Delete Conversation
        public async Task<IEnumerable<Message>> DeleteConversation(int userId, int recipientId)
        {
            var messages = await _context.Messages
               .Include(u => u.Sender)
               .Include(u => u.Recipient)
               .Where(m => m.RecipientId == userId && m.RecipientDeleted == false
                   && m.SenderId == recipientId
                   || m.RecipientId == recipientId && m.SenderId == userId
                   && m.SenderDeleted == false)
               .ToListAsync();
            RemoveRange(messages);
            return null;
        }
        #endregion


        /// <summary>
        /// Delete Message
        /// </summary>
        /// <param name="message"></param>

        #region Delete Message
        public void DeleteMessage(Message message)
        {
            Remove(message);
        }
        #endregion  Delete Message


        /// <summary>
        /// Delete message for only me
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>

        #region DeleteMessageOnlyMe
        public async Task DeleteMessageOnlyMe(int id, int userId)
        {
            var messageFromRepo = await GetMessageById(id);

            if (messageFromRepo.SenderId == userId)
                messageFromRepo.SenderDeleted = true;

            if (messageFromRepo.RecipientId == userId)
                messageFromRepo.RecipientDeleted = true;

            if (messageFromRepo.SenderDeleted && messageFromRepo.RecipientDeleted)
                DeleteMessage(messageFromRepo);
        }
        #endregion



        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        public async Task<Message> GetMessageById(int id)
        {
            return await _context.Messages.FirstOrDefaultAsync(x => x.Id == id);
        }


        #region Get Message Thread
        public async Task<IEnumerable<Message>> GetMessageThreads(int userId, int recipientId)
        {
            var messages = await _context.Messages
               .Include(u => u.Sender)
               .Include(u => u.Recipient)
               .Where(m => m.RecipientId == userId && m.RecipientDeleted == false
                   && m.SenderId == recipientId
                   || m.RecipientId == recipientId && m.SenderId == userId
                   && m.SenderDeleted == false)
               .ToListAsync();

            return messages;
        }
        #endregion


        public async Task<User> GetUserById(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }
    }
}
