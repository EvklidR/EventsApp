using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using EventsApp.AuthorisationService.Infrastructure.MSSQL;
using EventsApp.AuthorisationService.Application.Interfaces;
using EventsApp.AuthorisationService.Application.Services;
using EventsApp.AuthorisationService.Application.Mappings;
using EventsApp.AuthorisationService.Domain.Interfaces;
using EventsApp.AuthorisationService.Infrastructure.Repositories;
using EventsApp.AuthorisationService.Application.ApplicationServices;
using Microsoft.OpenApi.Models;
using FluentValidation;
using EventsApp.AuthorisationService.Application.Validators;
using EventsApp.EventsService;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace EventsApp.AuthorisationService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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

            builder.AddServiceDefaults();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>();


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

            builder.Services.AddTransient<ITokenService, TokenService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddAutoMapper(typeof(UserProfileMapper).Assembly);

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