using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdScanner.UI.Application.Interface;
using IdScanner.UI.Application.Services;
using IdScanner.UI.Domain.Interfaces;
using IdScanner.UI.Infrastructure.Provider;
using Microsoft.Extensions.DependencyInjection;

namespace IdScanner.UI.Infrastructure.Extension
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddEfcoreInfrastrucureService(this 
            IServiceCollection services)
        {
            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<IForgotPasswordRepository, ForgotPasswordRepository>();
            services.AddScoped<IMenuMasterRepository, MenuMasterRepository>();
            services.AddScoped<ICompanyMasterRepository, CompanyMasterRepository>();
            services.AddScoped<IDepartmentMasterRepository, DepartmentMasterRepository>();

            return services;
        }
    }
}
