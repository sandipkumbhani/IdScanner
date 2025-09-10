using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdScanner.UI.Application.Interface;
using IdScanner.UI.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IdScanner.UI.Application.Extension
{
    public static class ServiceExtention
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddScoped<ILoginServices,LoginServices>();
            services.AddScoped<IForgotPasswordService, ForgotPasswordService>();
            services.AddScoped<IMenuMasterService, MenuMasterService>();
            services.AddScoped<ICompanyMasterService, CompanyMasterService>();
            services.AddScoped<IDepartmentMasterService, DepartmentMasterService>();
            services.AddScoped<IUserDataService, UserDataService>();
            services.AddScoped<IGetUserNameByIdService, GetUserNameByIdService>();
            services.AddScoped<IResetPasswordService, ResetPasswordService>();
            return services;
        }
    }
}
