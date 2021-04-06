// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.

namespace AttributeRouting80Rc.Models
{
    public class Address
    {
        public string City { get; set; }
        public string Street { get; set; }
    }

    public class CnAddress : Address
    {
        public string Postcode { get; set; }
    }

    public class UsAddress : Address
    {
        public string Zipcode { get; set; }
    }
}
