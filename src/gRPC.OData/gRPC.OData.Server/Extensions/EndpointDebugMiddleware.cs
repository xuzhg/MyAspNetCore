using System.Reflection;
using System.Text;

namespace gRPC.OData.Server.Extensions
{
    public static class ApplicationBuilderEndpointDebugExtensions
    {
        public static IApplicationBuilder UseEndpointDebug(this IApplicationBuilder app)
        {
            return app.UseMiddleware<EndpointDebugMiddleware>();
        }
    }

    public class EndpointDebugMiddleware
    {
        private RequestDelegate _next;
        public EndpointDebugMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            HttpRequest request = context.Request;
            if (string.Equals(request.Path.Value, "/$endpoint", StringComparison.OrdinalIgnoreCase))
            {
                await WriteRoutesAsHtml(context).ConfigureAwait(false);
            }
            else
            {
                await _next(context).ConfigureAwait(false);
            }
        }

        private static IReadOnlyList<string> EmptyHeaders = Array.Empty<string>();

        internal static async Task WriteRoutesAsHtml(HttpContext context)
        {
            var stdRouteTable = new StringBuilder();

            var dataSource = context.RequestServices.GetRequiredService<EndpointDataSource>();
            foreach (var routeEndpiont in dataSource.Endpoints.OfType<RouteEndpoint>())
            {
                stdRouteTable.Append("<tr>");
                stdRouteTable.Append($"<td>{routeEndpiont.DisplayName}</td>");
                stdRouteTable.Append($"<td>{routeEndpiont.RoutePattern.RawText}</td>");

                var httpMethods = routeEndpiont.Metadata.GetMetadata<HttpMethodMetadata>()?.HttpMethods ?? EmptyHeaders;

                stdRouteTable.Append($"<td>{string.Join(",", httpMethods)}</td>");


                stdRouteTable.Append($"<td>{GetRequestDelegateString(routeEndpiont.RequestDelegate)}</td>");

                stdRouteTable.AppendLine("</tr>");
            }

            string output = RouteMappingHtmlTemplate;
            output = output.Replace("STD_ROUTE_CONTENT", stdRouteTable.ToString(), StringComparison.Ordinal);

            context.Response.ContentType = "text/html";
            await context.Response.WriteAsync(output).ConfigureAwait(false);
        }
        private static string GetRequestDelegateString(RequestDelegate rd)
        {
            StringBuilder sb = new StringBuilder();

            MethodInfo methodInfo = rd.Method;
            sb.Append(methodInfo.ReturnType?.Name);
            sb.Append(" ");
            sb.Append(methodInfo.DeclaringType?.Namespace + "." + methodInfo.DeclaringType?.Name);
            sb.Append(".");
            sb.Append(methodInfo.Name);
            sb.Append("(");
            int index = 0;
            foreach (var p in methodInfo.GetParameters())
            {
                if (index == 0)
                {
                    index = 1;
                }
                else
                {
                    sb.Append(",");
                }
                sb.Append(p.ParameterType.Name);

            }
            sb.Append(")");
            return sb.ToString();
        }

        private static string RouteMappingHtmlTemplate = @"<html>
<head>
    <title>Endpoint Routing Debugger</title>
    <style>
        table {
            font-family: arial, sans-serif;
            border-collapse: collapse;
            width: 100%;
        }
        td,
        th {
            border: 1px solid #dddddd;
            text-align: left;
            padding: 8px;
        }
        tr:nth-child(even) {
            background-color: #dddddd;
        }
    </style>
</head>
<body>
    <h1 id=""odata"">Endpoint Mappings</h1>
    <table>
        <tr>
            <th> Display Name </th>
            <th> RoutePattern </th>
            <th> HttpMethods </th>
            <th> RequestDelegate </th>
        </tr>
        STD_ROUTE_CONTENT
    </table>
</body>
</html>";
    }
}
