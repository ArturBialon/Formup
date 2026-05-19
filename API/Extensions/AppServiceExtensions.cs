using Application;
using Application.Common;
using Application.Services;
using Domain.Interfaces.UserAccessService;
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
            services.AddValidatorsFromAssembly(typeof(CreateWorkCaseCommandValidator).Assembly);
            services.AddValidatorsFromAssembly(typeof(UpdateWorkCaseCommandValidator).Assembly);
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
