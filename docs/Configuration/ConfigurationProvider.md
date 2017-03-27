## IConfigurationProvider

![IConfigurationProvider Interface](https://github.com/xuzhg/AspNetCore/blob/master/Images/IConfigurationProvider.png)


## ConfigurationProvider
It's an abstract class implemented `IConfigurationProvider` interface.

It's a wrapper for the **Dictionary<string, string>**

```C#
private ConfigurationReloadToken _reloadToken = new ConfigurationReloadToken();

protected IDictionary<string, string> Data { get; set; }
```


## MemoryConfigurationProvider

It's concrete class derived from `ConfigurationProvider`.

It simply fetches the data from `MemoryConfigurationSource.InitialData` to `Data` in its base class.

The basic usage is to call the extension method for:
```C#
public static IConfigurationBuilder AddInMemoryCollection(
            this IConfigurationBuilder configurationBuilder,
            IEnumerable<KeyValuePair<string, string>> initialData)
```
the given `initialData` will be assigned to `MemoryConfigurationSource.InitialData`, then it will save the values into the `Data` when construct the `MemoryConfigurationProvider `.
Below is an example:

```C#
var dic = new Dictionary<string, string>
{
      {"Section:Integer", "-2"},
      {"Section:Boolean", "TRUe"},
      {"Section:Nested:Integer", "11"},
      {"Section:Virtual", "Sup"}
};
var configurationBuilder = new ConfigurationBuilder();
configurationBuilder.AddInMemoryCollection(dic);
var config = configurationBuilder.Build();

```
