using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Linq;

namespace EFCoreLinqTest
{
    class Program
    {
        static void Main(string[] args)
        {
            GenerateDB();

            Console.WriteLine("==========================================");
            using (CustomerOrderContext db = new CustomerOrderContext())
            {
                var aa = db.Customers.Select(e => new CusomterDto { FullName = e.FirstName + " " + e.LastName });
                foreach (var a in aa)
                {
                    Console.WriteLine(a.FullName);
                }
            }

            Console.WriteLine("==========================================");
            using (CustomerOrderContext db = new CustomerOrderContext())
            {
                var aa = db.Customers.Take(1).Select(e => new CusomterDto { FullName = e.FirstName + " " + e.LastName });
                foreach (var a in aa)
                {
                    Console.WriteLine(a.FullName);
                }
            }

            Console.WriteLine("==========================================");
            using (CustomerOrderContext db = new CustomerOrderContext())
            {
                var aa = db.Customers.Select(e => new Customer
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    UserName = e.UserName
                })
                .Take(1).Select(e => new CusomterDto { FullName = e.FirstName + " " + e.LastName });
                foreach (var a in aa)
                {
                    Console.WriteLine(a.FullName);
                }
            }

            Console.WriteLine("==========================================");
            using (CustomerOrderContext db = new CustomerOrderContext())
            {
                var aa = db.Customers.Select(e => new Customer
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    UserName = e.UserName
                })
                .Select(e => new CusomterDto { FullName = e.FirstName + " " + e.LastName }).Take(1);
                foreach (var a in aa)
                {
                    Console.WriteLine(a.FullName);
                }
            }

            Console.WriteLine("Done");
        }

        static void GenerateDB()
        {
            using (CustomerOrderContext db = new CustomerOrderContext())
            {
                if (db.Database.EnsureCreated())
                {
                    if (!db.Customers.Any())
                    {
                        Customer c = new Customer { FirstName = "Sam", LastName = "Peter", UserName = "123", City = "Redmond", Street = "156 AVE" };
                        Order o = new Order { Price = 99 };
                        c.Order = o;
                        db.Customers.Add(c);
                        db.Orders.Add(o);

                        c = new Customer { FirstName = "Keruui", LastName = "Xu", UserName = "2123", City = "Bellevue", Street = "Main AVE" };
                        o = new Order { Price = 199 };
                        c.Order = o;
                        db.Customers.Add(c);
                        db.Orders.Add(o);

                        db.SaveChanges();
                    }
                }
            }
        }
    }

    public class CusomterDto
    {
        public string FullName { get; set; }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public Order Order { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public int Price { get; set; }
    }
    public class CustomerOrderContext : DbContext
    {
        public static readonly LoggerFactory MyLoggerFactory
            = new LoggerFactory(new[] { new ConsoleLoggerProvider((_, __) => true, true) });

        private const string ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Database=Demo.ODataEfCoreTestCustomerOrder;Integrated Security = True;ConnectRetryCount=0";

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString).UseLoggerFactory(MyLoggerFactory);
        }
    }
}
