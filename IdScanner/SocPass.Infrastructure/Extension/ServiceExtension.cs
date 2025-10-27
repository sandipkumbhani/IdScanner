using Microsoft.Extensions.DependencyInjection;
using SocPass.Domain.Interface;
using SocPass.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Infrastructure.Extension
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddInfrastrucureService(this IServiceCollection services)
        {
            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<ISocietyRepository, SocietyRepository>();
            services.AddScoped<IBlockRepository, BlockRepository>();
            services.AddScoped<IFlatRepository, FlatRepository>();
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IMenuMasterRepository, MenuMasterRepository>();
            services.AddScoped<IForgotPasswordDbRepository, ForgotPasswordDbRepository>();
            services.AddScoped<IGetLoginUserNameRepository, GetLoginUserNameRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();  
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            services.AddScoped<ISocietyDataRepository, SocietyDataRepository>();
            services.AddScoped<IUserFlatMappingRepository, UserFlatMappingRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            return services;
        }
    }
}
