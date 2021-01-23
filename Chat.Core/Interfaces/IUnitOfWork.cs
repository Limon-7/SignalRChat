using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.Core.Interfaces
{
	public interface IUnitOfWork:IDisposable
	{

        IAuthService Users { get; }

        IMessageService Messages { get; }

        /// <summary>
        /// Saves the changes for each registered context.
        /// </summary>
        int Complete();

    }
}
                                                            