using System.Text;
using ChatApp.Domain.Configurations;
using ChatApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ChatApp.Api.Extensions;

public static class ServiceExtensions
{
    public static void ConfigSwagger(this IServiceCollection services) {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
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
}
