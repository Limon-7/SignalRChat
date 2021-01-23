using Autofac;
using Chat.Core.Interfaces;
using Chat.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyResolver
{
    public static class BusinessAutofacModule
    {
        public static ContainerBuilder CreateAutofacBusinessContainer(this IServiceCollection services, ContainerBuilder builder)
        {
            
            builder.RegisterType<IMessageService>().As<MessageService>();
            builder.RegisterType<IAuthService>().As<AuthService>();

            return builder;
        }
    }

    public class BusinessAutofacModule1 : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MessageService>().As<IMessageService>();
            builder.RegisterType<IAuthService>().As<IAuthService>();
        }
    }
}
