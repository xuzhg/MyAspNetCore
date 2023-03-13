//-----------------------------------------------------------------------------
// <copyright file="CborMediaTypeResolver.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.OData;

namespace ODataCborExample.Extensions
{
    public class CborMediaTypeResolver : ODataMediaTypeResolver
    {
        private readonly ODataMediaTypeFormat[] _mediaTypeFormats =
        {
            new ODataMediaTypeFormat(new ODataMediaType("application", "cbor"), ODataFormat.Json)
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

}
