using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilterExtensionFor801.Models
{
    public class CustomerMetadata
    {
        public IDictionary<string, object> Dynamics { get; set; } = new Dictionary<string, object>();
    }
}
