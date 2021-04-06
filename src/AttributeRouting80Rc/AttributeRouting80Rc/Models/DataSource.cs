// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace AttributeRouting80Rc.Models
{
    public static class DataSource
    {
        private static IList<Book> _books { get; set; }
        private static IList<Customer> _customers { get; set; }

        public static IList<Book> GetBooks()
        {
            if (_books != null)
            {
                return _books;
            }

            _books = new List<Book>();

            // book #1
            Book book = new Book
            {
                Id = 1,
                ISBN = "978-0-321-87758-1",
                Title = "Essential C#5.0",
                Author = "Mark Michaelis",
                Price = 59.99m,
                Location = new Address { City = "Redmond", Street = "156TH AVE NE" },
                Press = new Press
                {
                    Id = 1,
                    Name = "Addison-Wesley",
                    Category = Category.Book
                }
            };
            _books.Add(book);

            // book #2
            book = new Book
            {
                Id = 2,
                ISBN = "063-6-920-02371-5",
                Title = "Enterprise Games",
                Author = "Michael Hugos",
                Price = 49.99m,
                Location = new Address { City = "Bellevue", Street = "Main ST" },
                Press = new Press
                {
                    Id = 2,
                    Name = "O'Reilly",
                    Category = Category.EBook,
                }
            };
            _books.Add(book);
 
            return _books;
        }


        public static IList<Customer> GetCustomers()
        {
            if (_customers != null)
            {
                return _customers;
            }

            _customers = new List<Customer>
            {
                new Customer
                {
                    Id = 1,
                    Name = "Jonier",
                    HomeAddress = new Address { City = "Hollewye", Street = "156TH AVE NE" },
                    Orders = new List<Order>
                    {
                        new Order { Id = 11, Price = 9 },
                        new Order { Id = 12, Price = 7 },
                    },
                },
                new Customer
                {
                    Id = 2,
                    Name = "Sam",
                    FavoriteColor = Color.Blue,
                    HomeAddress = new CnAddress { City = "Shanghai", Street = "Lianhua Rd", Postcode = "201100" },
                    Orders = new List<Order>
                    {
                        new Order { Id = 21, Price = 99 },
                        new Order { Id = 22, Price = 187 },
                    },
                },
                new Customer
                {
                    Id = 3,
                    Name = "Peter",
                    FavoriteColor = Color.Green,
                    HomeAddress = new UsAddress { City = "Redmond", Street = "Main St NE", Zipcode = "98029" },
                    Orders = new List<Order>
                    {
                        new Order { Id = 31, Price = 19 },
                        new Order { Id = 32, Price = 27 },
                    },
                }
            };

            return _customers;
        }
    }
}
