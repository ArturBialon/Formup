using Application;
using Application.Common.Behaviors;
using Application.Common.CurrencyServices;
using Application.Common.FileStorage;
using Application.Common.Jobs;
using FluentValidation;
using Hangfire;
using Infrastructure.Access;
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
            services.AddScoped<IWorkCaseCurrencyJobService, WorkCaseCurrencyJobService>();
            services.AddScoped<IFileStorageService, FileStorageService>();
            services.AddHangfire(hangfireConfig => hangfireConfig
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(config.GetConnectionString("MssqlDbConnString")));

            services.AddHangfireServer();

            return services;
        }
    }
}
