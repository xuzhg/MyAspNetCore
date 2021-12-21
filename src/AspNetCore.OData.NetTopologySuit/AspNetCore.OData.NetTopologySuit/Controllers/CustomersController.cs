using AspNetCore.OData.NetTopologySuit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.OData.NetTopologySuit.Controllers
{
    public class CustomersController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new Customer
            {
                Id = index,
                Name = Summaries[index],
                Location = new Point(-122.333056, 47.609722) { SRID = 4326 }
            })
            .ToArray();
        }
    }
}
