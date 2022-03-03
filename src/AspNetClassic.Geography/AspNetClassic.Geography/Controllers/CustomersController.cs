using AspNetClassic.Geography.Models;
using Microsoft.AspNet.OData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace AspNetClassic.Geography.Controllers
{
    public class CustomersController : ODataController
    {
        private CustomerGeoContext db = new CustomerGeoContext();

        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(db.Customers);
        }

        public IHttpActionResult Get(int key)
        {
            Customer customer = db.Customers.First(c => c.Id == key);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }
    }
}