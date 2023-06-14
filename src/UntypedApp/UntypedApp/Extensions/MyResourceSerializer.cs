//-----------------------------------------------------------------------------
// <copyright file="MyResourceSerializer.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.OData.Edm;

namespace UntypedApp.Extensions;

public class MyResourceSerializer : ODataResourceSerializer
{
    public MyResourceSerializer(IODataSerializerProvider serializerProvider) : base(serializerProvider)
    {
    }

    public override object CreateUntypedPropertyValue(IEdmStructuralProperty structuralProperty, ResourceContext resourceContext, out IEdmTypeReference actualType)
    {
        // add your logics here to return some values for untyped properties.
        // If returns an instance of ODataProperty, it will be used to write as property directly
        // Others, it will be used to write as resource/resourceSet.
        return base.CreateUntypedPropertyValue(structuralProperty, resourceContext, out actualType);
    }
}
