using MySqlConnector;

namespace RevitFlow.API.Data;

public class DbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("MariaDb")
            ?? throw new InvalidOperationException("MariaDb connection string is missing.");
    }

    public MySqlConnection CreateConnection()
        => new MySqlConnection(_connectionString);
}
