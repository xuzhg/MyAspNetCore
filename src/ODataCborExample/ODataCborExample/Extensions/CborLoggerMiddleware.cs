using System.Diagnostics;

namespace ODataCborExample.Extensions
{
    public class CborLoggerMiddleware
    {
        private RequestDelegate _next;

        public CborLoggerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IMessageWriter writer)
        {
            var accept = context.Request.Headers.Accept.First().Split(';').First();
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();

            await _next(context);

            stopwatch.Stop();
            writer.Write($"{accept} spends {stopwatch.ElapsedMilliseconds}ms.");
        }
    }

    public static class MyCborLoggerExtensions
    {
        public static IApplicationBuilder UseMyCborLogger(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CborLoggerMiddleware>();
        }
    }
}