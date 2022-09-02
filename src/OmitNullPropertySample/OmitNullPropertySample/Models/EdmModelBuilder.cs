using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace OmitNullPropertySample.Models
{
    public class EdmModelBuilder
    {
        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<School>("Schools");
            builder.EntitySet<Student>("Students");
            return builder.GetEdmModel();
        }
    }
}
