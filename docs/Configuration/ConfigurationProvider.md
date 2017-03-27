## IConfigurationProvider

![IConfigurationProvider Interface](https://github.com/xuzhg/AspNetCore/blob/master/Images/IConfigurationProvider.png)


## ConfigurationProvider

![ConfigurationProvider Interface](https://github.com/xuzhg/AspNetCore/blob/master/Images/ConfigurationProvider.png)

It's an abstract class implemented `IConfigurationProvider` interface.

It's a wrapper for the **Dictionary<string, string>**

```C#
private ConfigurationReloadToken _reloadToken = new ConfigurationReloadToken();

protected IDictionary<string, string> Data { get; set; }
```

It has a default virtual method as below:
```C#
public virtual void Load()
{
  // nothing here.
}
```
`Load()` method will be called in the construct of `ConfigurationRoot` for each provider.


## MemoryConfigurationProvider

It's concrete class derived from `ConfigurationProvider`.

It simply fetches the data from `MemoryConfigurationSource.InitialData` to `Data` in its base class.

It doesn't override the `Load()` method.
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

## CommandLineConfigurationProvider

It's a concrete class derived from `ConfigurationProvider`.

#### switchMappings
The switch is key to key mapping. for example:

```C#
var switchMappings = new Dictionary<string, string>(StringComparer.Ordinal)
{
    { "--KEY1", "LongKey1" },
    { "--key1", "SuperLongKey1" },
    { "-Key2", "LongKey2" },
    { "-KEY2", "LongKey2"}
};
```
So, the `KEY1` is mapped to `LongKey1`, etc.
the switch key must start with `--` or `-`.

#### args

it will override `Load()` method to parse the `command line args`.

for example:
```C#
var args = new string[]
{
     "-K1=Value1",
     "--Key2=Value2",
     "/Key3=Value3",
     "--Key4", "Value4",
     "/Key5", "Value5"
};
```
the arg key must start with `--` or `-` or `/`. It can contain `=`.

## EnvironmentVariablesConfigurationProvider

It's a concrete class derived from `ConfigurationProvider`. 
It overrides the `Load()` method to parse the environment variables.
```C#
public override void Load()
{
   Load(Environment.GetEnvironmentVariables());
}
```

## DockerSecretsConfigurationProvider
It's a concrete class derived from `ConfigurationProvider`. 
It overrides the `Load()` method to parse the secret files in the secret directory.
The key is the file name, the value is the file content.

## AzureKeyVaultConfigurationProvider
It's a concrete class derived from `ConfigurationProvider`. 

## FileConfigurationProvider

It's an abstract class derived from `ConfigurationProvider`. 
It's the based class for 
- IniConfigurationProvider
- JsonConfigurationProvider
- XmlConfigurationProvider

Each sub class will override the method:
```C#
public abstract void Load(Stream stream);
```

### IniConfigurationProvider

```txt
[DefaultConnection]
ConnectionString=TestConnectionString
Provider=SqlClient
[Data:Inventory]
ConnectionString=AnotherTestConnectionString
SubHeader:Provider=MySql
```

### JsonConfigurationProvider
```json
{
    "firstname": "test",
    "test.last.name": "last.name",
        "residential.address": {
            "street.name": "Something street",
            "zipcode": "12345"
        }
}
```

### XmlConfigurationProvider

```xml
<settings>
  <Data.Setting>
    <DefaultConnection>
      <Connection.String>Test.Connection.String</Connection.String>
      <Provider>SqlClient</Provider>
    </DefaultConnection>
    <Inventory>
      <ConnectionString>AnotherTestConnectionString</ConnectionString>
      <Provider>MySql</Provider>
    </Inventory>
  </Data.Setting>
</settings>
```
