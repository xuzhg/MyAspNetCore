//-----------------------------------------------------------------------------
// <copyright file="CborODataWriter.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.OData.Edm;
using Microsoft.OData.Json;
using System.Formats.Cbor;
using System.Text;

namespace ODataCborExample.Extensions;

public partial class CborODataWriter : IJsonWriter, IJsonWriterAsync
{
    private CborWriter _cborWriter = new CborWriter();

    private Stream _stream;
    private Encoding _encoding;
    private bool _isIeee754Compatible;

    public CborODataWriter(Stream stream, bool isIeee754Compatible, Encoding encoding)
    {
        _stream = stream;
        _isIeee754Compatible = isIeee754Compatible;
        _encoding = encoding;
    }

    public void EndArrayScope()
    {
        _cborWriter.WriteEndArray();
    }

    public void EndObjectScope()
    {
        _cborWriter.WriteEndMap();
    }

    public void EndPaddingFunctionScope()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Flush will be called twice from OData library
    /// Make sure we only write the bytes once.
    /// </summary>
    public void Flush()
    {
        if (!_cborWriter.IsWriteCompleted)
        {
            return;
        }

        var encode = _cborWriter.Encode();

        // if you want to get Base64 string, use the following codes
        //TextWriter sw = new StreamWriter(_stream);
        //sw.Write(Convert.ToBase64String(encode));
        //sw.Flush();

        // Be noted, if you write it as base64, please comment out the following code.
        _stream.Write(encode, 0, encode.Length);
        _stream.Flush();

        _cborWriter.Reset();
    }

    public void StartArrayScope()
    {
        _cborWriter.WriteStartArray(null);
    }

    public void StartObjectScope()
    {
        _cborWriter.WriteStartMap(null);
    }

    public void StartPaddingFunctionScope()
    {
        throw new NotImplementedException();
    }

    public void WriteName(string name)
    {
        _cborWriter.WriteTextString(name);
    }

    public void WritePaddingFunctionName(string functionName)
    {
        throw new NotImplementedException();
    }

    public void WriteRawValue(string rawValue)
    {
        _cborWriter.WriteTextString(rawValue);
    }

    public void WriteValue(bool value)
    {
        _cborWriter.WriteBoolean(value);
    }

    public void WriteValue(int value)
    {
        _cborWriter.WriteInt32(value);
    }

    public void WriteValue(float value)
    {
        _cborWriter.WriteSingle(value);
    }

    public void WriteValue(short value)
    {
        _cborWriter.WriteInt32(value);
    }

    public void WriteValue(long value)
    {
        _cborWriter.WriteInt64(value);
    }

    public void WriteValue(double value)
    {
        _cborWriter.WriteDouble(value);
    }

    public void WriteValue(Guid value)
    {
        // It's not write, but just do it now.
        _cborWriter.WriteTextString(value.ToString());
    }

    public void WriteValue(decimal value)
    {
        _cborWriter.WriteDecimal(value);
    }

    public void WriteValue(DateTimeOffset value)
    {
        _cborWriter.WriteDateTimeOffset(value);
    }

    public void WriteValue(TimeSpan value)
    {
        throw new NotImplementedException("No timespan writing");
    }

    public void WriteValue(byte value)
    {
        _cborWriter.WriteUInt32(value);// **
    }

    public void WriteValue(sbyte value)
    {
        _cborWriter.WriteInt32(value);// **
    }

    public void WriteValue(string value)
    {
        _cborWriter.WriteTextString(value);
    }

    public void WriteValue(byte[] value)
    {
        _cborWriter.WriteByteString(value);
    }

    public void WriteValue(Date value)
    {
        throw new NotImplementedException();
    }

    public void WriteValue(TimeOfDay value)
    {
        throw new NotImplementedException();
    }
}
