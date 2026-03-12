using System.Text;
using ChatApp.Application.Interfaces.Infrastructure;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Application.Services;
using ChatApp.Domain.Configurations;
using ChatApp.Domain.Interfaces;
using ChatApp.Infrastructure.Persistence;
using ChatApp.Infrastructure.Persistence.Repositories;
using ChatApp.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

namespace ChatApp.Api.Extensions;

public static class ServiceExtensions
{
    public static void ConfigSwagger(this IServiceCollection services) {
        services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(setup =>
            {
                setup.AddSecurityDefinition("Bearer",  new OpenApiSecurityScheme 
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
                });

                setup.AddSecurityRequirement(doc => new OpenApiSecurityRequirement 
                {
                    {
                        new OpenApiSecuritySchemeReference("Bearer"),
                        new List<string>()
                    },
                });
            });

    }

    public static void ConfigSqlServer(this IServiceCollection services, IConfiguration configuration) {
        services.AddDbContext<AppDbContext>(x => x.UseSqlServer(configuration.GetConnectionString("sqlConnection")));
    }

    public static void ConfigJwt(this IServiceCollection services) {

         var serviceProvider = services.BuildServiceProvider();
        JwtConfiguration jwtConfig = serviceProvider.GetRequiredService<IOptions<JwtConfiguration>>().Value;

        string key = Environment.GetEnvironmentVariable("JwtKey") ??
            throw new Exception("CAN'T GET JWT ENVIRONMET KEY");
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters() {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtConfig.ISSER,
                        ValidAudience = jwtConfig.AUDIENCE,

                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)) 
                    };
                });

    }

    public static void ConfigAppServices(this IServiceCollection services) {
        services.AddScoped<IAuthneticationService, AuthneticationService>();
        services.AddScoped<IUserServie, UserService>();
        services.AddScoped<IConversationService, ConversationService>();
    }

    public static void ConfigAppInfrastructure(this IServiceCollection services) {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IConversationRepository, ConversationRepository>();
    }
}
