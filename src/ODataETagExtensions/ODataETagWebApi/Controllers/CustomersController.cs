using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Logging;
using ODataETagWebApi.Models;

namespace ODataETagWebApi.Controllers
{
    public class CustomersController : ODataController
    {
        private static IList<Customer> Customers = new List<Customer>
        {
            new Customer { Id = 1, Name = "Freezing", Label = new Guid("81B06CD0-3B66-4447-B193-94B11328A762") },
            new Customer { Id = 2, Name = "Bracing", Label = new Guid("629C11E1-1918-4978-AFE0-F90BA6A452C6") },
            new Customer { Id = 3, Name = "Chilly", Label = new Guid("CA02BF9C-8364-4320-B74F-CB8956C9A502") },
        };

        [HttpGet]
        [EnableQuery]
        public IActionResult Get()
        {
            $filter = Id eq 2

                ==> Customers.Where(c => c.Id == 2);
            return Ok(Customers);
        }

        [HttpGet]
        [EnableQuery]
        public IActionResult Get(int key)
        {
            Customer c = Customers.FirstOrDefault(c => c.Id == key);
            if (c == null)
            {
                return NotFound($"Cannot find customer with Id={key}");
            }

            return Ok(c);
        }

        [HttpPost]
        public IActionResult Post([FromBody]Customer customer)
        {
            customer.Id = Customers.Last().Id + 1;
            if (customer.Label == Guid.Empty)
            {
                customer.Label = Guid.NewGuid();
            }

            Customers.Add(customer);
            return Created(customer);
        }

        [HttpPatch]
        public IActionResult Patch(int key, Delta<Customer> patch)
        {
            Customer old = Customers.FirstOrDefault(c => c.Id == key);
            if (old == null)
            {
                return NotFound($"Cannot find customer with Id={key}");
            }

            if (!patch.TryGetPropertyValue("Label", out object value))
            {
                return BadRequest($"Cannot find the ETag value");
            }

            Guid labelValue = (Guid)value;

            if (labelValue != old.Label)
            {
                return BadRequest($"ETag value can not match!");
            }

            patch.Patch(old);
            return Updated(old);
        }
    }
}
