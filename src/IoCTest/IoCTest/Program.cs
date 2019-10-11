using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Linq;

namespace IoCTest
{
    public enum Lifetime
    {
        Root, Self, Transient
    }
    public interface IFoo { }
    public interface IBar { }
    public interface IBaz { }
    public interface IFoobar<T1, T2> { }
    public class Base : IDisposable
    {
        public Base()
        {
            Console.WriteLine($"An instance of {GetType().Name} is created.");
        }
        public void Dispose()
        {
            Console.WriteLine($"The intance of {GetType().Name} is disposed.");
        }
    }
    public class Foo : Base, IFoo, IDisposable { }
    public class Bar : Base, IBar, IDisposable { }
    public class Baz : Base, IBaz, IDisposable { }
    public class FooBar<T1, T2> : IFoobar<T1, T2>
    {
        public IFoo Foo { get; }
        public IBar Bar { get; }
        public FooBar(IFoo foo, IBar bar)
        {
            Foo = foo;
            Bar = bar;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var a = typeof(Foo).GetConstructors();
            Console.WriteLine(1);

            var root = new Cat()
                .Register<IFoo, Foo>(Lifetime.Transient)
                .Register<IBar>(_ => new Bar(), Lifetime.Self)
                .Register<IBaz, Baz>(Lifetime.Root);

            var cat1 = root.CreateChild();
            var cat2 = root.CreateChild();

            void GetServices<TService>(Cat cat)
            {
                cat.GetService<TService>();
                cat.GetService<TService>();
            }

            GetServices<IFoo>(cat1);
            GetServices<IBar>(cat1);
            GetServices<IBaz>(cat1);
            Console.WriteLine();


            GetServices<IFoo>(cat2);
            GetServices<IBar>(cat2);
            GetServices<IBaz>(cat2);

            //
            Console.WriteLine(2);
            var myNewCat = new Cat()
                .Register<IFoo, Foo>(Lifetime.Transient)
                .Register<IBar, Bar>(Lifetime.Transient)
                .Register(typeof(IFoobar<,>), typeof(FooBar<,>), Lifetime.Transient);
            var foobar = (FooBar<IFoo, IBar>)myNewCat.GetService<IFoobar<IFoo, IBar>>();
            Debug.Assert(foobar.Foo is Foo);
            Debug.Assert(foobar.Bar is Bar);


            //
            Console.WriteLine(3);

            var services = new Cat().Register<Base, Foo>(Lifetime.Transient)
                .Register<Base, Bar>(Lifetime.Transient)
                .Register<Base, Baz>(Lifetime.Transient)
                .GetServices<Base>();

            Debug.Assert(services.OfType<Foo>().Any());
            Debug.Assert(services.OfType<Bar>().Any());
            Debug.Assert(services.OfType<Baz>().Any());

            Console.WriteLine(4);

            using (var anotherRoot = new Cat().Register<IFoo, Foo>(Lifetime.Transient)
                .Register<IBar, Bar>(Lifetime.Self)
                .Register<IBaz, Baz>(Lifetime.Root))
            {
                using (var cat = anotherRoot.CreateChild())
                {
                    cat.GetService<IFoo>();
                    cat.GetService<IBar>();
                    cat.GetService<IBaz>();
                }
                Console.WriteLine("Child cat is disposed.");

            }
            Console.WriteLine("Root cat is disposed.");

            Console.WriteLine(".........................Test DI..............");
            TestDI();

            TestDI_Dispose();
        }

        private static void TestDI()
        {
            var root = new ServiceCollection()
                .AddTransient<IFoo, Foo>()
                .AddScoped<IBar>(_ => new Bar())
                .AddSingleton<IBaz, Baz>()
                .BuildServiceProvider();

            var provider1 = root.CreateScope().ServiceProvider;
            var provider2 = root.CreateScope().ServiceProvider;

            void GetSerives<TService>(IServiceProvider provider)
            {
                provider.GetService<TService>();
                provider.GetService<TService>();
            }

            GetSerives<IFoo>(provider1);
            GetSerives<IBar>(provider1);
            GetSerives<IBaz>(provider1);
            Console.WriteLine();

            GetSerives<IFoo>(provider2);
            GetSerives<IBar>(provider2);
            GetSerives<IBaz>(provider2);
            Console.WriteLine();

            var provider3 = provider2.CreateScope().ServiceProvider;
            GetSerives<IFoo>(provider3);
            GetSerives<IBar>(provider3);
            GetSerives<IBaz>(provider3);
            Console.WriteLine();
        }

        private static void TestDI_Dispose()
        {
            Console.WriteLine(">>> TestDI_Dispose.");

            using (var root = new ServiceCollection()
                .AddTransient<IFoo, Foo>()
                .AddScoped<IBar, Bar>()
                .AddSingleton<IBaz, Baz>()
               // .BuildServiceProvider(true))
                .BuildServiceProvider())
            {
                root.GetService<IBar>(); // will throw exception on root if we open the check.
                using (var scope = root.CreateScope())
                {
                    var provider = scope.ServiceProvider;
                    provider.GetService<IFoo>();
                    provider.GetService<IBar>();
                    provider.GetService<IBaz>();
                }
                Console.WriteLine("Child container is disposed.");
            }
            Console.WriteLine("Root container is disposed.");
        }
    }
}

