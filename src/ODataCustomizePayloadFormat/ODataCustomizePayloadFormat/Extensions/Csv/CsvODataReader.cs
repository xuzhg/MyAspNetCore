//---------------------------------------------------------------------
// <copyright file="CsvODataReader.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//---------------------------------------------------------------------

using Microsoft.OData;
using Microsoft.OData.Edm;


namespace ODataCustomizePayloadFormat.Extensions.Csv;

public class CsvODataReader : ODataReader
{
    private ODataReaderState _state;
    private ODataItem _item;
    private IEdmStructuredType _structuredType;
    private TextReader _textReader;

    public CsvODataReader(TextReader txtReader, IEdmStructuredType structuredType)
    {
        _structuredType = structuredType;
        _state = ODataReaderState.Start;
        _item = null;
        _textReader = txtReader;
    }

    public override ODataItem Item => _item;

    public override ODataReaderState State => _state;

    public override bool Read()
    {
        // for simplicity
        bool result;
        switch (_state)
        {
            case ODataReaderState.Start:
                result = ReadAtStart();
                break;

            case ODataReaderState.ResourceStart:
                _state = ODataReaderState.ResourceEnd;
                result = true;
                break;

            case ODataReaderState.ResourceEnd:
                _state = ODataReaderState.Completed;
                result = false;
                break;

            default:
                throw new NotImplementedException("DIY!");
        }

        return result;
    }

    public override Task<bool> ReadAsync()
    {
        bool ret = Read();
        return Task.FromResult(ret);
    }

    private bool ReadAtStart()
    {
        // for simplicity, first line is the property name
        // second line is the property value
        // and only two line
        string firstLine = _textReader.ReadLine();
        string secondLine = _textReader.ReadLine();

        IList<ODataProperty> odataProperties = new List<ODataProperty>();

        string[] properties = firstLine.Split(',');
        string[] propertiesValue = secondLine.Split(",");
        for (int i = 0; i < properties.Length; ++i)
        {
            string propertyName = properties[i];
            string propertyValue = propertiesValue[i];

            IEdmProperty edmProperty = _structuredType.FindProperty(propertyName);

            ODataProperty property = new ODataProperty
            {
                Name = propertyName,
                Value = ConvertPropertyValue(edmProperty, propertyValue)
            };

            odataProperties.Add(property);
        }

        _item = new ODataResource
        {
            TypeName = _structuredType.FullTypeName(),
            Properties = odataProperties
        };

        _state = ODataReaderState.ResourceStart;
        return true;
    }


    internal static object ConvertPropertyValue(IEdmProperty edmProperty, string propertyValue)
    {
        switch (edmProperty.Type.TypeKind())
        {
            case EdmTypeKind.Primitive:
                IEdmPrimitiveTypeReference primitiveTypeRef = (IEdmPrimitiveTypeReference)edmProperty.Type;
                switch (primitiveTypeRef.PrimitiveKind())
                {
                    case EdmPrimitiveTypeKind.String:
                        return propertyValue;

                    case EdmPrimitiveTypeKind.Int32:
                        return int.Parse(propertyValue);

                    default:
                        throw new NotImplementedException("IEdmPrimitiveTypeReference DIY!");
                }

            default:
                throw new NotImplementedException("edmProperty.Type DIY!");
        }
    }
}
