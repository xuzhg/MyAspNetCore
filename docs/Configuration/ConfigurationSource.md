## IConfigurationSource

```C#

public interface IConfigurationSource
{
   IConfigurationProvider Build(IConfigurationBuilder builder);
}

```

The `Build(...)` method will be called by `ConfigurationBuilder`.

# Implementation

Here's the implementation for the `IConfigurationSource`

- MemoryConfigurationSource
- AzureKeyVaultKeyCongfigurationSource
- CommandLineConfigurationSource
- DockerSecretsConfigurationSource
- FileConfiguarationSource
  - IniConfigurationSource
  - JsonConfigurationSource
  - XmlConfigurationSource
  
  
Each implementation of `IConfigurationSource` is very simple, the `Build(...)` method will return the corresponding `IConfigurationProvider`.
For example:

```C#
public class JsonConfigurationSource : FileConfigurationSource
{
      public override IConfigurationProvider Build(IConfigurationBuilder builder)
      {
         EnsureDefaults(builder);
         return new JsonConfigurationProvider(this);
      }
}
```
The `JsonConfigurationSource` returns the `JsonConfigurationProvider`.

For the `IConfigurationProvider`, please refer to [HERE](https://github.com/xuzhg/AspNetCore/blob/master/docs/Configuration/ConfigurationProvider.md)
