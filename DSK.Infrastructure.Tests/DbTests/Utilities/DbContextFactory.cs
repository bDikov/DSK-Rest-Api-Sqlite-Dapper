using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DSK.Infrastructure.Tests.DbTests.Utilities;

public class DbContextFactory : IDisposable
{
    private SqliteConnection _connection;
    private AppDbContext _appDbContext;


    public DbContextFactory()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
        var options = new DbContextOptionsBuilder()
            .UseSqlite(_connection)
            .Options;
        _appDbContext = new AppDbContext(options);
        _appDbContext.Database.EnsureCreated();
    }

    public AppDbContext Create()
    {
        return _appDbContext;
    }


    public void Dispose()
    {
        _connection.Close();
    }
}
