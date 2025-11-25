using Microsoft.AspNetCore.OData.Edm;
using Microsoft.OData.ModelBuilder;
using ODataMinimalApi.Models;

namespace ODataMinimalApi.Extensions
{
    public class MyODataModelConfiguration : IODataModelConfiguration
    {
        public ODataModelBuilder Apply(HttpContext context, ODataModelBuilder builder, Type clrType)
        {
            if (context.Request.Path.StartsWithSegments("/modelconfig/customers", StringComparison.OrdinalIgnoreCase))
            {
                builder.EntityType<Info>();
               // builder.EntityType<Address>();
            }

            return builder;
        }
    }
}
