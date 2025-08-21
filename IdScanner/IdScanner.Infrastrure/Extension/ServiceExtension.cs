using IdScanner.Domain.Interface;
using IdScanner.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.Infrastructure.Extension
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddEfcoreInfrastrucureService(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMenuMasterRepository, MenuMasterRepository>();
            return services;
        }
    }
}

