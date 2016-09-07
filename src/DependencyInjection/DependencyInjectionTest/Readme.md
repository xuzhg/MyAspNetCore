## DI has two methodologies

### Constructor-based dependency injection

```C#
public interface IMyInterface
{
}

public class MyClass : IMyInterface
{
}

public class DIClass
{
   public DIClass(IMyInterface myInterface)
   {
      ...
   }
}

```

So, we can use the DI to create the instance of `DIClass`, the DI will help us create a `IMyInterface` instance and pass as argument to call the constructor:

* Register the service:

```C#
IServiceCollection services = new ServiceCollection();
services.AddSingleton(typeof(IMyInterface), typeof(MyClass));
services.AddScoped<DIClass>();

```

* Get the service object

```C#
IServiceProvider provider = services.BuildServiceProvider();
DIClass c = provider.GetRequiredService<DIClass>(); // here will call MyClass construtor, then call DIClass construtor.
```

**Where**, `c` will be a valid object.


### Action parameter injection with [FromServices] attribute

```C#
public IActionResult About([FromServices] IMyInterface myInterface)
{
}
```
