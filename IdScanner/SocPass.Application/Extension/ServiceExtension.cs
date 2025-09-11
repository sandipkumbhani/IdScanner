using Microsoft.Extensions.DependencyInjection;
using SocPass.Application.Interface;
using SocPass.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Application.Extension
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddScoped<ILoginService, LoginService>();           
            return services;
        }
    }
}
