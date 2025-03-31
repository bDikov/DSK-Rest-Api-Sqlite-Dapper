using DSK.Infrastructure.Configurations;
using DSK.Infrastructure.Interfaces.DbHelpers;
using DSK.Infrastructure.Services;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using Moq;

namespace DSK.Infrastructure.Tests.DbTests
{
    [TestFixture]
    public class DbHelperTests
    {
        private Mock<IOptions<DatabaseSettings>> _mockOptions;
        private Mock<IDbHelper> _mockDbHelper;
        private string _testConnectionString;
        private DbHelper _dbHelper;

        [SetUp]
        public void SetUp()
        {
            // Prepare mock options for DatabaseSettings  
            _testConnectionString = ":memory:"; // Using in-memory for testing  
            _mockOptions = new Mock<IOptions<DatabaseSettings>>();
            _mockOptions.Setup(opt => opt.Value).Returns(new DatabaseSettings { ConnectionString = _testConnectionString });
            _mockDbHelper = new Mock<IDbHelper>();
            _dbHelper = new DbHelper(_mockOptions.Object);
        }

        [Test]
        public async Task GetInMemoryDbConnectionAsync_CreatesConnection()
        {
            // Arrange  
            var expectedConnection = new SqliteConnection("Data Source=:memory:");
            expectedConnection.Open();

            // Act  
            var connection = await _dbHelper.GetInMemoryDbConnectionAsync(CancellationToken.None);

            // Assert  
            Assert.NotNull(connection);
            Assert.That(connection.ConnectionString, Is.EqualTo("Data Source=:memory:"));
            Assert.That(connection.State, Is.EqualTo(System.Data.ConnectionState.Open));

            // Ensure that calling again returns the same connection  
            var sameConnection = await _dbHelper.GetInMemoryDbConnectionAsync(CancellationToken.None);
            Assert.That(sameConnection, Is.SameAs(connection));
        }

        [Test]
        public async Task GetPhysicalConnectionAsync_CreatesPhysicalConnection()
        {
            // Arrange  
            var connection = await _dbHelper.GetPhysicalConnectionAsync(CancellationToken.None);

            // Assert  
            Assert.NotNull(connection);
            Assert.That(connection.ConnectionString, Is.EqualTo($"Data Source={_testConnectionString};Mode=ReadWrite"));
            Assert.That(connection.State, Is.EqualTo(System.Data.ConnectionState.Open));

            // Clean up  
            await connection.CloseAsync();
        }
    }
}