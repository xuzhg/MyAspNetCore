using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilterExtensionFor801.Models
{
    public class EdmModelBuilder
    {
        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");

            builder.EntityType<Customer>().Ignore(c => c.CustomMetadata);
            builder.EntityType<Customer>().ComplexProperty(c => c.Metadata);

            return builder.GetEdmModel();
        }

        public static IEdmModel GetEdmModel2()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");

            builder.EntityType<Customer>().Property(c => c.CustomMetadata);
          //  builder.EntityType<Customer>().ComplexProperty(c => c.Metadata);

            return builder.GetEdmModel();
        }
    }
}
