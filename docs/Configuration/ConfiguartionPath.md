### What's Configuration Path

A <strong> configuration path</strong> is a **string** separate from a delimiter. The default delimiter in ASP.NET Core Configuration is ":".

See the code [here](https://github.com/aspnet/Configuration/blob/dev/src/Microsoft.Extensions.Configuration.Abstractions/ConfigurationPath.cs#L17):
```C#
public static readonly string KeyDelimiter = ":";
```

### Methods
#### 1. [Combine(...)](https://github.com/aspnet/Configuration/blob/dev/src/Microsoft.Extensions.Configuration.Abstractions/ConfigurationPath.cs#L24): Combines the segments string into a path string using delimiter.
```C#
public static string Combine(params string[] pathSegments)
public static string Combine(IEnumerable<string> pathSegments)
```

Examples:
```C#
string path1 = ConfigurationPath.Combine("a", "b", "c"); // path1 == "a:b:c"

IList<string> segments = new List<string> { "x", "y", "z" };
string path2 = ConfigurationPath.Combine(segments); // path2 == "x:y:z"

```

#### 2. [GetSectionKey(...)](https://github.com/aspnet/Configuration/blob/dev/src/Microsoft.Extensions.Configuration.Abstractions/ConfigurationPath.cs#L52): extracts the last path segment from the path.
```C#
public static string GetSectionKey(string path)
```

Examples:
```C#
string section1 = ConfigurationPath.GetSectionKey(path1); // section1 == "c"
string section2 = ConfigurationPath.GetSectionKey(path2); // section2 == "z"
```

#### 3. [GetParentPath(...)](https://github.com/aspnet/Configuration/blob/dev/src/Microsoft.Extensions.Configuration.Abstractions/ConfigurationPath.cs#L68): extracts the the path corresponding to the parent node from the path.
```C#
public static string GetParentPath(string path)
```

Examples:
```C#
string parent1 = ConfigurationPath.GetParentPath(path1); // parent1 == "a:b"
string parent2 = ConfigurationPath.GetParentPath(path2); // parent2 == "x:y"

string parent3 = ConfigurationPath.GetParentPath(ConfigurationPath.GetParentPath(path1)); // parent3 == "a"
```

GetParentPath(...) will return `null` if the given path is
- null
- ""
- any string without delimiter
  
Besides, you can travel the path parent using:
```C#
string parent = path1;
while(parent != null)
{
  Console.WriteLine(parent);
  parent = ConfigurationPath.GetParentPath(parent);
}
```

The output is:
```txt
a:b:c
a:b
a
```
