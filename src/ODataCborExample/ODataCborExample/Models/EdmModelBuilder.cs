//---------------------------------------------------------------------
// <copyright file="CustomizedFormat.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//---------------------------------------------------------------------

using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace ODataCborExample.Models;

public static class EdmModelBuilder
{
    public static IEdmModel GetEdmModel()
    {
        var builder = new ODataConventionModelBuilder();
        builder.EntitySet<Book>("Books");
        return builder.GetEdmModel();
    }
}
