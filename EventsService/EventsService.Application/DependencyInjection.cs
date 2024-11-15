using Microsoft.Extensions.DependencyInjection;
using EventsService.Application.Validators;
using System.Reflection;
using FluentValidation;

namespace EventsService.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<CreateEventDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateProfileDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateEventDtoValidator>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            return services;

        }
    }
}
