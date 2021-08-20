using System;
using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.OData.Edm;

namespace ODataETagWebApi.Extensions
{
    public class ETagSerializerProvider : ODataSerializerProvider
    {
        public ETagSerializerProvider(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public override IODataEdmTypeSerializer GetEdmTypeSerializer(IEdmTypeReference edmType)
        {
            if (edmType.IsEntity() || edmType.IsComplex())
            {
                return new ETagResourceSerializer(this);
            }

            return base.GetEdmTypeSerializer(edmType);
        }
    }
}
