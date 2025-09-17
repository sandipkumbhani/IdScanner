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
            return services;
        }
    }
}
