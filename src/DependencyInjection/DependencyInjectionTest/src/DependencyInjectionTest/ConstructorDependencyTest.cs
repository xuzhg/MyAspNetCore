using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace DependencyInjectionTest
{
    public class ConstructorDependencyTest
    {
        private readonly ITestOutputHelper output;

        public ConstructorDependencyTest(ITestOutputHelper op)
        {
            output = op;
        }

        [Fact]
        public void ConstructorDependencyTest_Failed()
        {
            IServiceCollection services = new ServiceCollection();
     //       services.AddSingleton(typeof(IMyInterface), typeof(MyClass));
            services.AddScoped<ConstructorDependency>();

            IServiceProvider provider = services.BuildServiceProvider();

            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => provider.GetRequiredService<ConstructorDependency>());
            // output.WriteLine(exception.Message);
            Assert.Equal("Unable to resolve service for type 'DependencyInjectionTest.IMyInterface' while attempting to activate 'DependencyInjectionTest.ConstructorDependency'.", exception.Message);
        }

        [Fact]
        public void ConstructorDependencyTest_Successed()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton(typeof(IMyInterface), typeof(MyClass));
            services.AddScoped<ConstructorDependency>();

            IServiceProvider provider = services.BuildServiceProvider();

            ConstructorDependency c = provider.GetRequiredService<ConstructorDependency>();

            Assert.NotNull(c);
        }

        [Fact]
        public void ConstructorDependencyTest_Failed_ForInt()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton(typeof(IMyInterface), typeof(MyClass));
            services.AddScoped<ConstructorDependency2>();

            IServiceProvider provider = services.BuildServiceProvider();

            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => provider.GetRequiredService<ConstructorDependency2>());

            Assert.Equal("Unable to resolve service for type 'System.Int32' while attempting to activate 'DependencyInjectionTest.ConstructorDependency2'.", exception.Message);
        }

        [Fact]
        public void ConstructorDependencyTest_Successed_Multiple()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton(typeof (IMyInterface), typeof (MyClass));
            services.AddSingleton(typeof (IOtherInteface), typeof (OtherClass));
            services.AddScoped<ConstructorDependency3>();

            IServiceProvider provider = services.BuildServiceProvider();

            // here it will calll the multiple parameter construstor, dig deep?
            ConstructorDependency3 c = provider.GetRequiredService<ConstructorDependency3>();

            Assert.NotNull(c);
        }
    }

    public interface IMyInterface
    {
        
    }

    public class MyClass : IMyInterface
    {
        public MyClass()
        {
            Console.WriteLine("In MyClass Constructor!");
        }
    }

    public class ConstructorDependency
    {
        private IMyInterface _my;

        public ConstructorDependency(IMyInterface my)
        {
            _my = my;

            Console.WriteLine("In ConstructorDependency Constructor!");
        }
    }

    public class ConstructorDependency2
    {
        private IMyInterface _my;

        public ConstructorDependency2(IMyInterface my, int otherData)
        {
            _my = my;

            Console.WriteLine("In ConstructorDependency2 Constructor!");
        }
    }

    public interface IOtherInteface
    { }

    public class OtherClass : IOtherInteface
    {
        public OtherClass()
        {
            Console.WriteLine("In OtherClass Constructor!");
        }
    }


    public class ConstructorDependency3
    {
        private IMyInterface _my;

       
        public ConstructorDependency3(IMyInterface my, IOtherInteface otherData/*, int a*/)
        {
            _my = my;

            Console.WriteLine("In ConstructorDependency3 two interfaces Constructor!");
        }

        public ConstructorDependency3(IMyInterface my)
        {
            _my = my;

            Console.WriteLine("In ConstructorDependency3 Constructor!");
        }
    }
}
