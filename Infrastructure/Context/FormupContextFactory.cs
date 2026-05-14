using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Context
{
    public class FormupContextFactory
        : IDesignTimeDbContextFactory<FormupContext>
    {
        public FormupContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            var connectionString =
                config.GetConnectionString("MssqlDbConnString");

            var optionsBuilder =
                new DbContextOptionsBuilder<FormupContext>();

            optionsBuilder.UseSqlServer(connectionString);

            return new FormupContext(optionsBuilder.Options);
        }
    }
}