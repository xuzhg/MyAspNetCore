//---------------------------------------------------------------------
// <copyright file="CustomizedInputContext.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//---------------------------------------------------------------------

using Microsoft.OData.Edm;
using Microsoft.OData;
using ODataCustomizePayloadFormat.Extensions.Cbor;
using ODataCustomizePayloadFormat.Extensions.Yaml;
using ODataCustomizePayloadFormat.Extensions.Csv;
using System.Text;

namespace ODataCustomizePayloadFormat.Extensions;

public class CustomizedInputContext : ODataInputContext
{
    private ODataMediaType _mediaType;
    private ODataMessageInfo _messageInfo;

    public CustomizedInputContext(ODataFormat format, ODataMessageReaderSettings settings, ODataMessageInfo messageInfo)
        : base(format, messageInfo, settings)
    {
        MessageStream = messageInfo.MessageStream;
        _mediaType = messageInfo.MediaType;
        _messageInfo = messageInfo;
    }

    public Stream MessageStream { get; private set; }

    public override Task<ODataReader> CreateResourceSetReaderAsync(IEdmEntitySetBase entitySet, IEdmStructuredType resourceType)
    {
        ODataReader reader = CreateReader(resourceType);
        return Task.FromResult<ODataReader>(reader);
    }

    public override Task<ODataReader> CreateResourceReaderAsync(IEdmNavigationSource navigationSource, IEdmStructuredType resourceType)
    {
        ODataReader reader = CreateReader(resourceType);
        return Task.FromResult<ODataReader>(reader);
    }

    private ODataReader CreateReader(IEdmStructuredType resourceType)
    {
        TextReader textReader = CreateReader(_messageInfo.MessageStream, _messageInfo.Encoding);

        if (_mediaType.Type == "text" && _mediaType.SubType == "csv")
        {
            return new CsvODataReader(textReader, resourceType);
        }

        if (_mediaType.Type == "application" && _mediaType.SubType == "yaml")
        {
            return new YamlODataReader(textReader, resourceType);
        }

        throw new InvalidOperationException($"Not valid '{_mediaType.Type}/{_mediaType.SubType}' for this output context.");
    }

    private static TextReader CreateReader(Stream messageStream, Encoding encoding)
    {
        return new StreamReader(messageStream, encoding);
    }
}
