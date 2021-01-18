using ChatAPI.Data;
using ChatAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAPI.Services
{
    public class AuthRepository : IAuthRepository

    {
        private readonly ChatContext _context;
        public AuthRepository(ChatContext context)
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
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
