using Serilog;

namespace Application.Extensions
{
    public static class LoggerExtension
    {
        public static IServiceCollection AddLoggingServices(this IServiceCollection services)
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
