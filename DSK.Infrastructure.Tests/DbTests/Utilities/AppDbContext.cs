using DSK.Infrastructure.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace DSK.Infrastructure.Tests.DbTests.Utilities;

public class AppDbContext : DbContext
{
    public DbSet<CreditDbModel> Credits { get; set; }
    public DbSet<InvoiceDbModel> Invoices { get; set; }

    public AppDbContext(DbContextOptions options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CreditDbModel>()
       .HasMany(c => c.Invoices)
       .WithOne()
       .HasForeignKey(i => i.CreditId);
    }
}