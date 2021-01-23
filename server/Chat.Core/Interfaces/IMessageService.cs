using Chat.Data.Data_Interfaces;
using Chat.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Core.Interfaces
{
	public interface IMessageService  :IBaseRepository<Message>
	{
        Task<User> GetUserById(int id);
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<Message> GetMessageById(int id);

        Task DeleteMessageOnlyMe(int id,int userId);
        Task<IEnumerable<Message>> DeleteConversation(int userId, int recipientId);

        Task<IEnumerable<Message>> GetMessageThreads(int userId, int recipientid);
        Task<IEnumerable<User>> GetAllUsers();
    }
}
