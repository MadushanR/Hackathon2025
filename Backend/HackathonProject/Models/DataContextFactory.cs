using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;

namespace HackathonProject.Models
{
    /// <summary>
    /// DesignTimeDbContextFactory is required for 'dotnet ef' commands (like migrations add/update)
    /// to run successfully without relying on the application's service provider.
    /// This is configured to use the SQLite provider.
    /// </summary>
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            // 1. Load configuration (specifically appsettings.json)
            // We set the base path to the directory containing the main project file
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // 2. Get the connection string
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("DefaultConnection connection string is not set in appsettings.json.");
            }

            // 3. Configure DbContextOptions for SQLite
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

            // Now correctly using UseSqlite()
            optionsBuilder.UseSqlite(connectionString,
                sqliteOptions => sqliteOptions.MigrationsAssembly(typeof(DataContext).Assembly.FullName));

            // 4. Create and return the DataContext
            return new DataContext(optionsBuilder.Options);
        }
    }
}
