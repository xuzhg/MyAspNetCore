using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.Extensions.Primitives;
using Microsoft.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;
using System.Reflection.PortableExecutable;

namespace OmitNullPropertySample.Extensions
{
    public static class RequestExtensions
    {
        public static bool IsOmitNulls(this HttpRequest request)
        {
            // for simplicity, we check the prefer header
            string preferHeader = null;
            StringValues values;
            if (request.Headers.TryGetValue("Prefer", out values))
            {
                // If there are many "Prefer" headers, pick up the first one.
                preferHeader = values.FirstOrDefault();
            }

            if (preferHeader == null)
            {
                return false;
            }

            // use case insensitive string comparison
            if (preferHeader.Contains("omit-values=nulls", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        public static void SetPreferenceAppliedResponseHeader(this HttpRequest httpRequest)
        {
            HttpResponse response = httpRequest.HttpContext.Response;

            string prefer_applied = null;
            if (response.Headers.TryGetValue("Preference-Applied", out StringValues values))
            {
                // If there are many "Preference-Applied" headers, pick up the first one.
                prefer_applied = values.FirstOrDefault();
            }

            if (prefer_applied == null)
            {
                response.Headers["Preference-Applied"] = "omit-values=nulls";
            }
            else
            {
                response.Headers["Preference-Applied"] = $"{prefer_applied},omit-values=nulls";
            }
        }
    }
}
