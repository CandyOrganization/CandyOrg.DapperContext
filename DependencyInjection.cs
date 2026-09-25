using System.Reflection;
using CandyOrg.DapperContext.Common.Interfaces.Dapper;
using CandyOrg.DapperContext.Common.Interfaces.Dapper.Settings;
using CandyOrg.DapperContext.Dapper.Settings;
using CandyOrg.DapperContext.Dapper;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CandyOrg.DapperContext;

public static class DependencyInjection
{
    public static IServiceCollection AddDapper(this IServiceCollection services)
    {
        services.AddSingleton<IDapperSettings, DapperSettings>();
        services.AddSingleton<IDapperContext, Dapper.DapperContext>();
        return services;
    }
    
    public static IServiceCollection AddMigrations(this IServiceCollection services, IConfiguration configuration, Assembly migrationsAssembly)
    {
        services
            .AddLogging(c => c.AddFluentMigratorConsole())
            .AddFluentMigratorCore()
            .ConfigureRunner(c => c
                .AddPostgres()
                .WithGlobalConnectionString(configuration.GetConnectionString("Database"))
                .ScanIn(migrationsAssembly));
        
        return services;
    }

    public static IServiceProvider UseMigrations(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var runner = scope.ServiceProvider.GetService<IMigrationRunner>();
        runner.MigrateUp();
        
        return serviceProvider;
    }
}