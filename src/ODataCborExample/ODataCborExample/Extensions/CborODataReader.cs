//-----------------------------------------------------------------------------
// <copyright file="CborODataReader.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.OData.Json;
using System.Formats.Cbor;

namespace ODataCborExample.Extensions;

public partial class CborODataReader : IJsonReader, IJsonReaderAsync
{
    private Stack<JsonNodeType> _scopes = new Stack<JsonNodeType>();

    private CborReader _cborReader;

    public CborODataReader(Stream stream)
    {
        byte[] data = ReadAllBytes(stream);
        _cborReader = new CborReader(new ReadOnlyMemory<byte>(data));
        _scopes = new Stack<JsonNodeType>();
    }

    private static byte[] ReadAllBytes(Stream instream)
    {
        if (instream is MemoryStream)
            return ((MemoryStream)instream).ToArray();

        using (var memoryStream = new MemoryStream())
        {
            instream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }

    public object Value { get; private set; }

    public JsonNodeType NodeType { get; private set; } = JsonNodeType.None;

    public bool IsIeee754Compatible => throw new NotImplementedException();

    public bool Read()
    {
        CborReaderState readerState = _cborReader.PeekState();
        switch (readerState)
        {
            case CborReaderState.Finished:
                NodeType = JsonNodeType.EndOfInput;
                return false;

            case CborReaderState.StartArray:
                _cborReader.ReadStartArray();
                NodeType = JsonNodeType.StartArray;
                _scopes.Push(JsonNodeType.StartArray);
                return true;

            case CborReaderState.EndArray:
                _cborReader.ReadEndArray();
                NodeType = JsonNodeType.EndArray;
                _scopes.Pop();
                return true;

            case CborReaderState.StartMap:
                _cborReader.ReadStartMap();
                NodeType = JsonNodeType.StartObject;
                _scopes.Push(JsonNodeType.StartObject);
                return true;

            case CborReaderState.EndMap:
                _cborReader.ReadEndMap();
                NodeType = JsonNodeType.EndObject;
                _scopes.Pop();
                return true;

            case CborReaderState.UnsignedInteger:
                NodeType = ParsePrimitiveValueNodeType();
                Value = _cborReader.ReadUInt32();
                return true;

            case CborReaderState.TextString:
                NodeType = ParsePrimitiveValueNodeType();
                Value = _cborReader.ReadTextString();
                return true;

            case CborReaderState.Boolean:
                NodeType = ParsePrimitiveValueNodeType();
                Value = _cborReader.ReadBoolean();
                return true;

            case CborReaderState.ByteString:
                NodeType = ParsePrimitiveValueNodeType();
                Value = _cborReader.ReadByteString();
                return true;

            default:
                throw new NotImplementedException($"Not implemented, please add more case to handle {readerState}");
        }
    }

    private JsonNodeType ParsePrimitiveValueNodeType()
    {
        if (_scopes.Count > 0)
        {
            JsonNodeType nodeType = _scopes.Peek();
            if(nodeType == JsonNodeType.StartObject)
            {
                _scopes.Push(JsonNodeType.Property);
                return JsonNodeType.Property;
            }
            else if (nodeType == JsonNodeType.Property)
            {
                _scopes.Pop(); // pop property scope
            }
        }

        return JsonNodeType.PrimitiveValue;
    }

    /* Keep them for later reference.
    public bool Read()
    {
        if (_scopes.Count == 0)
        {
            CborReaderState readerState = _cborReader.PeekState();
            NodeType = JsonNodeType.EndOfInput;
            return false;
        }

        ScopeType currentScope = _scopes.Peek();
        JsonNodeType nodeType;
        object nodeValue;
        switch (currentScope)
        {
            case ScopeType.Root:
                // We expect a "value" - start array, start object or primitive value
                (nodeType, nodeValue) = ParseValue();
                break;

            case ScopeType.Array:
                (nodeType, nodeValue) = ParseValue();
                if (nodeType == JsonNodeType.EndArray)
                {
                    _scopes.Pop();
                }

                break;

            case ScopeType.Object:
                (nodeType, nodeValue) = ParseValue();
                if (nodeType == JsonNodeType.EndObject)
                {
                    _scopes.Pop();
                    break;
                }

                if (nodeType != JsonNodeType.PrimitiveValue)
                {
                    throw new InvalidOperationException($"Wrong node type {nodeType} in object {currentScope}");
                }

                _scopes.Push(ScopeType.Property);
                nodeType = JsonNodeType.Property;

                break;

            case ScopeType.Property:
                (nodeType, nodeValue) = ParseValue();
                break;

            default:
                throw new InvalidOperationException($"{currentScope} is current scope, but some wrong occurs.");
        }

        NodeType = nodeType;
        Value = nodeValue;
        return true;
    }

    private (JsonNodeType, object) ParseValue()
    {
        CborReaderState readerState = _cborReader.PeekState();
        JsonNodeType nodeType = JsonNodeType.None;
        object nodeValue = null;
        switch (readerState)
        {
            case CborReaderState.StartArray: // "[
                _cborReader.ReadStartArray();
                nodeType = JsonNodeType.StartArray;
                _scopes.Push(ScopeType.Array);
                break;

            case CborReaderState.EndArray:
                _cborReader.ReadEndArray();
                nodeType = JsonNodeType.EndArray;
                _scopes.Pop();
                break;

            case CborReaderState.StartMap:
                nodeType = JsonNodeType.StartObject;
                _cborReader.ReadStartMap();
                _scopes.Push(ScopeType.Object);
                break;

            case CborReaderState.EndMap:
                nodeType = JsonNodeType.EndObject;
                _cborReader.ReadEndMap();
                _scopes.Pop();
                break;

            case CborReaderState.UnsignedInteger:
                nodeType = JsonNodeType.PrimitiveValue;
                nodeValue = _cborReader.ReadUInt32();
                break;

            case CborReaderState.TextString:
                nodeType = JsonNodeType.PrimitiveValue;
                nodeValue = _cborReader.ReadTextString();
                break;

            case CborReaderState.Boolean:
                nodeType = JsonNodeType.PrimitiveValue;
                nodeValue = _cborReader.ReadBoolean();
                break;

            case CborReaderState.ByteString:
                nodeType = JsonNodeType.PrimitiveValue;
                nodeValue = _cborReader.ReadByteString();
                break;

            case CborReaderState.Finished:
                nodeType = JsonNodeType.EndOfInput;
                break;
        }

        TryPopPropertyScope();

        return (nodeType, nodeValue);
    }

    private void TryPopPropertyScope()
    {
        if (_scopes.Peek() == ScopeType.Property)
        {
            _scopes.Pop();
        }
    }*/

    public Task<object> GetValueAsync()
    {
        return Task.FromResult(Value);
    }

    public Task<bool> ReadAsync()
    {
        bool result = Read();
        return Task.FromResult(result);
    }
}
