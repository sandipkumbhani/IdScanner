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
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<ICompanyMasterRepository, CompanyMasterRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IUserDataRepository, UserDataRepository>();
            services.AddScoped<IGetLoginUserNameRepository, GetLoginUserNameRepository>();
            services.AddScoped<IForgotPasswordDbRepository,ForgotPasswordDbRepository>();
            
            return services;
        }
    }
}

