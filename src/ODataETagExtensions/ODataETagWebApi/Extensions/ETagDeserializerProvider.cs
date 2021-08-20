using System;
using Microsoft.AspNetCore.OData.Formatter.Deserialization;
using Microsoft.OData.Edm;

namespace ODataETagWebApi.Extensions
{
    public class ETagDeserializerProvider : ODataDeserializerProvider
    {
        public ETagDeserializerProvider(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public override IODataEdmTypeDeserializer GetEdmTypeDeserializer(IEdmTypeReference edmType, bool isDelta = false)
        {
            if (edmType.IsEntity() || edmType.IsComplex())
            {
                return new ETagResourceDeserializer(this);
            }

            return base.GetEdmTypeDeserializer(edmType, isDelta);
        }
    }
}
