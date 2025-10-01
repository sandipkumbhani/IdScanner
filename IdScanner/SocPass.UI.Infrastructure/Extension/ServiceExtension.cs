using Microsoft.Extensions.DependencyInjection;
using SocPass.UI.Domain.Interfaces;
using SocPass.UI.Infrastructure.Provider;

namespace SocPass.UI.Infrastructure.Extension
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddInfrastrucureService(this IServiceCollection services)
        {
            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<IForgotPasswordRepository, ForgotPasswordRepository>();
            services.AddScoped<IGetUserNameByIdRepository, GetUserNameByIdRepository>();
            services.AddScoped<IResetPasswordRepossitory,ResetPasswordRepossitory>();
            services.AddScoped<IMenuMasterRepository, MenuMasterRepository>();
            services.AddScoped<IBlockRepository, BlockRepository>();
            services.AddScoped<ISocietyRepository, SocietyRepository>();
            services.AddScoped<IFlatRepository, FlatRepository>();
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IMemberDetailsRepository, MemberDetailsRepository>();
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            services.AddScoped<IUserRepository,UserRepository>();   
            return services;
        }
    }
}
