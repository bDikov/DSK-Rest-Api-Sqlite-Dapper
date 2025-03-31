using Microsoft.Data.Sqlite;

namespace DSK.Infrastructure.Interfaces.DbHelpers;

public interface IDbHelper
{
    Task<SqliteConnection> GetInMemoryDbConnectionAsync(CancellationToken cancellationToken);

}
