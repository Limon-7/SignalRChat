using Chat.Data.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAPI.Healper
{
	public static class ExceptionMiddlewareExtensions
	{
		public static void AppApplicationError(this HttpResponse response, string message)
		{
			response.Headers.Add("Application-Error", message);
			response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
			response.Headers.Add("Access-Control-Allow-Origin", "*");
			response.Headers.Add("Access-Control-Allow-Credentials", "true");

		}
		public static BadRequestObjectResult CustomModelStateErrorHandler(this ActionContext actionContext)
		{
			return new BadRequestObjectResult(actionContext.ModelState
				  .Where(modelError => modelError.Value.Errors.Count > 0)
				  .Select(modelError => new ErrorModel
				  {
					  ErrorKey = modelError.Key,
					  ErrorMessage = modelError.Value.Errors.FirstOrDefault().ErrorMessage
				  }).ToList());
		}
	}
}
