using DSK.Infrastructure.Configurations;
using DSK.Infrastructure.Interfaces.DbHelpers;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;

namespace DSK.Infrastructure.Services;

public class DbHelper : IDbHelper
{
    private readonly string _connectionString;
    private SqliteConnection? _inMemoryDbConnection = null;
    public DbHelper(IOptions<DatabaseSettings> databaseSettings)
    {
        _connectionString = databaseSettings.Value.ConnectionString;
    }
    public async Task<SqliteConnection> GetInMemoryDbConnectionAsync(CancellationToken cancellationToken)
    {
        if (_inMemoryDbConnection == null)
        {
            _inMemoryDbConnection = new SqliteConnection("Data Source=:memory:");
            _inMemoryDbConnection.Open();

            var physicalConnection = await GetPhysicalConnectionAsync(cancellationToken);

            physicalConnection.BackupDatabase(_inMemoryDbConnection);
            await physicalConnection.CloseAsync();

            return _inMemoryDbConnection;
        }
        return _inMemoryDbConnection;
    }

    public async Task<SqliteConnection> GetPhysicalConnectionAsync(CancellationToken cancellationToken)
    {
        var connection = new SqliteConnection("Data Source=" + _connectionString + ";Mode=ReadWrite");
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}
