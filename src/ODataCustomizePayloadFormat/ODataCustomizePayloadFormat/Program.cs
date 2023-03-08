using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.OData;
using ODataCustomizePayloadFormat.Extensions;
using ODataCustomizePayloadFormat.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().
    AddOData(opt =>
        opt.EnableQueryFeatures()
        .AddRouteComponents("odata", EdmModelBuilder.GetEdmModel(),
            service => service.AddSingleton<ODataMediaTypeResolver>(sp => new CustomizedMediaTypeResolver())));
          // service => service.AddSingleton<ODataMediaTypeResolver>(sp => CsvMediaTypeResolver.Instance))); // keep here for post reference

builder.Services.AddControllers(opt =>
{
    var odataFormatter = opt.OutputFormatters.OfType<ODataOutputFormatter>().First();
    odataFormatter.SupportedMediaTypes.Add("text/csv");
    odataFormatter.SupportedMediaTypes.Add("application/yaml");
    odataFormatter.SupportedMediaTypes.Add("application/cbor");
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseODataRouteDebug();

app.UseAuthorization();

app.MapControllers();

app.Run();
