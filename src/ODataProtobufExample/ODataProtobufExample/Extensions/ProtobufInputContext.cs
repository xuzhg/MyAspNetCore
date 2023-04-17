//---------------------------------------------------------------------
// <copyright file="ProtobufInputContext.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//---------------------------------------------------------------------

using Microsoft.OData.Edm;
using Microsoft.OData;

namespace ODataProtobufExample.Protobuf;

public class ProtobufInputContext : ODataInputContext
{
    public ProtobufInputContext(ODataFormat format,
        ODataMessageReaderSettings settings, ODataMessageInfo messageInfo)
        : base(format, messageInfo, settings)
    {
        MessageStream = messageInfo.MessageStream;
    }

    public Stream MessageStream { get; private set; }

    public override Task<ODataReader> CreateResourceSetReaderAsync(IEdmEntitySetBase entitySet, IEdmStructuredType resourceType)
       => Task.FromResult<ODataReader>(new ProtobufReader(this, resourceType));

    public override Task<ODataReader> CreateResourceReaderAsync(IEdmNavigationSource navigationSource, IEdmStructuredType resourceType)
       => Task.FromResult<ODataReader>(new ProtobufReader(this, resourceType));
}
