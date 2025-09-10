using IdScanner.UI.Domain.Interfaces;
using IdScanner.UI.Infrastructure.Provider;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.UI.Infrastructure.Extension
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddInfrastrucureService(this IServiceCollection services)
        {
            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<IForgotPasswordRepository, ForgotPasswordRepository>();
            services.AddScoped<IMenuMasterRepository, MenuMasterRepository>();
            services.AddScoped<ICompanyMasterRepository, CompanyMasterRepository>();
            services.AddScoped<IDepartmentMasterRepository, DepartmentMasterRepository>();
            services.AddScoped<IUserDataRepository, UserDataRepository>();
            services.AddScoped<IGetUserNameByIdRepository, GetUserNameByIdRepository>();
            services.AddScoped<IResetPasswordRepossitory,ResetPasswordRepossitory>();
            return services;
        }
    }
}
