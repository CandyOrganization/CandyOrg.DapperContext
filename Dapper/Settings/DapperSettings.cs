using CandyOrg.DapperContext.Common.Interfaces.Dapper.Settings;
using Microsoft.Extensions.Configuration;

namespace CandyOrg.DapperContext.Dapper.Settings;

public class DapperSettings : IDapperSettings
{
    public DapperSettings(IConfiguration configuration)
    {
        ConnectionString = configuration.GetConnectionString("Database");
    }

    public string ConnectionString { get; set; }
}