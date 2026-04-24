using Gumas.Domain.Interfaces;
using Gumas.Infrastructure.Data;
using Gumas.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gumas.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database configuration
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        // Check if using SQLite (Data Source=*.db pattern)
        if (string.IsNullOrEmpty(connectionString) ||
            connectionString.Contains(".db", StringComparison.OrdinalIgnoreCase) ||
            connectionString.StartsWith("Data Source=", StringComparison.OrdinalIgnoreCase) && !connectionString.Contains("Server"))
        {
            // Use SQLite for development
            var sqliteConnection = string.IsNullOrEmpty(connectionString) ? "Data Source=gumas.db" : connectionString;
            
            // Ensure the directory for the SQLite database file exists
            var match = System.Text.RegularExpressions.Regex.Match(sqliteConnection, @"Data Source=(.+\.db)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (match.Success)
            {
                var dbPath = match.Groups[1].Value;
                var dbDir = Path.GetDirectoryName(dbPath);
                if (!string.IsNullOrEmpty(dbDir))
                {
                    Directory.CreateDirectory(dbDir);
                }
            }
            
            services.AddDbContext<GumasDbContext>(options =>
                options.UseSqlite(sqliteConnection));
        }
        else
        {
            // Use SQL Server for production
            services.AddDbContext<GumasDbContext>(options =>
                options.UseSqlServer(connectionString,
                    b => b.MigrationsAssembly(typeof(GumasDbContext).Assembly.FullName)));
        }

        // Repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
