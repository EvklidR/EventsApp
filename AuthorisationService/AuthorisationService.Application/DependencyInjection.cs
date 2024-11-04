using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using AuthorisationService.Application.UseCases;
using AuthorisationService.Application.Interfaces.UseCases;

namespace AuthorisationService.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddScoped<ILoginUser, LoginUser>();
            services.AddScoped<IRegisterUser, RegisterUser>();
            services.AddScoped<IRefreshToken, RefreshToken>();
            services.AddScoped<IRevokeToken, RevokeToken>();

            return services;
        }
    }
}
