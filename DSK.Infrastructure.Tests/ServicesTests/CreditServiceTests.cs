using AutoMapper;
using DSK.Domain.Entities;
using DSK.Domain.Repositories;
using DSK.Domain.ValueObject;
using DSK.Infrastructure.Database.Enums;
using DSK.Infrastructure.Services;
using Moq;

namespace DSK.Infrastructure.Tests.ServicesTests
{
    public class CreditServiceTests
    {
        private Mock<ICreditRepository> _mockCreditRepository;
        private Mock<IMapper> _mockMapper;
        private CreditService _creditService;

        [SetUp]
        public void Setup()
        {
            _mockCreditRepository = new Mock<ICreditRepository>();
            _mockMapper = new Mock<IMapper>();
            _creditService = new CreditService(_mockCreditRepository.Object, _mockMapper.Object);
        }

        [Test]
        public async Task GetCreditsAsync_ShouldReturnMappedCredits()
        {
            // Arrange  
            var cancellationToken = CancellationToken.None;

            var repositoryCredits = new List<Credit>
            {
                new Credit(
                    number: "C001",
                    name: "Credit One Mapped",
                    amount: 100.00m,
                    requestDate: DateTime.UtcNow,
                    status: CreditStatusType.AwaitingPayment.ToString(),
                    invoices:
                    [
                        new Invoice(
                            number: "INV001", // Invoice number  
                            amount: 50.00m    // Invoice amount  
                        )
                    ]
                ),
                new Credit(
                    number: "C002",
                    name: "Credit Two Mapped",
                    amount: 200.00m,
                    requestDate: DateTime.UtcNow, // Example request date  
                    status: CreditStatusType.Paid.ToString(),
                    invoices: [] // No invoices for this credit  
                )
            };

            var mappedCredits = new List<Credit>
            {
                new Credit(
                    number: "C001",
                    name: "Credit One Mapped",
                    amount: 100.00m,
                    requestDate: DateTime.UtcNow, // Example request date  
                    status: CreditStatusType.AwaitingPayment.ToString(),
                    invoices:
                    [
                        new Invoice(
                            number: "INV001", // Invoice number  
                            amount: 50.00m    // Invoice amount  
                        )
                    ]
                ),
                new Credit(
                    number: "C002",
                    name: "Credit Two Mapped",
                    amount: 200.00m,
                    requestDate: DateTime.UtcNow,
                    status: CreditStatusType.Paid.ToString(),
                    invoices: []
                )
            };

            _mockCreditRepository.Setup(repo => repo.GetCreditsQueryableAsync(cancellationToken))
                .ReturnsAsync(repositoryCredits);

            _mockMapper.Setup(m => m.Map<IEnumerable<Credit>>(repositoryCredits))
                .Returns(mappedCredits);

            // Act  
            var result = await _creditService.GetCreditsAsync(cancellationToken);

            // Assert  
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(mappedCredits.Count));

            var firstCredit = result.First();
            Assert.Multiple(() =>
            {
                Assert.That(firstCredit.Number, Is.EqualTo(mappedCredits[0].Number));
                Assert.That(firstCredit.Name, Is.EqualTo(mappedCredits[0].Name));
                Assert.That(firstCredit.Amount, Is.EqualTo(mappedCredits[0].Amount));
                Assert.That(firstCredit.Status, Is.EqualTo(mappedCredits[0].Status));
                Assert.That(firstCredit.Invoices, Has.Count.EqualTo(1));
                Assert.That(firstCredit.Invoices.First().Number, Is.EqualTo(mappedCredits[0].Invoices.First().Number));
            });

        }

        [Test]
        public async Task GetCreditsAsync_WhenNoCreditsExist_ShouldReturnEmptyCollection()
        {
            var cancellationToken = CancellationToken.None;
            var repositoryCredits = new List<Credit>();
            var mappedCredits = new List<Credit>();

            _mockCreditRepository.Setup(repo => repo.GetCreditsQueryableAsync(cancellationToken))
                .ReturnsAsync(repositoryCredits);

            _mockMapper.Setup(m => m.Map<IEnumerable<Credit>>(It.IsAny<IEnumerable<Credit>>()))
                .Returns(mappedCredits);

            var result = await _creditService.GetCreditsAsync(cancellationToken);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetCreditSummaryAsync_ShouldReturnMappedSummary()
        {
            // Arrange  
            var cancellationToken = CancellationToken.None;
            var repositorySummary = new CreditSummary
            {
                TotalPaidSum = 150.00m,
                FullAwaitingPaymentSum = 200.00m,
                FullAwaitingPaymentSumPercentage = 350.00m,
                TotalPaidSumPercentage = 42.86m,
                TotalAwaitingPaymentPercentage = 57.14m
            };

            var mappedSummary = new CreditSummary
            {
                TotalPaidSum = 150.00m,
                FullAwaitingPaymentSum = 200.00m,
                FullAwaitingPaymentSumPercentage = 350.00m,
                TotalPaidSumPercentage = 42.86m,
                TotalAwaitingPaymentPercentage = 57.14m
            };

            _mockCreditRepository.Setup(repo => repo.GetCreditSummaryAsync(cancellationToken))
                .ReturnsAsync(repositorySummary);

            _mockMapper.Setup(m => m.Map<CreditSummary>(repositorySummary))
                .Returns(mappedSummary);

            // Act  
            var result = await _creditService.GetCreditSummaryAsync(cancellationToken);

            // Assert  
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.TotalPaidSum, Is.EqualTo(mappedSummary.TotalPaidSum));
                Assert.That(result.FullAwaitingPaymentSum, Is.EqualTo(mappedSummary.FullAwaitingPaymentSum));
                Assert.That(result.FullAwaitingPaymentSumPercentage, Is.EqualTo(mappedSummary.FullAwaitingPaymentSumPercentage));
                Assert.That(result.TotalPaidSumPercentage, Is.EqualTo(mappedSummary.TotalPaidSumPercentage));
                Assert.That(result.TotalAwaitingPaymentPercentage, Is.EqualTo(mappedSummary.TotalAwaitingPaymentPercentage));
            });

        }

        [Test]
        public async Task GetCreditSummaryAsync_WhenNoCreditsExist_ShouldReturnZeroValues()
        {
            // Arrange  
            var cancellationToken = CancellationToken.None;
            var repositorySummary = new CreditSummary
            {
                TotalPaidSum = 0m,
                FullAwaitingPaymentSum = 0m,
                FullAwaitingPaymentSumPercentage = 0m,
                TotalPaidSumPercentage = 0m,
                TotalAwaitingPaymentPercentage = 0m
            };

            var mappedSummary = new CreditSummary
            {
                TotalPaidSum = 0m,
                FullAwaitingPaymentSum = 0m,
                FullAwaitingPaymentSumPercentage = 0m,
                TotalPaidSumPercentage = 0m,
                TotalAwaitingPaymentPercentage = 0m
            };

            _mockCreditRepository.Setup(repo => repo.GetCreditSummaryAsync(cancellationToken))
                .ReturnsAsync(repositorySummary);

            _mockMapper.Setup(m => m.Map<CreditSummary>(repositorySummary))
                .Returns(mappedSummary);

            // Act  
            var result = await _creditService.GetCreditSummaryAsync(cancellationToken);

            // Assert  
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.TotalPaidSum, Is.EqualTo(mappedSummary.TotalPaidSum));
                Assert.That(result.FullAwaitingPaymentSum, Is.EqualTo(mappedSummary.FullAwaitingPaymentSum));
                Assert.That(result.FullAwaitingPaymentSumPercentage, Is.EqualTo(mappedSummary.FullAwaitingPaymentSumPercentage));
                Assert.That(result.TotalPaidSumPercentage, Is.EqualTo(mappedSummary.TotalPaidSumPercentage));
                Assert.That(result.TotalAwaitingPaymentPercentage, Is.EqualTo(mappedSummary.TotalAwaitingPaymentPercentage));
            });

        }
    }
}