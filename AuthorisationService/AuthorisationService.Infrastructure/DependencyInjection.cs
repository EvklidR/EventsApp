using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AuthorisationService.Infrastructure.MSSQL;
using AuthorisationService.Domain.Interfaces;
using AuthorisationService.Infrastructure.Repositories;
using AuthorisationService.Infrastructure.Services;
using AuthorisationService.Application.Interfaces;

namespace AuthorisationService.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<ITokenService, TokenService>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
