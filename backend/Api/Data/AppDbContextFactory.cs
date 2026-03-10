using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Api.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");

            if (File.Exists(envPath))
            {
                Env.Load(envPath);
            }

            var dbHost = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost";
            var dbPort = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5434";
            var dbName =
                Environment.GetEnvironmentVariable("POSTGRES_DB")
                ?? throw new Exception("POSTGRES_DB is missing.");
            var dbUser =
                Environment.GetEnvironmentVariable("POSTGRES_USER")
                ?? throw new Exception("POSTGRES_USER is missing.");
            var dbPassword =
                Environment.GetEnvironmentVariable("POSTGRES_PASSWORD")
                ?? throw new Exception("POSTGRES_PASSWORD is missing.");

            var connectionString =
                $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword}";

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
