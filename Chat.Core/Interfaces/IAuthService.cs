using Chat.Core.Resourses;
using Chat.Data.Data_Interfaces;
using Chat.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Core.Interfaces
{
	public interface IAuthService:IBaseRepository<User>
	{
		Task<User> Register(User user);
		Task<User> Login(string userName);

		Task<IEnumerable<User>> GetAllUser(int id);
		Task<User> GetUserById(int id);
	}

}
