//-----------------------------------------------------------------------------
// <copyright file="MyUntypedResourceMapper.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.AspNetCore.OData.Formatter.Serialization;
using System.Text.Json;

namespace UntypedApp.Extensions;

public class MyUntypedResourceMapper : DefaultUntypedResourceMapper
{
    public override IDictionary<string, object> Map(object resource, ODataSerializerContext context)
    {
        if (resource is JsonDocument document)
        {
            if (document.RootElement.ValueKind == JsonValueKind.Object)
            {
                IDictionary<string, object> values = (IDictionary<string, object>)document.Deserialize(typeof(IDictionary<string, object>));

                IDictionary<string, object> newValues = new Dictionary<string, object>();
                foreach (var item in values)
                {
                    JsonElement element = (JsonElement)item.Value;
                    switch(element.ValueKind)
                    {
                        case JsonValueKind.String:
                            newValues[item.Key] = element.GetString();
                            break;

                        case JsonValueKind.True:
                            newValues[item.Key] = true;
                            break;

                        case JsonValueKind.Number:
                            newValues[item.Key] = element.GetInt32();
                            break;
                    }
                }

                return newValues;
            }
        }

        return base.Map(resource, context);
    }
}
