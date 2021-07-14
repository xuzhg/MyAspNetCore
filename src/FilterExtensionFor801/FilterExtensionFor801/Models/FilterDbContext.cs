using Microsoft.EntityFrameworkCore;

namespace FilterExtensionFor801.Models
{
    public class FilterDbContext : DbContext
    {
        public FilterDbContext(DbContextOptions<FilterDbContext> options)
            : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
    }
}
