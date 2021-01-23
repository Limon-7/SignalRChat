using Chat.Core.Interfaces;
using Chat.Data.Context;
using Chat.Data.Data_Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.Core.Services
{
	public class UnitOfWork : IUnitOfWork
	{
        private readonly ChatContext _context;



        public UnitOfWork(ChatContext context)
        {
            _context = context;
            Users = new AuthService(_context);
            Messages = new MessageService(_context);
        }

        public IAuthService Users { get; }

        public IMessageService Messages { get; }


        private bool disposed;


        public  int Complete()
        {
            return _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                _context.Dispose();
            }

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }

		
	}
}
