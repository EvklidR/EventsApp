using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using AutoMapper;
using AuthorisationService.Application.Interfaces;
using AuthorisationService.Application.UseCases;

namespace AuthorisationService.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddScoped<IAddUser, AddUser>();
            services.AddScoped<IGetUserById, GetUserById>();
            services.AddScoped<IFindUserByCredentials, FindUserByCredentials>();
            services.AddScoped<IUpdateUser, UpdateUser>();
            services.AddScoped<IUserServiceFacade, UserServiceFacade>();

            return services;
        }
    }
}
