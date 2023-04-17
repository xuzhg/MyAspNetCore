using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace Bookstores // keep the same namespace
{
    public static class ModelBuilder
    {
        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Shelf>("Shelves");
            builder.EntitySet<Book>("Books");
            return builder.GetEdmModel();
        }
    }
}