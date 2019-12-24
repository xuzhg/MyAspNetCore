using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MyLearn.Models
{
    public interface IFoo
    { }

    public interface IBar
    { }

    public interface IBaz
    { }

    public class Foo : Disposable, IFoo { }

    public class Bar : Disposable, IBar { }

    public class Baz : Disposable, IBaz { }

    public class Disposable : IDisposable
    {
        public void Dispose()
        {
            Debug.WriteLine($"{this.GetType()}.Dispose()");
        }
    }

    public interface IFoobar : IDisposable
    {

    }

    public class Foobar : IFoobar
    {
        ~Foobar()
        {
            Debug.WriteLine("Foobar.Finalize()");
        }

        public void Dispose()
        {
            Debug.WriteLine("Foobar.Dispose()");
        }
    }
}
