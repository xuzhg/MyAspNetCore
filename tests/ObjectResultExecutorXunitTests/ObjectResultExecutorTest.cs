using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace ObjectResultExecutorXunitTests
{
    public class CustomersContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("MyAggregationContextCore");
            base.OnConfiguring(optionsBuilder);
        }
    }

    public class ObjectResultExecutorTest
    {
        [Fact]
        public async Task ExecuteAsync_UsesSpecifiedContentType()
        {
            // Arrange
            var executor = CreateExecutor();

            var httpContext = new DefaultHttpContext();
            var actionContext = new ActionContext() { HttpContext = httpContext };
            httpContext.Request.Headers[HeaderNames.Accept] = "application/json"; // This will not be used
            httpContext.Response.ContentType = "application/json";

            IList<Customer> customers = new List<Customer>();
            IQueryable<Customer> customersQuery = customers.AsQueryable<Customer>();

            CustomersContext db = new CustomersContext();
            // db.Customers

            Expression expression = Expression.Constant("abc");
            EntityQueryable<Customer> value = new EntityQueryable<Customer>(db.GetDependencies().QueryProvider, expression);

            var result = new ObjectResult(value)
            {
                ContentTypes = { "application/json", },
            };
            result.Formatters.Add(new TestJsonOutputFormatter());

            // Act
            await executor.ExecuteAsync(actionContext, result);

            // Assert
           // MediaTypeAssert.Equal("text/xml; charset=utf-8", httpContext.Response.ContentType);
        }

        private static IServiceCollection CreateServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<ILoggerFactory>(NullLoggerFactory.Instance);

            return services;
        }

        private static HttpContext GetHttpContext()
        {
            var services = CreateServices();

            var httpContext = new DefaultHttpContext();
            httpContext.RequestServices = services.BuildServiceProvider();

            return httpContext;
        }

        private static ObjectResultExecutor CreateExecutor(MvcOptions options = null)
        {
            options ??= new MvcOptions();
            var optionsAccessor = Options.Create(options);
            var selector = new DefaultOutputFormatterSelector(optionsAccessor, NullLoggerFactory.Instance);
            return new ObjectResultExecutor(selector, new TestHttpResponseStreamWriterFactory(), NullLoggerFactory.Instance, optionsAccessor);
        }

        private class TestJsonOutputFormatter : TextOutputFormatter
        {
            public TestJsonOutputFormatter()
            {
                SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
                SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/json"));
                SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/*+json"));

                SupportedEncodings.Add(Encoding.UTF8);
            }

            public OutputFormatterWriteContext LastOutputFormatterContext { get; private set; }

            public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
            {
                LastOutputFormatterContext = context;
                return Task.FromResult(0);
            }
        }
    }

    public class TestHttpResponseStreamWriterFactory : IHttpResponseStreamWriterFactory
    {
        public const int DefaultBufferSize = 16 * 1024;

        public TextWriter CreateWriter(Stream stream, Encoding encoding)
        {
            return new HttpResponseStreamWriter(stream, encoding, DefaultBufferSize);
        }
    }
}
