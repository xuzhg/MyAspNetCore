//---------------------------------------------------------------------
// <copyright file="CsvOutputContext.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//---------------------------------------------------------------------

using Microsoft.OData.Edm;
using Microsoft.OData;

namespace ODataCustomizePayloadFormat.Extensions.Csv;

public class CsvOutputContext : ODataOutputContext
{
    private Stream stream;

    public CsvOutputContext(ODataFormat format, ODataMessageWriterSettings settings, ODataMessageInfo messageInfo)
        : base(format, messageInfo, settings)
    {
        stream = messageInfo.MessageStream;
        Writer = new StreamWriter(stream);
    }

    public TextWriter Writer { get; private set; }

    public override Task<ODataWriter> CreateODataResourceSetWriterAsync(IEdmEntitySetBase entitySet, IEdmStructuredType resourceType)
       => Task.FromResult<ODataWriter>(new CsvWriter(this, resourceType));

    public override Task<ODataWriter> CreateODataResourceWriterAsync(IEdmNavigationSource navigationSource, IEdmStructuredType resourceType)
       => Task.FromResult<ODataWriter>(new CsvWriter(this, resourceType));

    public void Flush() => stream.Flush();

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            try
            {
                if (Writer != null)
                {
                    Writer.Dispose();
                }

                if (stream != null)
                {
                    stream.Dispose();
                }
            }
            finally
            {
                Writer = null;
                stream = null;
            }
        }

        base.Dispose(disposing);
    }
}
