//---------------------------------------------------------------------
// <copyright file="CustomizedOutputContext.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//---------------------------------------------------------------------

using Microsoft.OData.Edm;
using Microsoft.OData;
using ODataCustomizePayloadFormat.Extensions.Cbor;
using ODataCustomizePayloadFormat.Extensions.Yaml;

namespace ODataCustomizePayloadFormat.Extensions;

public class CustomizedOutputContext : ODataOutputContext
{
    private Stream _stream;
    private ODataMediaType _mediaType;

    public CustomizedOutputContext(ODataFormat format, ODataMessageWriterSettings settings, ODataMessageInfo messageInfo)
        : base(format, messageInfo, settings)
    {
        _stream = messageInfo.MessageStream;
        _mediaType = messageInfo.MediaType;
        Writer = new StreamWriter(_stream);
    }

    public Stream Stream => _stream;

    public TextWriter Writer { get; }

    public override Task<ODataWriter> CreateODataResourceSetWriterAsync(IEdmEntitySetBase entitySet, IEdmStructuredType resourceType)
    {
        ODataWriter writer = CreateWriter(resourceType);

        return Task.FromResult(writer);
    }

    public override Task<ODataWriter> CreateODataResourceWriterAsync(IEdmNavigationSource navigationSource, IEdmStructuredType resourceType)
    {
        ODataWriter writer = CreateWriter(resourceType);
        return Task.FromResult(writer);
    }

    public void Flush()
    {
        _stream.Flush();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            try
            {
                if (this.Writer != null)
                {
                    this.Writer.Dispose();
                }

                if (_stream != null)
                {
                    _stream.Dispose();
                }
            }
            finally
            {
                _stream = null;
                _stream = null;
            }
        }

        base.Dispose(disposing);
    }

    private ODataWriter CreateWriter(IEdmStructuredType resourceType)
    {
        if (_mediaType.Type == "text" && _mediaType.SubType == "csv")
        {
           // return new CsvWriter(this, resourceType);
           // Keep Csv separated for clear post
        }

        if (_mediaType.Type == "application" && _mediaType.SubType == "yaml")
        {
            return new YamlODataWriter(this, resourceType);
        }

        if (_mediaType.Type == "application" && _mediaType.SubType == "cbor")
        {
            return new CborODataWriter(this, resourceType);
        }

        throw new InvalidOperationException($"Not valid '{_mediaType.Type}/{_mediaType.SubType}' for this output context.");
    }
}
