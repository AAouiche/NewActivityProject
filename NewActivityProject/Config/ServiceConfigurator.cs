using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace NewActivityProject.Config
{
    public static class ServiceConfigurator
    {
        public static void AddJwtAuthentication(this WebApplicationBuilder builder)
        {
            // Get the key from configuration
            var jwtKey = builder.Configuration["JwtConfig:Key"];

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        // You can add other methods to configure other services as needed
    }
}