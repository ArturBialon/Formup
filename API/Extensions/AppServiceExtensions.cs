using Application.Extensions.ServiceCreator;
using Application.Services;
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
                options.UseSqlServer(
                    config.GetConnectionString("MssqlDbConnString"),
                    b => b.MigrationsAssembly("Infrastructure"));
            });

            AngularServiceCreator.ConfigureSwaggerAsync().Wait();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
