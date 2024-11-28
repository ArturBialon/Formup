using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Application.Extensions
{
    public static class LoggerExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("C:/FormupLogs/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

            services.AddSingleton(Log.Logger);

            return services;
        }
    }
}
