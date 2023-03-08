//---------------------------------------------------------------------
// <copyright file="CsvMediaTypeResolver.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//---------------------------------------------------------------------

using Microsoft.OData;

namespace ODataCustomizePayloadFormat.Extensions.Csv;

/// <summary>
/// Keep this class for the post reference.
/// Please refer to the <see cref="CustomizedMediaTypeResolver"/> for all type resolver.
/// </summary>
public class CsvMediaTypeResolver : ODataMediaTypeResolver
{
    private static readonly CsvMediaTypeResolver _instance = new CsvMediaTypeResolver();

    private readonly ODataMediaTypeFormat[] _mediaTypeFormats =
    {
        new ODataMediaTypeFormat(new ODataMediaType("text", "csv"), new CsvFormat()),
    };

    public static CsvMediaTypeResolver Instance => _instance;

    public override IEnumerable<ODataMediaTypeFormat> GetMediaTypeFormats(ODataPayloadKind payloadKind)
    {
        if (payloadKind == ODataPayloadKind.Resource || payloadKind == ODataPayloadKind.ResourceSet)
        {
            return _mediaTypeFormats.Concat(base.GetMediaTypeFormats(payloadKind));
        }

        return base.GetMediaTypeFormats(payloadKind);
    }
}
