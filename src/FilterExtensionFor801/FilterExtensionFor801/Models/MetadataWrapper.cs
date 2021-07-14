using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace FilterExtensionFor801.Models
{
    public class MetadataWrapper
    {
        private static object ConvertValue(JsonElement jsonElement)
        {
            switch(jsonElement.ValueKind)
            {
                case JsonValueKind.String:
                    return jsonElement.GetString();

                case JsonValueKind.Number:
                    return jsonElement.GetInt32();
            }

            throw new NotSupportedException("Not supported json kind yet");
        }

        private static IDictionary<string, object> ConvertToDict(IDictionary<string, object> value)
        {
            IDictionary<string, object> results = new Dictionary<string, object>();
            foreach (var item in value)
            {
                if (item.Value == null)
                {
                    results[item.Key] = null;
                }
                else
                {
                    Type type = item.Value.GetType();
                    if (type == typeof(JsonElement))
                    {
                        JsonElement jsonElement = (JsonElement)item.Value;
                        results[item.Key] = ConvertValue(jsonElement);
                    }
                    else
                    {
                        results[item.Key] = item.Value;
                    }
                }
            }

            return results;
        }

        public static implicit operator string(MetadataWrapper wrapper)
        {
            return wrapper.ToJson();
        }

        public  static implicit operator MetadataWrapper(string metadata)
        {
            object value = JsonSerializer.Deserialize(metadata, typeof(IDictionary<string, object>));

            CustomerMetadata cm = new CustomerMetadata();
            cm.Dynamics = ConvertToDict(value as IDictionary<string, object>);
            return new MetadataWrapper(cm);
        }

        public static implicit operator CustomerMetadata(MetadataWrapper wrapper)
        {
            return wrapper.Metadata;
        }

        public static implicit operator MetadataWrapper(CustomerMetadata metadata)
        {
            return new MetadataWrapper(metadata);
        }

        protected MetadataWrapper(CustomerMetadata metadata)
        {
            Metadata = metadata;
        }

        public CustomerMetadata Metadata { get; }

        public string ToJson()
        {
            return JsonSerializer.Serialize(Metadata.Dynamics);
        }
    }
}
