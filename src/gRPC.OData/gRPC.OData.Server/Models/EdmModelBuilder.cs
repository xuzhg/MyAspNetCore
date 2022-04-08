
using Bookstores;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace gRPC.OData.Server.Models
{
    internal class EdmModelBuilder
    {
        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataModelBuilder();

            var shelf = builder.EntityType<Shelf>();
            shelf.HasKey(b => b.Id);
            shelf.Property(b => b.Theme);

            var bookMessage = builder.EntityType<Book>();
            bookMessage.HasKey(b => b.Id);
            bookMessage.Property(b => b.Title);
            bookMessage.Property(b => b.Author);

            builder.EntitySet<Book>("Books");
            builder.EntitySet<Shelf>("Shelves").HasManyBinding(s => s.Books, "Books");

            return builder.GetEdmModel();
        }
    }
}