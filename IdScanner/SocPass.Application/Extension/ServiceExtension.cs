using Microsoft.Extensions.DependencyInjection;
using SocPass.Application.Interface;
using SocPass.Application.Services;
using SocPass.Domain.Interface;
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
            services.AddScoped<ISocietyService, SocietyService>();
            services.AddScoped<IBlockService, BlockService>();
            services.AddScoped<IFlatService, FlatService>();
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<IMenuMasterService, MenuMasterService>();
            services.AddScoped<IForgotPasswordService, ForgotPasswordService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IGetLoginUserNameService,GetLoginUserNameService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddScoped<ILoginService, LoginService>();  
            services.AddScoped< IUserService ,UserService>();   
            return services;
        }
    }
}
