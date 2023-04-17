//---------------------------------------------------------------------
// <copyright file="ProtobufOutputContext.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//---------------------------------------------------------------------

using Microsoft.OData.Edm;
using Microsoft.OData;

namespace ODataProtobufExample.Protobuf;

public class ProtobufOutputContext : ODataOutputContext
{
    public ProtobufOutputContext(ODataFormat format, ODataMessageWriterSettings settings, ODataMessageInfo messageInfo)
        : base(format, messageInfo, settings)
    {
        MessageStream = messageInfo.MessageStream;
        Writer = new StreamWriter(MessageStream);
    }

    public Stream MessageStream { get; private set; }

    public TextWriter Writer { get; private set; }

    public override Task<ODataWriter> CreateODataResourceSetWriterAsync(IEdmEntitySetBase entitySet, IEdmStructuredType resourceType)
       => Task.FromResult<ODataWriter>(new ProtobufWriter(this, resourceType));

    public override Task<ODataWriter> CreateODataResourceWriterAsync(IEdmNavigationSource navigationSource, IEdmStructuredType resourceType)
       => Task.FromResult<ODataWriter>(new ProtobufWriter(this, resourceType));

    public void Flush() => MessageStream.Flush();

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

                if (MessageStream != null)
                {
                    MessageStream.Dispose();
                }
            }
            finally
            {
                Writer = null;
                MessageStream = null;
            }
        }

        base.Dispose(disposing);
    }
}
