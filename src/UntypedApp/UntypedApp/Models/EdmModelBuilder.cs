//-----------------------------------------------------------------------------
// <copyright file="EdmModelBuilder.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace UntypedApp.Models;

public class EdmModelBuilder
{
    public static IEdmModel GetEdmModel()
    {
        var builder = new ODataConventionModelBuilder();
        builder.ComplexType<Address>(); // add Address complex type explicitly since there's no reference to this type.
        builder.EntitySet<Person>("People");
        EdmModel model = (EdmModel)builder.GetEdmModel();

        // Add a complex type without C# class mapped
        EdmComplexType complexType = new EdmComplexType("UntypedApp.Models", "Message");
        complexType.AddStructuralProperty("Description", EdmCoreModel.Instance.GetString(true));
        complexType.AddStructuralProperty("Status", EdmCoreModel.Instance.GetString(false));
        model.AddElement(complexType);
        return model;
    }
}
