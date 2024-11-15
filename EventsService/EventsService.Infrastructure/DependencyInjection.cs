using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EventsService.Infrastructure.MSSQL;
using EventsService.Infrastructure.Repositories;
using EventsService.Domain.Interfaces;
using EventsService.Infrastructure.Services;
using EventsService.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace EventsService.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IParticipantRepository, ParticipantRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IUserService, UserService>();


            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), configuration["ImageSettings:ImagePath"]);
            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }

            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("127.0.0.1:6379"));

            services.AddScoped<IImageService>(provider => new ImageService(imagePath, provider.GetRequiredService<IConnectionMultiplexer>()));

            return services;
        }
    }
}
