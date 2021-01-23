using AutoMapper;
using Chat.Core.Interfaces;
using Chat.Data.Context;
using Chat.Data.Data_Interfaces;
using Chat.Data.Data_Repositories;
using Chat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Core.Services
{
	public class AuthService:BaseRepository<User>,IAuthService
	{
		private readonly ChatContext _context;
		

		public AuthService( ChatContext context):base(context)
		{
			_context = context;
		}

        public async Task<IEnumerable<User>> GetAllUser(int id)
        {
            return await _context.Users.Where(x => x.Id != id).ToListAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }
        public async Task<User> Login(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
            return user;
        }

        public async Task<User> Register(User user)
        {
            await _context.AddAsync(user);
            return user;
        }

    }
}
