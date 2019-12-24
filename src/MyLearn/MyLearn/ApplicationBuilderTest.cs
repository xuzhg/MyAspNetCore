using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MyLearn
{
    public class ApplicationBuilderTest
    {
        private readonly ITestOutputHelper _output;

        public ApplicationBuilderTest(ITestOutputHelper output)
        {
            this._output = output;
        }

        [Fact]
        public void Test()
        {
            ServiceProvider serviceProvider = new ServiceCollection().BuildServiceProvider();

            ApplicationBuilder builder = new ApplicationBuilder(serviceProvider);

            var requestDelegate = builder.Build();

            Assert.NotNull(requestDelegate);

            HttpContext context = new DefaultHttpContext();
            Task task = requestDelegate.Invoke(context);
        }
    }
}
