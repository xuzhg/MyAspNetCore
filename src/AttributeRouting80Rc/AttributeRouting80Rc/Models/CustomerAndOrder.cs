// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace AttributeRouting80Rc.Models
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Color FavoriteColor { get; set; }

       // public string[] Emails { get; set; }

        public virtual Address HomeAddress { get; set; }

        public virtual IList<Order> Orders { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }

        public int Price { get; set; }
    }

    public enum Color
    {
        Red,

        Green,

        Blue
    }
}
