//---------------------------------------------------------------------
// <copyright file="CustomizedFormat.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//---------------------------------------------------------------------

using Microsoft.OData;
using ODataCustomizePayloadFormat.Extensions.Csv;

namespace ODataCustomizePayloadFormat.Extensions;

public class CustomizedMediaTypeResolver : ODataMediaTypeResolver
{
    private static CustomizedFormat _customizedFormat = new CustomizedFormat();

    private readonly ODataMediaTypeFormat[] _mediaTypeFormats =
    {
        new ODataMediaTypeFormat(new ODataMediaType("text", "csv"), new CsvFormat()),
        new ODataMediaTypeFormat(new ODataMediaType("application", "yaml"), _customizedFormat),
        new ODataMediaTypeFormat(new ODataMediaType("application", "cbor"), _customizedFormat)
    };

    public override IEnumerable<ODataMediaTypeFormat> GetMediaTypeFormats(ODataPayloadKind payloadKind)
    {
        if (payloadKind == ODataPayloadKind.Resource || payloadKind == ODataPayloadKind.ResourceSet)
        {
            return _mediaTypeFormats.Concat(base.GetMediaTypeFormats(payloadKind));
        }

        return base.GetMediaTypeFormats(payloadKind);
    }
}
