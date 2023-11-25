using Infrastructure.Context;
using Infrastructure.Services.Implementations;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class AppServiceExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<FormupContext>(options =>
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
