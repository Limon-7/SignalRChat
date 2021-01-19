using System;
using System.Linq;
using System.Net;
using System.Text;
using AutoMapper;
using ChatAPI.Data;
using ChatAPI.Healper;
using ChatAPI.Models;
using ChatAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ChatAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<ChatContext>(x => x.UseSqlServer(Configuration.GetConnectionString("connection")));
            services.AddCors();
            services.AddSignalR();
            services.AddControllers().AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            }).ConfigureApiBehaviorOptions(opt =>
            {
                opt.InvalidModelStateResponseFactory = actionContext =>
                {
                    return CustomModelStateErrorHandler(actionContext);
                };
            });

            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AppSettings:secretKey"]))
                };
            });
            services.AddHealthChecks();

        }
        private BadRequestObjectResult CustomModelStateErrorHandler(ActionContext actionContext)
        {
            return new BadRequestObjectResult(actionContext.ModelState
                  .Where(modelError => modelError.Value.Errors.Count > 0)
                  .Select(modelError => new ErrorModel
                  {
                      ErrorKey = modelError.Key,
                      ErrorMessage = modelError.Value.Errors.FirstOrDefault().ErrorMessage
                  }).ToList());
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
