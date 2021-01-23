using AutoMapper;
using Chat.Core.Interfaces;
using Chat.Core.Services;
using Chat.Data.Context;
using Chat.Data.Entities;
using ChatAPI.Healper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAPI.Configuration
{
    public static class AppServices
    {
        public static void AddDefaultServices(this IServiceCollection services, IConfiguration Configuration)
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

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<UserActivity>();
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Social-Chat", Version = "v1" });
            });

            services.AddHealthChecks();
        }



        private static BadRequestObjectResult CustomModelStateErrorHandler(ActionContext actionContext)
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
