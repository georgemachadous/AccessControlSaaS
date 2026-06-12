using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace AccessControl.Infrastructure.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AccessControlDbContext>
{
    public AccessControlDbContext CreateDbContext(string[] args)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{env}.json", optional: true)
            .AddEnvironmentVariables();

        var config = builder.Build();
        var conn = config.GetConnectionString("Default") ?? config.GetConnectionString("DefaultConnection") ?? "Data Source=accesscontrol.db";

        var optionsBuilder = new DbContextOptionsBuilder<AccessControlDbContext>();
        optionsBuilder.UseSqlite(conn);
        return new AccessControlDbContext(optionsBuilder.Options);
    }
}
