//---------------------------------------------------------------------
// <copyright file="ProtobufMediaTypeResolver.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//---------------------------------------------------------------------

using Microsoft.OData;

namespace ODataProtobufExample.Protobuf;

/// <summary>
/// Keep this class for the post reference.
/// </summary>
public class ProtobufMediaTypeResolver : ODataMediaTypeResolver
{
    private readonly ODataMediaTypeFormat[] _mediaTypeFormats =
    {
        new ODataMediaTypeFormat(new ODataMediaType("application", "x-protobuf"), new ProtobufFormat()),
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
