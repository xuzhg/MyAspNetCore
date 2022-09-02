using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Formatter.Deserialization;
using Microsoft.AspNetCore.OData.Formatter.Serialization;
using OmitNullPropertySample.Extensions;
using OmitNullPropertySample.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddOData(opt => opt.AddRouteComponents("odata", EdmModelBuilder.GetEdmModel(), 
    services => services.AddSingleton<ODataResourceSerializer, OmitNullResourceSerializer>()).EnableQueryFeatures());

builder.Services.AddTransient<ISchoolStudentRepository, SchoolStudentRepositoryInMemory>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseODataRouteDebug();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
