
using Chat.Core.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChatAPI.Healper
{
	public  class UserActivity: IAsyncActionFilter
	{
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            var userId = int.Parse(resultContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

			var repo = resultContext.HttpContext.RequestServices.GetService<IAuthService>();
			var unitOfWork = resultContext.HttpContext.RequestServices.GetService<IUnitOfWork>();

			var user = await repo.GetUserById(userId);
			user.LastActive = DateTime.Now;
			 unitOfWork.Complete();
		}
	}

}
