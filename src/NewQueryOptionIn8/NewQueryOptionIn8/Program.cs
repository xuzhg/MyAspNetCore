using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Query.Expressions;
using NewQueryOptionIn8.Extensions;
using NewQueryOptionIn8.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IProductSaleRepository, ProductSaleInMemoryRespository>();

builder.Services.AddControllers().
    AddOData(opt => opt.EnableQueryFeatures()
    .AddRouteComponents("odata", ModelBuilder.GetEdmModel(), services => services.AddSingleton<ISearchBinder, ProductSaleSearchBinder>()));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
