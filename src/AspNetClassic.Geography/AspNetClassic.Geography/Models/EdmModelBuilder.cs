using Microsoft.AspNet.OData.Builder;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetClassic.Geography.Models
{
    public class EdmModelBuilder
    {
        public static IEdmModel BuildCustomerEdmModel()
        {
            var builder = new ODataModelBuilder();
            builder.EntitySet<Customer>("Customers");

            var customerType = builder.EntityType<Customer>();
            customerType.HasKey(c => c.Id).Ignore(c => c.Location);
            customerType.Ignore(c => c.Line);
            customerType.Property(c => c.Name);

            // Cannot call customerType.Property(c => c.HomeLocation)
            var customer = builder.StructuralTypes.First(t => t.ClrType == typeof(Customer));
            customer.AddProperty(typeof(Customer).GetProperty("EdmLocation")).Name = "Location";
            customer.AddProperty(typeof(Customer).GetProperty("EdmLine")).Name = "Line";
            return builder.GetEdmModel();
        }
    }
}