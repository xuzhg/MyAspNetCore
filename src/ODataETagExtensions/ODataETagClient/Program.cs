using Default;
using Microsoft.OData.Client;
using ODataETagWebApi.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ODataETagClient
{
    class Program
    {
        async static Task Main(string[] args)
        {
            Guid label = new Guid("00000000-029B-484E-A257-111122223333");
            await ListCustomers();

            await CreateCustomer("Sam");
            await CreateCustomer("Lucas", label);

            await UpdateCustomer(1, "Peter");
            await UpdateCustomer(2, "John", label);
        }

        async static Task ListCustomers()
        {
            Console.WriteLine("List Customers: ");

            Container context = GetContext();
            IEnumerable<Customer> customers = await context.Customers.ExecuteAsync();
            foreach (var customer in customers)
            {
                Console.WriteLine("  {0}) Name={1,-10}Label={2}", customer.Id, customer.Name + ",", customer.Label);
            }

            Console.WriteLine();
        }

        async static Task CreateCustomer(string name, Guid? label = null)
        {
            Console.WriteLine($"Create a new Customer: name={name}");
            Container context = GetContext();

            Customer newCustomer = new Customer
            {
                Name = name,
                Label = label ?? Guid.Empty
            };

            context.AddToCustomers(newCustomer);

            DataServiceResponse responses = await context.SaveChangesAsync();
            foreach (var response in responses)
            {
                Console.WriteLine($"  StatusCode={response.StatusCode}");
            }

            Console.WriteLine();

            // List Customers
            await ListCustomers();
        }

        async static Task UpdateCustomer(int key, string name, Guid? label = null)
        {
            Console.WriteLine($"Update Customers({key}) with name={name}: ");
            Container context = GetContext();

            Customer customer = context.Customers.ByKey(key).GetValue();
            customer.Name = name;
            if (label != null)
            {
                customer.Label = label.Value;
            }

            context.UpdateObject(customer);

            try
            {
                DataServiceResponse responses = await context.SaveChangesAsync();
                foreach (var response in responses)
                {
                    Console.WriteLine($"  StatusCode={response.StatusCode}");
                }
            }
            catch (DataServiceRequestException ex)
            {
                foreach (var response in ex.Response)
                {
                    Console.WriteLine($"  StatusCode={response.StatusCode},\n  Message={ex.InnerException.Message}");
                }
            }

            Console.WriteLine();

            // List Customers
            await ListCustomers();
        }

        private static Container GetContext()
        {
            var serviceRoot = "http://localhost:5000/odata/";

            var context = new Container(new Uri(serviceRoot));
         //   context.MergeOption = MergeOption.OverwriteChanges;//
            context.Configurations.ResponsePipeline.OnEntityMaterialized(args =>
            {
                if (args.Entity is Customer customer)
                {
                    if (args.Entry.ETag != null)
                    {
                        customer.Label = DecodeETag(args.Entry.ETag);
                    }
                }
            });

            context.Configurations.RequestPipeline.OnEntryStarting(args =>
            {
                Customer customer = args.Entity as Customer;
                if (customer != null && customer.Label != Guid.Empty)
                {
                    args.Entry.ETag = EncodeETag(customer.Label);
                }

                args.Entry.Properties = args.Entry.Properties.ToList().Where(c => c.Name != "Label");
            });

            return context;
        }

        private static string EncodeETag(Guid guid)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(guid.ToString());
            return Convert.ToBase64String(bytes);
        }

        private static Guid DecodeETag(string etag)
        {
            byte[] base64 = Convert.FromBase64String(etag);
            string base64Str = Encoding.UTF8.GetString(base64);
            return new Guid(base64Str);
        }
    }
}
