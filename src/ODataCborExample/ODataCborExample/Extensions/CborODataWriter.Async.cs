using Microsoft.OData.Edm;
using Microsoft.OData.Json;

namespace ODataCborExample.Extensions;

public partial class CborODataWriter : IJsonWriter, IJsonWriterAsync
{
    public Task EndArrayScopeAsync()
    {
        EndArrayScope();
        return Task.CompletedTask;
    }

    public Task EndObjectScopeAsync()
    {
        EndObjectScope();
        return Task.CompletedTask;
    }

    public Task EndPaddingFunctionScopeAsync()
    {
        throw new NotImplementedException();
    }

    public Task FlushAsync()
    {
        Flush();
        return Task.CompletedTask;
    }

    public Task StartArrayScopeAsync()
    {
        StartArrayScope();
        return Task.CompletedTask;
    }

    public Task StartObjectScopeAsync()
    {
        StartObjectScope();
        return Task.CompletedTask;
    }

    public Task StartPaddingFunctionScopeAsync()
    {
        throw new NotImplementedException();
    }

    public Task WriteNameAsync(string name)
    {
        WriteName(name);
        return Task.CompletedTask;
    }

    public Task WritePaddingFunctionNameAsync(string functionName)
    {
        throw new NotImplementedException();
    }

    public Task WriteRawValueAsync(string rawValue)
    {
        WriteRawValue(rawValue);
        return Task.CompletedTask;
    }

    public Task WriteValueAsync(bool value)
    {
        WriteValue(value);
        return Task.CompletedTask;
    }

    public Task WriteValueAsync(int value)
    {
        WriteValue(value);
        return Task.CompletedTask;
    }

    public Task WriteValueAsync(float value)
    {
        WriteValue(value);
        return Task.CompletedTask;
    }

    public Task WriteValueAsync(short value)
    {
        WriteValue(value);
        return Task.CompletedTask;
    }

    public Task WriteValueAsync(long value)
    {
        WriteValue(value);
        return Task.CompletedTask;
    }

    public Task WriteValueAsync(double value)
    {
        WriteValue(value);
        return Task.CompletedTask;
    }

    public Task WriteValueAsync(Guid value)
    {
        WriteValue(value);
        return Task.CompletedTask;
    }

    public Task WriteValueAsync(decimal value)
    {
        WriteValue(value);
        return Task.CompletedTask;
    }

    public Task WriteValueAsync(DateTimeOffset value)
    {
        WriteValue(value);
        return Task.CompletedTask;
    }

    public Task WriteValueAsync(TimeSpan value)
    {
        WriteValue(value);
        return Task.CompletedTask;
    }

    public Task WriteValueAsync(byte value)
    {
        WriteValue(value);
        return Task.CompletedTask;
    }

    public Task WriteValueAsync(sbyte value)
    {
        WriteValue(value);
        return Task.CompletedTask;
    }

    public Task WriteValueAsync(string value)
    {
        WriteValue(value);
        return Task.CompletedTask;
    }

    public Task WriteValueAsync(byte[] value)
    {
        WriteValue(value);
        return Task.CompletedTask;
    }

    public Task WriteValueAsync(Date value)
    {
        WriteValue(value);
        return Task.CompletedTask;
    }

    public Task WriteValueAsync(TimeOfDay value)
    {
        WriteValue(value);
        return Task.CompletedTask;
    }
}
