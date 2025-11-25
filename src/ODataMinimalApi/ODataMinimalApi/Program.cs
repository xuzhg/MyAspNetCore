using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Edm;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;
using ODataMinimalApi.Extensions;
using ODataMinimalApi.Models;

IEdmModel model = EdmModelBuilder.GetEdmModel();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDb>(options => options.UseInMemoryDatabase("CustomerOrderList"));
builder.Services.AddSingleton<IODataModelConfiguration, MyODataModelConfiguration>();

builder.Services.AddOData(q => q.EnableAll());

var app = builder.Build();

app.MakeSureDbCreated();

app.UseODataMiniBatching("/odata/$batch", model);

// With or with `WithODataResult`
app.MapGet("json/customers", (ApplicationDb db) => db.Customers.Include(s => s.Orders));


app.MapGet("odata/customers", (ApplicationDb db) => db.Customers.Include(s => s.Orders))
    .WithODataResult()
    ;

app.MapGet("modelconfig/customers", (ApplicationDb db) => db.Customers.Include(s => s.Orders))
    .WithODataResult()
    ;

app.MapGet("/withmodel/customers", (ApplicationDb db) => db.Customers.Include(s => s.Orders))
    .WithODataResult()
    .WithODataModel(model)
    ;

app.MapGet("/v401/customers", (ApplicationDb db) => db.Customers.Include(s => s.Orders))
    .WithODataResult()
    .WithODataModel(model)
    .WithODataVersion(ODataVersion.V401)
    ;

app.MapGet("/baseaddress/customers", (ApplicationDb db) => db.Customers.Include(s => s.Orders))
    .WithODataResult()
    .WithODataModel(model)
    .WithODataBaseAddressFactory(c => new Uri("http://abc.com"))
    ;


app.MapGet("/queryoptions/customers", (ApplicationDb db, ODataQueryOptions<Customer> queryOptions) =>
    queryOptions.ApplyTo(db.Customers.Include(s => s.Orders)))
    .WithODataResult()
    ;

app.MapGet("queryfilter/customers", (ApplicationDb db) => db.Customers.Include(s => s.Orders))
    .AddODataQueryEndpointFilter()
    .WithODataResult()
    ;

app.MapODataServiceDocument("/$document", model);

app.MapODataMetadata("/$metadata", model);


app.MapGet("function/customers/addsuffix(suffix={suffix})", (ApplicationDb db, string suffix) =>
{
    var customers = db.Customers.ToList();
    foreach (var customer in customers)
    {
        customer.Name += suffix;
    }
    return customers;
})
    .WithODataResult()
    .WithODataModel(model)
    .AddODataQueryEndpointFilter()
    .WithODataPathFactory(
        (h, t) =>
        {
            string suffix = h.GetRouteValue("suffix") as string;
            IEdmEntitySet customers = model.FindDeclaredEntitySet("Customers");
            IEdmOperation function = model.FindDeclaredOperations("Default.AddNameSuffixForAllCustomer").Single();
            IList<OperationSegmentParameter> parameters = new List<OperationSegmentParameter>
            {
                new OperationSegmentParameter("suffix", suffix)
            };
            return new ODataPath(new EntitySetSegment(customers), new OperationSegment(function, parameters, customers));
        });

app.MapPost("action/customers/{id}/rateByName", (ApplicationDb db, int id, ODataActionParameters parameters) =>
{
    Customer customer = db.Customers.FirstOrDefault(s => s.Id == id);
    if (customer == null)
    {
        return null; // should return Results.NotFound();
    }
    ;

    return $"{customer.Name}: {System.Text.Json.JsonSerializer.Serialize(parameters)}";
})
            .WithODataResult()
            .WithODataModel(model)
            .WithODataPathFactory(
                (h, t) =>
                {
                    string idStr = h.GetRouteValue("id") as string;
                    int id = int.Parse(idStr);
                    IEdmEntitySet customers = model.FindDeclaredEntitySet("Customers");

                    IDictionary<string, object> keysValues = new Dictionary<string, object>();
                    keysValues["Id"] = id;

                    IEdmAction action = model.SchemaElements.OfType<IEdmAction>().First(a => a.Name == "RateByName");

                    return new ODataPath(new EntitySetSegment(customers),
                        new KeySegment(keysValues, customers.EntityType, customers),
                        new OperationSegment(action, null)
                        );
                });

app.MapPatch("delta/customers/{id}", (ApplicationDb db, int id, Delta<Customer> delta) =>
{
    Customer customer = db.Customers.FirstOrDefault(s => s.Id == id);
    if (customer == null)
    {
        return null;
    }

    delta.Patch(customer);

    return customer;
})
            .WithODataResult()
            .WithODataModel(model)
            .AddODataQueryEndpointFilter()
            .WithODataPathFactory(
                (h, t) =>
                {
                    string idStr = h.GetRouteValue("id") as string;
                    int id = int.Parse(idStr);
                    IEdmEntitySet customers = model.FindDeclaredEntitySet("Customers");

                    IDictionary<string, object> keysValues = new Dictionary<string, object>();
                    keysValues["Id"] = id;
                    return new ODataPath(new EntitySetSegment(customers), new KeySegment(keysValues, customers.EntityType, customers));
                });


app.MapGet("/customers1", (ApplicationDb db) => db.Customers.Include(s => s.Orders))
    ;

app.MapGet("/customers", (ApplicationDb db) => db.Customers.Include(s => s.Orders))
    .WithODataResult()
    ;


app.Run();


