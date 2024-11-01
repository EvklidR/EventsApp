using EventsService.Infrastructure.MSSQL;
using EventsService.Application.Interfaces;
using EventsService.Application.ApplicationServices;
using EventsService.Domain.Interfaces;
using EventsService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using StackExchange.Redis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using EventsService.Infrastructure.Services;
using EventsService.Application.Validators;
using FluentValidation;
using System.Text;



namespace EventsService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.AddServiceDefaults();

            var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins:CorsOrigins").Get<string[]>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder.WithOrigins(allowedOrigins)
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Authorization Service", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter your token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IEventService, EventService>();
            builder.Services.AddScoped<IParticipantService, ParticipantService>();
            builder.Services.AddTransient<IEmailSender, EmailSender>();


            builder.Services.AddValidatorsFromAssemblyContaining<CreateEventDtoValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<CreateProfileDtoValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<ParticipantOfEventDtoValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<UpdateEventDtoValidator>();

            var imagePath = Path.Combine(builder.Environment.ContentRootPath, builder.Configuration["ImageSettings:ImagePath"]);

            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }
            builder.Services.AddScoped<IUnitOfWork>(provider =>
            {
                return new UnitOfWork(provider.GetRequiredService<ApplicationDbContext>(), imagePath);
            });

            builder.Services.AddScoped<IEventRepository>(provider =>
            {
                return new EventRepository(provider.GetRequiredService<ApplicationDbContext>(), imagePath);
            });

            builder.Services.AddScoped<IParticipantRepository, ParticipantRepository>();

            builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("127.0.0.1:6379"));

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["AuthOptions:Issuer"],
                    ValidAudience = builder.Configuration["AuthOptions:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["AuthOptions:Key"]
                        ))
                };
            });

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

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(imagePath),
                RequestPath = "/images"
            });

            app.Run();
        }
    }
}