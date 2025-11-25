using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace ODataMinimalApi.Models;

public class EdmModelBuilder
{
    public static IEdmModel GetEdmModel()
    {
        var builder = new ODataConventionModelBuilder();

        builder.EntitySet<Customer>("Customers");
        builder.EntitySet<Order>("Orders");
        builder.EntityType<Address>(); // Intentionally built it as entity type
        builder.ComplexType<Info>(); // Intentionally built it as complex type

        var function = builder.EntityType<Customer>().Collection.Function("AddNameSuffixForAllCustomer");
        function.Parameter<string>("suffix");
        function.ReturnsCollectionFromEntitySet<Customer>("Customers");

        var action = builder.EntityType<Customer>().Action("RateByName");
        action.Parameter<string>("name");
        action.Parameter<int>("age");
        action.Returns<string>();

        return builder.GetEdmModel();
    }
}