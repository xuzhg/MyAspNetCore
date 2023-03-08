//-----------------------------------------------------------------------------
// <copyright file="CustomersController.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using ODataCustomizePayloadFormat.Models;

namespace ODataCustomizePayloadFormat.Controllers;

public class CustomersController : ControllerBase
{
    private static IList<Customer> _customers = GetCustomers();

    [HttpGet]
    [EnableQuery]
    public IActionResult Get()
    {
        return Ok(_customers);
    }

    [HttpGet]
    [EnableQuery]
    public Customer Get(int key)
    {
        Customer customer = _customers.FirstOrDefault(c => c.Id == key);
        return customer;
    }

    [HttpGet]
    [EnableQuery]
    public IActionResult GetHomeAddress(int key)
    {
        Customer customer = _customers.FirstOrDefault(c => c.Id == key);
        return Ok(customer.HomeAddress);
    }

    [HttpGet]
    [EnableQuery]
    public IActionResult GetFavoriteAddresses(int key)
    {
        Customer customer = _customers.FirstOrDefault(c => c.Id == key);
        return Ok(customer.FavoriteAddresses);
    }

    private static IList<Customer> GetCustomers()
    {
        return new List<Customer>
        {
            new Customer
            {
                Id = 1,
                Name = "Jonier",
                FavoriteColor = Color.Red,
                HomeAddress = new Address { City = "Redmond", Street = "156 AVE NE" },
                Amount = 3,
                Tokens = new List<string>{ "1a", "1b", "1c" },
                FavoriteAddresses = new List<Address>
                {
                    new Address { City = "Redmond", Street = "256 AVE NE" },
                    new Address { City = "Redd", Street = "56 AVE NE" },
                },
            },
            new Customer
            {
                Id = 2,
                Name = "Sam",
                FavoriteColor = Color.Blue,
                HomeAddress = new CnAddress { City = "Bellevue", Street = "Main St NE", Postcode = "201100" },
                Amount = 6,
                Tokens = new List<string>{ "2k", "2C" },
                FavoriteAddresses = new List<Address>
                {
                    new Address { City = "Red4ond", Street = "456 AVE NE" },
                    new Address { City = "Re4d", Street = "51 NE" },
                },
            },
            new Customer
            {
                Id = 3,
                Name = "Peter",
                FavoriteColor = Color.Green,
                HomeAddress = new UsAddress { City = "Hollewye", Street = "Main St NE", Zipcode = "98029" },
                Amount = 4,
                Tokens = new List<string>{ }, // empty dy default
                FavoriteAddresses = new List<Address>
                {
                    new Address { City = "R4mond", Street = "546 NE" },
                    new Address { City = "R4d", Street = "546 AVE" },
                },
            },
            new Customer
            {
                Id = 4,
                Name = "Sam",
                FavoriteColor = Color.Red,
                HomeAddress = new UsAddress { City = "Banff", Street = "183 St NE", Zipcode = "111" },
                Amount = 5,
                Tokens = new List<string>{ "4x", "4y" },
                FavoriteAddresses = new List<Address>
                {
                    new Address { City = "R4m11ond", Street = "116 NE" },
                    new Address { City = "Jesper", Street = "5416 AVE" },
                }
            }
        };
    }
}
