//---------------------------------------------------------------------
// <copyright file="CborODataWriter.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//---------------------------------------------------------------------

using System.Formats.Cbor;
using Microsoft.AspNetCore.OData.Formatter.Wrapper;
using Microsoft.OData;
using Microsoft.OData.Edm;

namespace ODataCustomizePayloadFormat.Extensions.Cbor;


/// <summary>
/// This is just for test and your reference.
/// Since CBOR is binary representation of JSON, we should use other way to customize CBOR.
/// </summary>
public class CborODataWriter : CustomizedWriter
{
    private CborWriter _cborWriter = new CborWriter();

    public CborODataWriter(CustomizedOutputContext context, IEdmStructuredType structuredType)
        : base (context, structuredType)
    {
    }

    protected override void WriteItems()
    {
        if (TopLevelItem == null)
        {
            return;
        }

        // For simplicity, NOT consider the inheritance
        if (TopLevelItem is ODataResourceSetWrapper topResourceSet)
        {
            WriteResourceSet(topResourceSet);
        }
        else if (TopLevelItem is ODataResourceWrapper topResource)
        {
            WriteResource(topResource);
        }

        var encoded = _cborWriter.Encode();

        // If you want to write the byte[] directly into response, use this line code.
         Context.Stream.Write(encoded, 0, encoded.Length);

        // Writing byte[] as base64 is just for readibility, you can choose any technique to write the byte[]
        // Context.Writer.Write(Convert.ToBase64String(encoded));
    }

    private void WriteResourceSet(ODataResourceSetWrapper resourceSetWrapper)
    {
        int count = resourceSetWrapper.Resources.Count;
        _cborWriter.WriteStartArray(count);

        foreach (var resource in resourceSetWrapper.Resources)
        {
            WriteResource(resource);
        }

        _cborWriter.WriteEndArray();
    }

    private void WriteResource(ODataResourceWrapper resourceWrapper)
    {
        int count = resourceWrapper.Resource.Properties.Count();
        count += resourceWrapper.NestedResourceInfos.Count();

        _cborWriter.WriteStartMap(count);

        foreach (var property in resourceWrapper.Resource.Properties)
        {
            _cborWriter.WriteTextString(property.Name); // key

            WriteValue(property.Value);
        }

        foreach (var property in resourceWrapper.NestedResourceInfos)
        {
            _cborWriter.WriteTextString(property.NestedResourceInfo.Name); // key

            WriteNestedProperty(property);
        }

        _cborWriter.WriteEndMap();
    }

    private void WriteNestedProperty(ODataNestedResourceInfoWrapper nestedResourceInfoWrapper)
    {
        foreach (ODataItemWrapper childItem in nestedResourceInfoWrapper.NestedItems)
        {
            ODataResourceSetWrapper resourceSetWrapper = childItem as ODataResourceSetWrapper;
            if (resourceSetWrapper != null)
            {
                WriteResourceSet(resourceSetWrapper);
                continue;
            }

            ODataResourceWrapper resourceWrapper = childItem as ODataResourceWrapper;
            if (resourceWrapper != null)
            {
                WriteResource(resourceWrapper);
            }
        }
    }

    protected void WriteValue(object value)
    {
        Type valueType = value.GetType();
        valueType = Nullable.GetUnderlyingType(valueType) ?? valueType;

        if (value is ODataEnumValue enumValue)
        {
            _cborWriter.WriteTextString(enumValue.Value);
        }
        else if (value is ODataCollectionValue collectionValue)
        {
            _cborWriter.WriteStartArray(collectionValue.Items.Count());
            foreach (var item in collectionValue.Items)
            {
                WriteValue(item);
            }
            _cborWriter.WriteEndArray();
        }
        else if (valueType == typeof(int))
        {
            _cborWriter.WriteInt32((int)value);
        }
        else if (valueType == typeof(bool))
        {
            _cborWriter.WriteBoolean((bool)value);
        }
        else if (valueType == typeof(string))
        {
            _cborWriter.WriteTextString(value.ToString());
        }
        else
        {
            throw new NotImplementedException("I don't have time to implement all. You can add more if you need more.");
        }
    }
}
