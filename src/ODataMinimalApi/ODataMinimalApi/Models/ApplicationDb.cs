using Microsoft.EntityFrameworkCore;
namespace ODataMinimalApi.Models;

public class ApplicationDb : DbContext
{
    public ApplicationDb(DbContextOptions<ApplicationDb> options) : base(options) { }

    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>().HasKey(x => x.Id);
        modelBuilder.Entity<Order>().HasKey(x => x.Id);
        modelBuilder.Entity<Customer>().OwnsOne(x => x.Info);
        modelBuilder.Entity<Customer>().OwnsOne(x => x.Location);
    }
}
