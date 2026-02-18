// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

A a = new A();
B b = a; // Implicit conversion from A to B

A a2 = (A)b; // Explicit conversion from B to A


ODataNullResource nullResource = new ODataNullResource();

ODataResource? resource2 = nullResource;

ODataResource? resource = (ODataResource?)nullResource;

if (resource == null)
{
    Console.WriteLine("The resource is null.");
}
else
{
    Console.WriteLine("The resource is not null.");
}

public class A
{
    public static implicit operator B(A a) => new B();
}

public class B
{
       public static explicit operator A(B b) => new A();
}

public abstract class ODataResourceBase { }

public sealed class ODataResource : ODataResourceBase { }

public sealed class ODataDeletedResource : ODataResourceBase { }

public sealed class ODataNullResource : ODataResourceBase
{
    public static implicit operator ODataResource?(ODataNullResource a) => new ODataResource();
}