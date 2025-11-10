using Microsoft.Extensions.DependencyInjection;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Interfaces;
using SocPass.UI.Infrastructure.Provider;

namespace SocPass.UI.Infrastructure.Extension
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddInfrastrucureService(this IServiceCollection services)
        {
            services.AddScoped<ILoginAdapter, LoginAdapter>();
            services.AddScoped<IForgotPasswordAdapter, ForgotPasswordAdapter>();
            services.AddScoped<IGetUserNameByIdAdapter, GetUserNameByIdAdapter>();
            services.AddScoped<IResetPasswordAdapter,ResetPasswordAdapter>();
            services.AddScoped<IMenuMasterAdapter, MenuMasterAdapter>();
            services.AddScoped<IBlockAdapter, BlockAdapter>();
            services.AddScoped<ISocietyAdapter, SocietyAdapter>();
            services.AddScoped<IFlatAdapter, FlatAdapter>();
            services.AddScoped<IMemberAdapter, MemberAdapter>();
            services.AddScoped<IMemberDetailsAdapter, MemberDetailsAdapter>();
            services.AddScoped<ISubscriptionAdapter, SubscriptionAdapter>();
            services.AddScoped<IUserAdapter,UserAdapter>();   
            services.AddScoped<ISocietyDataAdapter, SocietyDataAdapter>();
            services.AddScoped<IUserFlatMappingAdapter, UserFlatMappingAdapter>();
            services.AddScoped<IEventAdapter, EventAdapter>();
            services.AddScoped<IReportAdapter, ReportAdapter>();
            services.AddScoped<ICommonAdapter, CommonAdapter>();
            return services;
        }
    }
}
