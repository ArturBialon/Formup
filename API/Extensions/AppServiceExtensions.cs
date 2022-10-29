using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using API.Services.Interfaces;
using API.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Extensions
{
    public static class AppServiceExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<FWD_CompContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("MssqlDbConnString"));
            });

            //database conn logic
            services.AddScoped<IClientDbRepository, ClientDbRepository>();
            services.AddScoped<IServiceProviderDbRepository, ServiceProviderDbRepository>();
            services.AddScoped<IForwardersDbRepository, ForwardersDbRepository>();
            services.AddScoped<ICaseDbRepository, CaseDbRepository>();
            services.AddScoped<ILogin, Login>();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
