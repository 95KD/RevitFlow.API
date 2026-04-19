using MySqlConnector;

namespace RevitFlow.API.Data;

public class DbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection connection string is missing.");
    }

    public MySqlConnection CreateConnection()
        => new MySqlConnection(_connectionString);
}
