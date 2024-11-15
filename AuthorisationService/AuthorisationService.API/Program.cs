using AuthorisationService.Application.DependencyInjection;
using AuthorisationService.Api.Middleware;
using AuthorisationService.Infrastructure.DependencyInjection;
using AuthorisationService.Api.DependencyInjection;


namespace EventsApp.AuthorisationService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddApiServices(builder.Configuration);
            builder.AddServiceDefaults();

            var app = builder.Build();
            app.UseCors("AllowSpecificOrigin");
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Authorization Service V1");
                });
            }

            app.MapControllers();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapDefaultEndpoints();
            app.Run();
        }
    }
}
