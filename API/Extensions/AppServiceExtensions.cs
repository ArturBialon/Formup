using Application.Services;
using Domain.Interfaces.UserAccessService;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

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

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(AppServiceExtensions).Assembly);
            });

            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
