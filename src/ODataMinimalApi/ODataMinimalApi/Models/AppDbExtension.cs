namespace ODataMinimalApi.Models;

static class AppDbExtension
{
    public static void MakeSureDbCreated(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<ApplicationDb>();

            if (context.Customers.Count() == 0)
            {
                var customers = new List<Customer>
                {
                    new Customer { Id = 1,
                        Name = "Alice",
                        Info = new Info { Email = "alice@example.com", Phone = "123-456-7819" },
                        Location = new Address { Street = "123 Main St", City = "Redmond" },
                        Orders = [
                            new Order { Id = 11, Amount = 9},
                            new Order { Id = 12, Amount = 19},
                        ] },
                    new Customer { Id = 2,
                        Name = "Johnson",
                        Info = new Info { Email = "johnson@abc.com", Phone = "233-468-7289" },
                        Location = new Address { Street = "228 Ave NE", City = "Sammamish" },
                        Orders = [
                            new Order { Id = 21, Amount = 8},
                            new Order { Id = 22, Amount = 76},
                        ] },
                };

                foreach (var s in customers)
                {
                    context.Customers.Add(s);
                    foreach (var o in s.Orders)
                    {
                        context.Orders.Add(o);
                    }
                }

                context.SaveChanges();
            }
        }
    }
}