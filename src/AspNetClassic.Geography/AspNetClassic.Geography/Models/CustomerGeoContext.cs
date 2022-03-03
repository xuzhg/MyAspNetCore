using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace AspNetClassic.Geography.Models
{
    public class CustomerGeoContext : DbContext
    {
        public CustomerGeoContext() : base("CustomerGeoContextConnect")
        { }

        public DbSet<Customer> Customers { get; set; }

        public static void Init()
        {
            using (CustomerGeoContext context = new CustomerGeoContext())
            {
                if (!context.Customers.Any())
                {
                    // 1
                    Customer customer = new Customer
                    {
                        Name = "Dubhe",
                        Location = DbGeography.FromText("POINT (1 60)"),
                        Line = DbGeography.FromText("LINESTRING (0 0, 1 60)")
                    };
                    context.Customers.Add(customer);

                    // 2
                    customer = new Customer
                    {
                        Name = "Dubhe",
                        Location = DbGeography.FromText("POINT (1.5 50)"),
                        Line = DbGeography.FromText("LINESTRING (1.5 50, 10 49, 15 58)")
                    };
                    context.Customers.Add(customer);
                    context.SaveChanges();
                }
            }
        }
    }
}