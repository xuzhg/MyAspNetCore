using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace NewQueryOptionIn8.Models
{
    public class ModelBuilder
    {
        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Product>("Products");
            builder.EntitySet<Sale>("Sales");
            return builder.GetEdmModel();
        }
    }
}
