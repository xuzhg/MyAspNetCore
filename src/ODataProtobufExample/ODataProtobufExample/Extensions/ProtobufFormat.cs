//---------------------------------------------------------------------
// <copyright file="ProtobufFormat.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//---------------------------------------------------------------------

using Microsoft.OData;

namespace ODataProtobufExample.Protobuf;

public class ProtobufFormat : ODataFormat
{
    public override Task<ODataOutputContext> CreateOutputContextAsync(
        ODataMessageInfo messageInfo, ODataMessageWriterSettings messageWriterSettings)
    {
        return Task.FromResult<ODataOutputContext>(
            new ProtobufOutputContext(this, messageWriterSettings, messageInfo));
    }

    public override ODataInputContext CreateInputContext(
        ODataMessageInfo messageInfo, ODataMessageReaderSettings messageReaderSettings)
        => throw new NotImplementedException();

    public override Task<ODataInputContext> CreateInputContextAsync(
        ODataMessageInfo messageInfo, ODataMessageReaderSettings messageReaderSettings)
        => Task.FromResult<ODataInputContext>(
            new ProtobufInputContext(this, messageReaderSettings, messageInfo));

    public override ODataOutputContext CreateOutputContext(
        ODataMessageInfo messageInfo, ODataMessageWriterSettings messageWriterSettings)
        => throw new NotImplementedException();

    public override IEnumerable<ODataPayloadKind> DetectPayloadKind(
        ODataMessageInfo messageInfo, ODataMessageReaderSettings settings)
        => throw new NotImplementedException();

    public override Task<IEnumerable<ODataPayloadKind>> DetectPayloadKindAsync(
        ODataMessageInfo messageInfo, ODataMessageReaderSettings settings)
        => throw new NotImplementedException();
}
