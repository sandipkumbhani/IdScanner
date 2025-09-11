using Microsoft.Extensions.DependencyInjection;
using SocPass.Domain.Interface;
using SocPass.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Infrastructure.Extension
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddEfcoreInfrastrucureService(this IServiceCollection services)
        {
            services.AddScoped<ILoginRepository,LoginRepository >();
            return services;
        }
    }
}
