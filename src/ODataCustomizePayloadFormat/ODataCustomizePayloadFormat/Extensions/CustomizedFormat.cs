//---------------------------------------------------------------------
// <copyright file="CustomizedFormat.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//---------------------------------------------------------------------

using Microsoft.OData;

namespace ODataCustomizePayloadFormat.Extensions;

public class CustomizedFormat : ODataFormat
{
    public override Task<ODataOutputContext> CreateOutputContextAsync(
        ODataMessageInfo messageInfo, ODataMessageWriterSettings messageWriterSettings)
    {
        return Task.FromResult<ODataOutputContext>(
            new CustomizedOutputContext(this, messageWriterSettings, messageInfo));
    }

    public override Task<ODataInputContext> CreateInputContextAsync(
    ODataMessageInfo messageInfo, ODataMessageReaderSettings messageReaderSettings)
    {
        return Task.FromResult<ODataInputContext>(
            new CustomizedInputContext(this, messageReaderSettings, messageInfo));
    }

    #region Synchronization not used
    public override ODataInputContext CreateInputContext(
        ODataMessageInfo messageInfo, ODataMessageReaderSettings messageReaderSettings)
        => throw new NotImplementedException();

    public override ODataOutputContext CreateOutputContext(
        ODataMessageInfo messageInfo, ODataMessageWriterSettings messageWriterSettings)
        => throw new NotImplementedException();

    public override IEnumerable<ODataPayloadKind> DetectPayloadKind(
        ODataMessageInfo messageInfo, ODataMessageReaderSettings settings)
        => throw new NotImplementedException();

    public override Task<IEnumerable<ODataPayloadKind>> DetectPayloadKindAsync(
        ODataMessageInfo messageInfo, ODataMessageReaderSettings settings)
        => throw new NotImplementedException();
    #endregion
}
