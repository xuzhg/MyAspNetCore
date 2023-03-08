//---------------------------------------------------------------------
// <copyright file="CustomizedFormat.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//---------------------------------------------------------------------

using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace ODataCustomizePayloadFormat.Models;

public static class EdmModelBuilder
{
    public static IEdmModel GetEdmModel()
    {
        var builder = new ODataConventionModelBuilder();
        builder.EntitySet<Book>("Books");
        builder.EntitySet<Customer>("Customers");
        return builder.GetEdmModel();
    }
}
