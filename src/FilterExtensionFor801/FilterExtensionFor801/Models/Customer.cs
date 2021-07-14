using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FilterExtensionFor801.Models
{
    public class Customer
    {
        private MetadataWrapper _wrapper;

        public int Id { get; set; }

        public string Name { get; set; }

        public string CustomMetadata
        {
            get => _wrapper;
            set => _wrapper = value;
        }

        [NotMapped] // Not put into DB
        public CustomerMetadata Metadata
        {
            get => _wrapper;
            set => _wrapper = value;
        }
    }
}
