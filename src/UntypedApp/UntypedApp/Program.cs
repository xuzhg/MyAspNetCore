//-----------------------------------------------------------------------------
// <copyright file="Program.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Formatter.Serialization;
using UntypedApp.Extensions;
using UntypedApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddOData(opt =>
        opt.EnableQueryFeatures()
           .AddRouteComponents("odata", EdmModelBuilder.GetEdmModel(), services =>
           services.AddSingleton<ODataResourceSerializer, MyResourceSerializer>()
               .AddSingleton<IUntypedResourceMapper, MyUntypedResourceMapper>())
           );

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseODataRouteDebug();

app.UseAuthorization();

app.MapControllers();

app.Run();
