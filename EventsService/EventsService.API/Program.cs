using EventsService.Application.DependencyInjection;
using EventsService.Infrastructure.DependencyInjection;
using EventsService.Api.DependencyInjection;

namespace EventsService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.AddServiceDefaults();
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddApiServices(builder.Configuration);

            var app = builder.Build();

            app.MapDefaultEndpoints();

            app.UseCors("AllowSpecificOrigin");


            app.UseMiddleware<ExceptionHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();

            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(imagePath),
            //    RequestPath = "/images"
            //});

            app.Run();
        }
    }
}