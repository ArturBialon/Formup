using Application;
using Application.Common.Behaviors;
using Application.Common.CurrencyServices;
using Application.Services;
using Domain.Interfaces.UserService;
using FluentValidation;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
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
                cfg.RegisterServicesFromAssembly(typeof(IApplicationMarker).Assembly);
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });
            services.AddValidatorsFromAssembly(typeof(IApplicationMarker).Assembly);
            services.AddScoped<ITokenService, TokenService>();
            services.AddHttpClient<ICurrencyConverterService, NbpCurrencyConverterService>();

            return services;
        }
    }
}
