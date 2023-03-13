//-----------------------------------------------------------------------------
// <copyright file="CborODataJsonWriterFactory.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.OData;
using Microsoft.OData.Json;
using System.Text;

namespace ODataCborExample.Extensions
{
    public class CborODataJsonWriterFactory : IStreamBasedJsonWriterFactory
    {
        private ODataMessageInfo _messageInfo;
        private IJsonWriterFactoryAsync _jsonWriterFactory;

        public CborODataJsonWriterFactory(ODataMessageInfo messageInfo, IJsonWriterFactoryAsync jsonWriterFactory)
        {
            _messageInfo = messageInfo;
            _jsonWriterFactory = jsonWriterFactory;
        }

        public IJsonWriter CreateJsonWriter(Stream stream, bool isIeee754Compatible, Encoding encoding)
            => throw new NotImplementedException();

        public IJsonWriterAsync CreateAsynchronousJsonWriter(Stream stream, bool isIeee754Compatible, Encoding encoding)
        {
            if (_messageInfo.MediaType.Type == "application" && _messageInfo.MediaType.SubType == "cbor")
            {
                return new CborODataWriter(stream, isIeee754Compatible, encoding);
            }

            // delegete to normal json
            TextWriter textWriter = new StreamWriter(stream, encoding);
            return _jsonWriterFactory.CreateAsynchronousJsonWriter(textWriter, isIeee754Compatible);
        }
    }
}
