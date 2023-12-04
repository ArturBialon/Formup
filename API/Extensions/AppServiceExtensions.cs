using Application.Extensions.ServiceCreator;
using Application.Services.Implementations;
using Application.Services.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
    public static class AppServiceExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<FormupContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("MssqlDbConnString"));
            });

            AngularServiceCreator.ConfigureSwaggerAsync().Wait();

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
