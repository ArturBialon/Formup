using Application.Extensions.ServiceCreator;
using Application.Repository.Implementations;
using Application.Services;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Services;
using Domain.Interfaces.UserAccessService;
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

            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<IClientDbRepository, ClientDbRepository>();
            services.AddTransient<IServiceProviderService, ServiceProviderService>();
            services.AddTransient<IServiceProviderDbRepository, ServiceProviderDbRepository>();
            services.AddTransient<IForwarderService, ForwarderService>();
            services.AddTransient<IForwarderDbRepository, ForwarderDbRepository>();
            services.AddTransient<ICaseService, CaseService>();
            services.AddTransient<ICaseDbRepository, CaseDbRepository>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
