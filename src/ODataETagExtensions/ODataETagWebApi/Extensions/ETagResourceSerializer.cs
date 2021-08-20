using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.AspNetCore.OData.Formatter.Value;
using Microsoft.OData;
using ODataETagWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODataETagWebApi.Extensions
{
    public class ETagResourceSerializer : ODataResourceSerializer
    {
        public ETagResourceSerializer(IODataSerializerProvider serializerProvider)
            : base(serializerProvider)
        { }

        public override ODataResource CreateResource(SelectExpandNode selectExpandNode, ResourceContext resourceContext)
        {
            ODataResource resource = base.CreateResource(selectExpandNode, resourceContext);

            if (resource.ETag == null && resourceContext.ResourceInstance is Customer c)
            {
                resource.ETag = EncodeETag(c.Label);
            }

            return resource;
        }

        private static string EncodeETag(Guid guid)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(guid.ToString());
            return Convert.ToBase64String(bytes);
        }
    }
}
