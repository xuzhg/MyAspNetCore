//-----------------------------------------------------------------------------
// <copyright file="Program.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.OData;
using Microsoft.OData.Json;
using ODataCborExample.Extensions;
using ODataCborExample.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().
    AddOData(opt =>
        opt.EnableQueryFeatures()
        .AddRouteComponents("odata", EdmModelBuilder.GetEdmModel(),
            services =>
            {
                // for JSON Reader factory
                var selector = services.First(s => s.ServiceType == typeof(IJsonReaderFactory));
                services.Remove(selector);
                services.Add(new ServiceDescriptor(selector.ImplementationType, implementationType: selector.ImplementationType, lifetime: Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton));

                services.AddScoped<IJsonReaderFactory>(s =>
                {
                    return new CborODataJsonReaderFactory(
                        s.GetRequiredService<ODataMessageInfo>(),
                        (IJsonReaderFactory)s.GetRequiredService(selector.ImplementationType));
                });

                // For JSON Writer factory
                services
                .AddScoped<IStreamBasedJsonWriterFactory, CborODataJsonWriterFactory>()
                .AddSingleton<ODataMediaTypeResolver>(sp => new CborMediaTypeResolver())
               ;
            }
        ));

builder.Services.AddScoped<IMessageWriter, LoggingMessageWriter>();

builder.Services.AddControllers(opt =>
{
    var odataFormatter = opt.OutputFormatters.OfType<ODataOutputFormatter>().First();
   odataFormatter.SupportedMediaTypes.Add("application/cbor");

    var odataInputFormatter = opt.InputFormatters.OfType<ODataInputFormatter>().First();
    //odataInputFormatter.SupportedMediaTypes.Add("application/cbor"); // This line seems not necessary.
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseODataRouteDebug();

app.UseAuthorization();

app.UseMyCborLogger();

app.MapControllers();

app.Run();
