using ChatAPI.Healper;
using ChatAPI.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ChatAPI.Configuration
{
	public static class AppConfigurations
	{
		public static void AddDefaultConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
		{
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Social Chat v1"));
            }
            else
            {
                app.UseExceptionHandler(build =>
                {
                    build.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                        var error = exceptionHandlerPathFeature.Error;

                        if (error != null && context.Response.StatusCode != 400)
                        {
                            var result = JsonConvert.SerializeObject(new { error = error.Message });
                            context.Response.AppApplicationError(error.Message);
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsync(result);
                        }
                    });
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(x => x
                 .SetIsOriginAllowed(origin => true)
                 .AllowAnyMethod()
                 .AllowAnyHeader()
                 .AllowCredentials());
            // can use the convenience extension method GetAutofacRoot.
            // this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();
            app.UseAuthentication();

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<BroadcastHub>("/notify");
                endpoints.MapControllers();
            });
        }
	}
}
