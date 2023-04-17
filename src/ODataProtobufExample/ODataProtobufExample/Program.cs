using Bookstores;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.OData;
using ODataProtobufExample.Models;
using ODataProtobufExample.Protobuf;
using Google.Protobuf;

var builder = WebApplication.CreateBuilder(args);

// WriteShelf();
// WriteBook();

// Add services to the container.
builder.Services.AddTransient<IShelfBookRepository, ShelfBookInMemoryRepository>();

builder.Services.AddControllers().
    AddOData(opt =>
        opt.EnableQueryFeatures()
        .AddRouteComponents("odata", ModelBuilder.GetEdmModel(),
            service => service.AddSingleton<ODataMediaTypeResolver>(sp => new ProtobufMediaTypeResolver())));

builder.Services.AddControllers(opt =>
{
    var odataFormatter = opt.OutputFormatters.OfType<ODataOutputFormatter>().First();
    odataFormatter.SupportedMediaTypes.Add("application/x-protobuf");
});

var app = builder.Build();

// app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
app.UseODataRouteDebug(); // send "/$odata" in browser to debug

app.UseAuthorization();

app.MapControllers();

app.Run();


void WriteShelf()
{
    Shelf shelf = new Shelf
    {
        Theme = "Non-Fiction"
    };
    using (var output = File.Create(@"d:\shelf.dat"))
    {
        shelf.WriteTo(output);
    }
}

void WriteBook()
{
    Book book = new Book
    {
        Author = "Sam Xu",
        Title = "MyStory",
        Isbn = "444-1314-1115",
        Page = 115
    };
    using (var output = File.Create(@"d:\book.dat"))
    {
        book.WriteTo(output);
    }
}