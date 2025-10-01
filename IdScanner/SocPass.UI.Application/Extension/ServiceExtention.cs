using Microsoft.Extensions.DependencyInjection;
using SocPass.UI.Application.Interface;
using SocPass.UI.Application.Services;

namespace SocPass.UI.Application.Extension
{
    public static class ServiceExtention
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddScoped<ILoginServices,LoginServices>();
            services.AddScoped<IForgotPasswordService, ForgotPasswordService>();
            services.AddScoped<IMenuMasterService, MenuMasterService>();
            services.AddScoped<IGetUserNameByIdService, GetUserNameByIdService>();
            services.AddScoped<IResetPasswordService, ResetPasswordService>();
            services.AddScoped<ISocietyService, SocietyService>();
            services.AddScoped<IFlatService, FlatService>();
            services.AddScoped<IBlockService, BlockService>();
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<IMemberDetailsService, MemberDetailsService>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            services.AddScoped<IUserService, UserService>();    
            services.AddScoped<ISocietyDataService, SocietyDataService>();
            return services;
        }
    }
}
