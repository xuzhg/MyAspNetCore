using Microsoft.Extensions.DependencyInjection;
using MyLearn.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MyLearn
{
    public class ServiceDescriptorTests
    {
        [Fact]
        public void TestConstructor()
        {
            var serviceDescriptor = new ServiceDescriptor(typeof(IFoo), typeof(Foo), ServiceLifetime.Scoped);

            Assert.NotNull(serviceDescriptor.ServiceType);
        }
    }
}
