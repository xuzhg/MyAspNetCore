## IConfigurationBuilder

![IConfigurationBuilder Interface](https://github.com/xuzhg/AspNetCore/blob/master/Images/ConfigurationBuilder.png)

## ConfigurationBuilder

It's the default implementation of `IConfigurationBuilder` interface. 
Basically, it's a wrapper of `IList<IConfigurationSource>`.

You can access the readonly property `Sources` to query the `IConfiguarationSource` added in this builder.
You can call `Add(IConfigurationSource source)` method to add a `IConfigurationSource` into this builder.

The main/key method is `Build()`, it returns an instance of `IConfigurationRoot`.
Below is the detail implmentation of `Build()` method:

```C#
public IConfigurationRoot Build()
{
   var providers = new List<IConfigurationProvider>();
   foreach (var source in _sources)
   {
       var provider = source.Build(this);
       providers.Add(provider);
   }
   return new ConfigurationRoot(providers);
}
```
