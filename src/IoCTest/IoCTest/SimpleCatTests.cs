
using System;
using Xunit;

namespace IoCTest
{
    public class SimpleCatTests
    {
        [Fact]
        public void BasicTest()
        {
            SimpleCat cat = new SimpleCat();
            cat.Register<IFoo, Foo>();
            cat.Register<IBar, Bar>();
            cat.Register<IBaz, Baz>();
            cat.Register<IQux, Qux>();

            IFoo service = cat.GetService<IFoo>();
            Foo foo = (Foo)service;
            Baz baz = (Baz)foo.Baz;

            Console.WriteLine("cat.GetService<IFoo>(): {0}", service);
            Console.WriteLine("cat.GetService<IFoo>().Bar: {0}", foo.Bar);
            Console.WriteLine("cat.GetService<IFoo>().Baz: {0}", foo.Baz);
            Console.WriteLine("cat.GetService<IFoo>().Baz.Qux: {0}", baz.Qux);
        }

        public interface IFoo { }
        public interface IBar { }
        public interface IBaz { }
        public interface IQux { }

        public class Foo : IFoo
        {
            public IBar Bar { get; private set; }

            [Injection]
            public IBaz Baz { get; set; }

            public Foo() { }

            [Injection]
            public Foo(IBar bar)
            {
                this.Bar = bar;
            }
        }

        public class Bar : IBar { }

        public class Baz : IBaz
        {
            public IQux Qux { get; private set; }

            [Injection]
            public void Initialize(IQux qux)
            {
                this.Qux = qux;
            }
        }

        public class Qux : IQux { }
    }
}
