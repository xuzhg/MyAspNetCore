//---------------------------------------------------------------------
// <copyright file="YamlODataReader.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//---------------------------------------------------------------------

using Microsoft.OData;
using Microsoft.OData.Edm;
using ODataCustomizePayloadFormat.Extensions.Csv;

namespace ODataCustomizePayloadFormat.Extensions.Yaml;

public class YamlODataReader : ODataReader
{
    private ODataReaderState _state;
    private ODataItem _item;
    private IEdmStructuredType _structuredType;
    private TextReader _textReader;

    public YamlODataReader(TextReader txtReader, IEdmStructuredType structuredType)
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
        // for simplicity
        IList<ODataProperty> odataProperties = new List<ODataProperty>();

        string line = _textReader.ReadLine();
        while (line != null)
        {
            string[] properties = line.Split(':');

            string propertyName = properties[0].Trim();
            string propertyValue = properties[1].Trim();

            IEdmProperty edmProperty = _structuredType.FindProperty(propertyName);

            ODataProperty property = new ODataProperty
            {
                Name = propertyName,
                Value = CsvODataReader.ConvertPropertyValue(edmProperty, propertyValue)
            };

            odataProperties.Add(property);

            line = _textReader.ReadLine();
        }

        _item = new ODataResource
        {
            TypeName = _structuredType.FullTypeName(),
            Properties = odataProperties
        };

        _state = ODataReaderState.ResourceStart;
        return true;
    }
}
