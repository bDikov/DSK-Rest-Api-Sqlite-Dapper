using DSK.Infrastructure.Database.Enums;
using DSK.Infrastructure.Database.Models;
using DSK.Infrastructure.Tests.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace DSK.Infrastructure.Tests.DbTests;

[TestClass]
public class DbCotextTests
{
    [Test]
    public void Create_Credit()
    {
        // Arrange  
        using var factory = new DbContextFactory();
        using var context = factory.Create();

        var credit1Id = Guid.NewGuid().ToString();

        CreditDbModel credit = new()
        {
            Id = credit1Id,
            Number = "CREDIT001",
            Name = "Alice Smith",
            Amount = 5000,
            RequestDate = DateTime.UtcNow,
            Status = CreditStatusType.Created
        };

        // Act  
        context.Add(credit);
        context.SaveChanges();

        // Assert  
        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(context.Credits.Count() == 1);
        var actual = context.Credits.First();
        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(credit1Id, actual.Id);
    }

    [Test]
    public void Create_Invoice_With_Existing_Credit()
    {
        // Arrange  
        using var factory = new DbContextFactory();
        using var context = factory.Create();

        var credit1Id = Guid.NewGuid().ToString();
        CreditDbModel credit = new()
        {
            Id = credit1Id,
            Number = "CREDIT001",
            Name = "Alice Smith",
            Amount = 5000,
            RequestDate = DateTime.UtcNow,
            Status = CreditStatusType.Created
        };

        context.Add(credit);
        context.SaveChanges();

        var invoice = new InvoiceDbModel()
        {
            Id = Guid.NewGuid().ToString(),
            CreditId = credit1Id, // Linking to the existing credit  
            CreatedOn = DateTime.UtcNow,
            Number = "INVOICE001",
            Amount = 1500
        };

        // Act  
        context.Add(invoice);
        context.SaveChanges();

        // Assert  
        Assert.AreEqual(1, context.Invoices.Count());
        var actualInvoice = context.Invoices.First();
        Assert.AreEqual("INVOICE001", actualInvoice.Number);
        Assert.AreEqual(1500, actualInvoice.Amount);
        Assert.AreEqual(credit1Id, actualInvoice.CreditId);
    }

    [Test]
    public void Retrieve_Credit_By_Id()
    {
        // Arrange  
        using var factory = new DbContextFactory();
        using var context = factory.Create();

        var credit1Id = Guid.NewGuid().ToString();
        CreditDbModel credit = new()
        {
            Id = credit1Id,
            Number = "CREDIT001",
            Name = "Alice Smith",
            Amount = 5000,
            RequestDate = DateTime.UtcNow,
            Status = CreditStatusType.Created
        };

        context.Add(credit);
        context.SaveChanges();

        // Act  
        var retrievedCredit = context.Credits.Find(credit1Id);

        // Assert  
        Assert.IsNotNull(retrievedCredit);
        Assert.AreEqual(credit1Id, retrievedCredit.Id);
    }

    [Test]
    public void Update_Credit()
    {
        // Arrange  
        using var factory = new DbContextFactory();
        using var context = factory.Create();

        var credit1Id = Guid.NewGuid().ToString();
        CreditDbModel credit = new()
        {
            Id = credit1Id,
            Number = "CREDIT001",
            Name = "Alice Smith",
            Amount = 5000,
            RequestDate = DateTime.UtcNow,
            Status = CreditStatusType.Created
        };

        context.Add(credit);
        context.SaveChanges();

        // Act  
        credit.Name = "Alice Updated";
        context.Update(credit);
        context.SaveChanges();

        // Assert  
        var updatedCredit = context.Credits.First(c => c.Id == credit1Id);
        Assert.AreEqual("Alice Updated", updatedCredit.Name);
    }

    [Test]
    public void Create_Invoice_Without_Existing_Credit_Should_Throw_Exception()
    {
        // Arrange  
        using var factory = new DbContextFactory();
        using var context = factory.Create();

        InvoiceDbModel invoice = new()
        {
            Id = Guid.NewGuid().ToString(),
            CreditId = Guid.NewGuid().ToString(),
            CreatedOn = DateTime.UtcNow,
            Number = "INVOICE001",
            Amount = 1500
        };

        // Act & Assert  
        Assert.ThrowsException<DbUpdateException>(() =>
        {
            context.Add(invoice);
            context.SaveChanges();
        });
    }
}
