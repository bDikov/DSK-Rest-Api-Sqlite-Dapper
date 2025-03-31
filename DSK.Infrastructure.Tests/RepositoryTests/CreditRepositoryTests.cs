using AutoMapper;
using Dapper;
using DSK.Domain.Entities;
using DSK.Infrastructure.Database.Models;
using DSK.Infrastructure.Interfaces.DbHelpers;
using DSK.Infrastructure.Repositories;
using Moq;
using System.Data;

namespace DSK.Infrastructure.Tests.RepositoryTests
{
    public class CreditRepositoryTests
    {
        private Mock<IDbHelper> _mockDbHelper;
        private IMapper _mapper;
        private CreditRepository _creditRepository;

        [SetUp]
        public void Setup()
        {
            // Setup AutoMapper  
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreditDbModel, Credit>()
                    .ForMember(dest => dest.Invoices, opt => opt.MapFrom(src => src.Invoices));
                cfg.CreateMap<InvoiceDbModel, Invoice>();
            });
            _mapper = config.CreateMapper();

            // Mock the IDbHelper  
            _mockDbHelper = new Mock<IDbHelper>();
            _creditRepository = new CreditRepository(_mockDbHelper.Object, _mapper);
        }

        [Test]
        public async Task GetCreditsQueryableAsync_ReturnsMappedCredits()
        {
            // Arrange  
            var connectionString = "DataSource=:memory:";
            using var connection = new Microsoft.Data.Sqlite.SqliteConnection(connectionString);
            connection.Open();
            InitializeDatabase(connection);

            _mockDbHelper.Setup(m => m.GetInMemoryDbConnectionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(connection);

            // Act  
            var result = await _creditRepository.GetCreditsQueryableAsync(CancellationToken.None);

            // Assert  
            Assert.IsNotNull(result);
            Assert.That(result.Count(), Is.EqualTo(2)); // Expecting 2 mapped credits  

            var firstCredit = result.First();
            Assert.That(firstCredit.Number, Is.EqualTo("C001"));
            Assert.That(firstCredit.Name, Is.EqualTo("Credit One"));
            Assert.That(firstCredit.Amount, Is.EqualTo(100.00m));
            Assert.That(firstCredit.Invoices, Has.Count.EqualTo(1)); // Check if invoice is mapped  
            Assert.That(firstCredit.Invoices.First().Number, Is.EqualTo("INV001"));
        }

        [Test]
        public async Task GetCreditsQueryableAsync_ReturnsEmptyCollection_WhenNoCreditsExist()
        {
            // Arrange  
            var connectionString = "DataSource=:memory:";
            using var connection = new Microsoft.Data.Sqlite.SqliteConnection(connectionString);
            connection.Open();

            InitializeEmptyDatabase(connection);

            _mockDbHelper.Setup(m => m.GetInMemoryDbConnectionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(connection);

            // Act  
            var result = await _creditRepository.GetCreditsQueryableAsync(CancellationToken.None);

            // Assert  
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetCreditSummaryAsync_ReturnsCorrectSummary()
        {
            // Arrange
            var connectionString = "DataSource=:memory:";
            using var connection = new Microsoft.Data.Sqlite.SqliteConnection(connectionString);
            connection.Open();
            InitializeDatabaseWithSummaryData(connection);

            _mockDbHelper.Setup(m => m.GetInMemoryDbConnectionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(connection);

            // Act
            var result = await _creditRepository.GetCreditSummaryAsync(CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.TotalPaidSum, Is.EqualTo(150.00m));
                Assert.That(result.FullAwaitingPaymentSum, Is.EqualTo(200.00m));
                Assert.That(result.FullAwaitingPaymentSumPercentage, Is.EqualTo(350.00m));
                Assert.That(result.TotalPaidSumPercentage, Is.EqualTo(42.86m));
                Assert.That(result.TotalAwaitingPaymentPercentage, Is.EqualTo(57.14m));
            });
        }

        [Test]
        public async Task GetCreditSummaryAsync_WhenNoCreditsExist_ReturnsZeroValues()
        {
            // Arrange
            var connectionString = "DataSource=:memory:";
            using var connection = new Microsoft.Data.Sqlite.SqliteConnection(connectionString);
            connection.Open();
            InitializeEmptyDatabase(connection);

            _mockDbHelper.Setup(m => m.GetInMemoryDbConnectionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(connection);

            // Act
            var result = await _creditRepository.GetCreditSummaryAsync(CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.TotalPaidSum, Is.EqualTo(0m));
                Assert.That(result.FullAwaitingPaymentSum, Is.EqualTo(0m));
                Assert.That(result.FullAwaitingPaymentSumPercentage, Is.EqualTo(0m));
                Assert.That(result.TotalPaidSumPercentage, Is.EqualTo(0m));
                Assert.That(result.TotalAwaitingPaymentPercentage, Is.EqualTo(0m));
            });
        }

        private void InitializeEmptyDatabase(IDbConnection connection)
        {
            var createCreditsTable = @"  
        CREATE TABLE Credits (  
            Id TEXT PRIMARY KEY,  
            Number TEXT,  
            Name TEXT,  
            Amount REAL,  
            RequestDate TEXT,  
            Status INTEGER  
        );";

            var createInvoicesTable = @"  
        CREATE TABLE Invoices (  
            Id TEXT PRIMARY KEY,  
            CreditId TEXT,  
            CreatedOn TEXT,  
            Number TEXT,  
            Amount REAL,  
            FOREIGN KEY (CreditId) REFERENCES Credits (Id)  
        );";

            connection.Execute(createCreditsTable);
            connection.Execute(createInvoicesTable);
        }

        private void InitializeDatabase(IDbConnection connection)
        {
            // Create and seed the database schema  
            var createCreditsTable = @"  
                CREATE TABLE Credits (  
                    Id TEXT PRIMARY KEY,  
                    Number TEXT,  
                    Name TEXT,  
                    Amount REAL,  
                    RequestDate TEXT,  
                    Status INTEGER  
                );";

            var createInvoicesTable = @"  
                CREATE TABLE Invoices (  
                    Id TEXT PRIMARY KEY,  
                    CreditId TEXT,  
                    CreatedOn TEXT,  
                    Number TEXT,  
                    Amount REAL,  
                    FOREIGN KEY (CreditId) REFERENCES Credits (Id)  
                );";

            connection.Execute(createCreditsTable);
            connection.Execute(createInvoicesTable);

            // Insert sample data into Credits  
            var insertCredits = @"  
                INSERT INTO Credits (Id, Number, Name, Amount, RequestDate, Status) VALUES  
                ('1', 'C001', 'Credit One', 100.00, '2025-03-31T00:00:00Z', 0),  
                ('2', 'C002', 'Credit Two', 200.00, '2025-03-31T00:00:00Z', 1);";

            connection.Execute(insertCredits);

            // Insert sample data into Invoices  
            var insertInvoices = @"  
                INSERT INTO Invoices (Id, CreditId, CreatedOn, Number, Amount) VALUES  
                ('1', '1', '2025-03-31T00:00:00Z', 'INV001', 50.00);";

            connection.Execute(insertInvoices);
        }

        private void InitializeDatabaseWithSummaryData(IDbConnection connection)
        {
            // Create and seed the database schema  
            var createCreditsTable = @"  
                CREATE TABLE Credits (  
                    Id TEXT PRIMARY KEY,  
                    Number TEXT,  
                    Name TEXT,  
                    Amount REAL,  
                    RequestDate TEXT,  
                    Status INTEGER  
                );";

            connection.Execute(createCreditsTable);

            // Insert sample data into Credits with specific statuses for summary testing
            var insertCredits = @"  
                INSERT INTO Credits (Id, Number, Name, Amount, RequestDate, Status) VALUES  
                ('1', 'C001', 'Credit One', 150.00, '2025-03-31T00:00:00Z', 2),  -- Paid
                ('2', 'C002', 'Credit Two', 200.00, '2025-03-31T00:00:00Z', 1); -- AwaitingPayment";

            connection.Execute(insertCredits);
        }
    }
}