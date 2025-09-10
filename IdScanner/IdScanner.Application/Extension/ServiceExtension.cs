using IdScanner.Application.Interface;
using IdScanner.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Emertec.UI.Application.Extension
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IMenuMasterService, MenuMasterService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddScoped<ICompanyMasterService, CompanyMasterService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IUserDataService, UserDataService>();
            services.AddScoped<IGetLoginUserNameService, GetLoginUserNameService>();
            services.AddScoped<IForgotPasswordService, ForgotPasswordService>();
            services.AddScoped<IEmailService, EmailService>();
            return services;
        }
    }
}
