using gRPC.OData.Server.Extensions;
using gRPC.OData.Server.Models;
using gRPC.OData.Server.Services;
using Microsoft.AspNetCore.OData;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IShelfBookRepository, ShelfBookInMemoryRepository>();

builder.Services.AddControllers()
    .AddOData(opt => opt.EnableQueryFeatures().AddRouteComponents("odata", EdmModelBuilder.GetEdmModel()));

builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseEndpointDebug(); // send "/$endpoint" in browser to debug

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGrpcService<BookstoreService>();

app.MapControllers();

app.Run();
