using System.Text;
using DevIO.Api.Data;
using DevIO.Api.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DevIO.Api.Configuration
{
    public static class IdentityConfig
    {
        //O identity serve para criar e autentificar os usuarios
        public static IServiceCollection AddIdentityConfig(this IServiceCollection services,
            IConfiguration configuration)
        {
            //Criando context do banco do entity
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            //Adicionando o entity
            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>() //Poder trabalhar com roles 
                .AddEntityFrameworkStores<ApplicationDbContext>() //  
                .AddErrorDescriber<IdentityMensagensPortugues>()
                .AddDefaultTokenProviders();// Serve para gerar tokens para adicionar usuarios

            // JWT

            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = appSettings.ValidoEm,
                    ValidIssuer = appSettings.Emissor
                };
            });

            return services;
        }
    }
}