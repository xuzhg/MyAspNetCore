using Microsoft.Extensions.DependencyInjection;
using MyLearn.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace MyLearn
{
    public class ServiceScopeTests
    {
        private readonly ITestOutputHelper _output;

        public ServiceScopeTests(ITestOutputHelper output)
        {
            this._output = output;
        }

        [Fact]
        public void Test()
        {
            ServiceProvider serviceProvider = new ServiceCollection().BuildServiceProvider();

            // _root is in .NET Core 2.x, not in .NET Core 3.x
            FieldInfo rootField = serviceProvider.GetType().GetField("_root", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.Null(rootField);

            if (rootField != null)
            {
                object rootOfServiceProvider = rootField.GetValue(serviceProvider);
                Assert.Null(rootOfServiceProvider);
            }

            IServiceScope scope = serviceProvider.GetService<IServiceScopeFactory>().CreateScope();
            IServiceProvider serviceProvider1 = scope.ServiceProvider;

            IServiceScope scope2 = serviceProvider.CreateScope();
            Assert.False(object.ReferenceEquals(scope, scope2));

            if (rootField != null)
            {
                object root = rootField.GetValue(serviceProvider1);
                Assert.True(object.ReferenceEquals(serviceProvider, root));
            }

            Assert.False(object.ReferenceEquals(serviceProvider, serviceProvider1));
        }

        [Fact]
        public void TestServiceLife()
        {
            IServiceProvider root = new ServiceCollection()
                .AddTransient<IFoo, Foo>()
                .AddScoped<IBar, Bar>()
                .AddSingleton<IBaz, Baz>()
                .BuildServiceProvider();

            IServiceProvider child1 = root.GetService<IServiceScopeFactory>().CreateScope().ServiceProvider;
            IServiceProvider child2 = root.GetService<IServiceScopeFactory>().CreateScope().ServiceProvider;

            _output.WriteLine("ReferenceEquals(root.GetService<IFoo>(), root.GetService<IFoo>() = {0}", // false
                ReferenceEquals(root.GetService<IFoo>(), root.GetService<IFoo>()));

            _output.WriteLine("ReferenceEquals(child1.GetService<IBar>(), child1.GetService<IBar>() = {0}", // true
                ReferenceEquals(child1.GetService<IBar>(), child1.GetService<IBar>()));

            _output.WriteLine("ReferenceEquals(child1.GetService<IBar>(), child2.GetService<IBar>() = {0}", // false
                ReferenceEquals(child1.GetService<IBar>(), child2.GetService<IBar>()));

            _output.WriteLine("ReferenceEquals(child1.GetService<IBaz>(), child2.GetService<IBaz>() = {0}", // true
                ReferenceEquals(child1.GetService<IBaz>(), child2.GetService<IBaz>()));
        }

        [Fact]
        public void TestDispose()
        {
            IServiceProvider root = new ServiceCollection()
                .AddTransient<IFoo, Foo>()
                .AddScoped<IBar, Bar>()
                .AddSingleton<IBaz, Baz>()
                .BuildServiceProvider();

            IServiceProvider child1 = root.GetService<IServiceScopeFactory>().CreateScope().ServiceProvider;
            IServiceProvider child2 = root.GetService<IServiceScopeFactory>().CreateScope().ServiceProvider;

            child1.GetService<IFoo>();
            child1.GetService<IFoo>();
            child2.GetService<IBar>();
            child2.GetService<IBaz>();

            Debug.WriteLine("child1.Dispose()");
            ((IDisposable)child1).Dispose();

            Debug.WriteLine("child2.Dispose()");
            ((IDisposable)child2).Dispose();

            Debug.WriteLine("root.Dispose()");
            ((IDisposable)root).Dispose();
        }

        [Fact]
        public void TestDc()
        {
            IServiceProvider serviceProvider = new ServiceCollection()
                .AddTransient<IFoobar, Foobar>()
                .BuildServiceProvider();

            serviceProvider.GetService<IFoobar>().Dispose();
            GC.Collect();

            Debug.WriteLine("---------------");
            using(IServiceScope serviceScope = serviceProvider.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<IFoobar>();
            }

            GC.Collect();
            Console.Read();
        }
    }
}
