//---------------------------------------------------------------------
// <copyright file="ProtobufReader.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//---------------------------------------------------------------------

using Bookstores;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Microsoft.OData;
using Microsoft.OData.Edm;
using System.Reflection;

namespace ODataProtobufExample.Protobuf;

public class ProtobufReader : ODataReader
{
    private ProtobufInputContext _context;
    private ODataReaderState _state;
    private ODataItem _item;
    private IEdmStructuredType _structuredType;
    public ProtobufReader(ProtobufInputContext context, IEdmStructuredType structuredType)
    {
        _context = context;
        _structuredType = structuredType;
        _state = ODataReaderState.Start;
        _item = null;
    }

    public override ODataItem Item => _item;

    public override ODataReaderState State => _state;

    public override bool Read()
    {
        // for simplicity
        bool result;
        switch (_state)
        {
            case ODataReaderState.Start:
                result = ReadAtStart();
                break;

            case ODataReaderState.ResourceStart:
                _state = ODataReaderState.ResourceEnd;
                result = true;
                break;

            case ODataReaderState.ResourceEnd:
                _state = ODataReaderState.Completed;
                result = false;
                break;

            default:
                throw new NotImplementedException("DIY!");
        }

        return result;
    }

    public override Task<bool> ReadAsync()
    {
        bool ret = Read();
        return Task.FromResult(ret);
    }

    private bool ReadAtStart()
    {
        // for simplicity
        byte[] data = ReadAllBytes(_context.MessageStream);
        string typeName = _structuredType.FullTypeName();
        if (typeName == "Bookstores.Book")
        {
            Book book = Book.Parser.ParseFrom(data);
            _item = new ODataResource
            {
                TypeName = _structuredType.FullTypeName(),
                Properties = BuildProperties(book).ToList()
            };

        }
        else
        {
            Shelf shelf = Shelf.Parser.ParseFrom(data);
            _item = new ODataResource
            {
                TypeName = _structuredType.FullTypeName(),
                Properties = BuildProperties(shelf).ToList()
            };
        }

        _state = ODataReaderState.ResourceStart;
        return true;
    }

    private IEnumerable<ODataProperty> BuildProperties<T>(T source) where T : IMessage
    {
        Type sourceType = typeof(T);

        IList<FieldDescriptor> fields = sourceType == typeof(Book) ?
            Book.Descriptor.Fields.InFieldNumberOrder() :
            Shelf.Descriptor.Fields.InFieldNumberOrder();
        //PropertyInfo descriptor = sourceType.GetProperty("Descriptor", BindingFlags.Public | BindingFlags.Static);
        //object descriptorValue = descriptor.GetValue(null);

        foreach (var field in fields)
        {
            if (field.IsRepeated)
            {
                continue;
            }

            string hasField = $"Has{field.PropertyName}";
            PropertyInfo hasFieldProperty = sourceType.GetProperty(hasField);
            bool hasFieldValue = (bool)hasFieldProperty.GetValue(source);
            if (hasFieldValue)
            {
                //PropertyInfo fieldProperty = sourceType.GetProperty(field.PropertyName);
                //object fieldValue = fieldProperty.GetValue(source);
                object fieldValue = field.Accessor.GetValue(source);

                yield return new ODataProperty
                {
                    Name = field.PropertyName,
                    Value = fieldValue,
                };
            }
        }
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
}
