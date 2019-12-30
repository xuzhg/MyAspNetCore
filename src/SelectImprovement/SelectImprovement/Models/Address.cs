using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SelectImprovement.Models
{
    public class Address
    {
        public string Street { get; set; }

        public string City { get; set; }

        public ZipCode ZipCode { get; set; }
    }

    public class BillAddress : Address
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
