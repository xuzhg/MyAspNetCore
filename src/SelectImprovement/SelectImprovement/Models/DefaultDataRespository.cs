using System.Collections.Generic;

namespace SelectImprovement.Models
{
    public class DefaultDataRespository : IDataRepository
    {
        private IList<Customer> _customers;

        public IEnumerable<Customer> GetCustomers()
        {
            if (_customers == null)
            {
                Address[] addresses = new Address[]
                {
                    new Address
                    {
                        Street = "145TH AVE",
                        City = "Redonse",
                        ZipCode = new ZipCode { Id = 71, DisplayName = "aebc" }
                    },
                    new BillAddress // Bill
                    {
                        FirstName = "Peter",
                        LastName = "Jok",
                        Street = "Main ST",
                        City = "Issaue",
                        ZipCode = new ZipCode { Id = 61, DisplayName = "yxbc" }
                    },
                    new Address
                    {
                        Street = "32ST NE",
                        City = "Bellewe",
                        ZipCode = new ZipCode { Id = 81, DisplayName = "bexc" }
                    }
                };

                Order[] orders = new Order[]
                {
                    new Order
                    {
                        Id = 11,
                        Title = "Redonse"
                    },
                    new Order
                    {
                        Id = 12,
                        Title = "Bellewe"
                    }
                };

                _customers = new List<Customer>
                {
                    new Customer
                    {
                        Id = 1,
                        Name = "Balmy",
                        Emails = new List<string> { "E1", "E3", "E2" },
                        HomeAddress = addresses[0],
                        FavoriteAddresses = addresses,
                        PersonOrder = orders[0],
                        Orders = orders
                    },
                    new Customer
                    {
                        Id = 2,
                        Name = "Chilly",
                        Emails = new List<string> { "E8", "E7", "E9" },
                        HomeAddress = addresses[1],
                        FavoriteAddresses = addresses,
                        PersonOrder = orders[1],
                        Orders = orders
                    },
                };
            }

            return _customers;
        }
    }
}
