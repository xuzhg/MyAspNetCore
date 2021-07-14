using FilterExtensionFor801.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilterExtensionFor801.Controllers
{
    public class CustomersController : Controller
    {
        private FilterDbContext _context;
        public CustomersController(FilterDbContext context)
        {
            context.Database.EnsureCreated();
            if (!context.Customers.Any())
            {
                Customer customer = new Customer
                {
                    Name = "First",
                    CustomMetadata = "{\"CustomPropA\": \"CustomValA\" }"
                };
                context.Customers.Add(customer);

                customer = new Customer
                {
                    Name = "Second",
                    CustomMetadata = "{\"CustomPropB\": \"CustomValB\" }"
                };
                context.Customers.Add(customer);

                customer = new Customer
                {
                    Name = "Third",
                    CustomMetadata = "{\"CustomPropA\": \"CustomValA\", \"CustomPropC\": \"CustomValC\" }"
                };
                context.Customers.Add(customer);

                context.SaveChanges();
            }

            _context = context;
        }

        //[EnableQuery]
        public IActionResult Get(ODataQueryOptions<Customer> queryOptions)
        {
            if (queryOptions.Filter != null)
            {
                var model = EdmModelBuilder.GetEdmModel2();
                IEdmEntitySet entitySet = model.EntityContainer.FindEntitySet("Customers");
                EntitySetSegment segment = new EntitySetSegment(entitySet);
                ODataPath path = new ODataPath(segment);
        //        Request.ODataFeature().Model = model; // if you have these codes, the output is different
         //       Request.ODataFeature().Path = path;
                ODataQueryContext context = new ODataQueryContext(model, typeof(Customer), path);

                // I hard code the rawValue, you should retrieve the filter clause from queryOptions to construct the it.
                string rawValue = "contains(CustomMetadata, 'CustomPropA')";

                ODataQueryOptionParser parser = new ODataQueryOptionParser(model, path, new Dictionary<string, string>
                {
                    { "$filter", rawValue }
                });

                FilterQueryOption filter = new FilterQueryOption(rawValue, context, parser);
                return Ok(filter.ApplyTo(_context.Customers, new ODataQuerySettings()));
            }

            return Ok(_context.Customers);
        }
    }
}
