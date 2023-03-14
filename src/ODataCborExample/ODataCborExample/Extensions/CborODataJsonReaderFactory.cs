//-----------------------------------------------------------------------------
// <copyright file="CborODataJsonReaderFactory.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.OData;
using Microsoft.OData.Json;

namespace ODataCborExample.Extensions
{
    public class CborODataJsonReaderFactory : IJsonReaderFactory
    {
        private ODataMessageInfo _messageInfo;
        private IJsonReaderFactory _innerFactory;

        public CborODataJsonReaderFactory(ODataMessageInfo messageInfo, IJsonReaderFactory innerFactory)
        {
            _messageInfo = messageInfo;
            _innerFactory = innerFactory;
        }

        public IJsonReader CreateJsonReader(TextReader textReader, bool isIeee754Compatible)
        {
            if (_messageInfo.MediaType.Type == "application" && _messageInfo.MediaType.SubType == "cbor")
            {
                // OData provides the StreamReader by default, so it's saft to cast to StreamReader.
                // You'd not rely on this assumption. And ODL should provide 
                // IStreamBasedODataJosonReaderFactory.
                StreamReader reader = textReader as StreamReader;
                return new CborODataReader(reader.BaseStream);
            }

            return _innerFactory.CreateJsonReader(textReader, isIeee754Compatible);
        }
    }
}
