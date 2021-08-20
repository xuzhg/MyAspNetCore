using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Formatter.Deserialization;
using Microsoft.AspNetCore.OData.Formatter.Wrapper;
using Microsoft.OData;
using Microsoft.OData.Edm;
using ODataETagWebApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ODataETagWebApi.Extensions
{
    public class ETagResourceDeserializer : ODataResourceDeserializer
    {
        public ETagResourceDeserializer(IODataDeserializerProvider serializerProvider)
            : base(serializerProvider)
        { }

        public override object ReadResource(ODataResourceWrapper resourceWrapper, IEdmStructuredTypeReference structuredType, ODataDeserializerContext readContext)
        {
            object resource = base.ReadResource(resourceWrapper, structuredType, readContext);

            if (resourceWrapper.Resource.ETag != null)
            {
                Guid label = DecodeETag(resourceWrapper.Resource.ETag);
                if (resource is Customer c)
                {
                    c.Label = label;
                }
                else if (resource is IDelta delta)
                {
                    Type deltaType = typeof(Delta<>).MakeGenericType(typeof(Customer));
                    var fieldInfo = deltaType.GetField("_updatableProperties", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                    var updateProperties = fieldInfo.GetValue(delta) as HashSet<string>;
                    updateProperties.Add("Label");
                    delta.TrySetPropertyValue("Label", label);
                }
            }

            return resource;
        }

        private static Guid DecodeETag(string etag)
        {
            byte[] base64 = Convert.FromBase64String(etag);
            string base64Str = Encoding.UTF8.GetString(base64);
            return new Guid(base64Str);
        }
    }
}
