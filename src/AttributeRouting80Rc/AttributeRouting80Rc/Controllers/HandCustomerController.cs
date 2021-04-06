// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using AttributeRouting80Rc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.EntityFrameworkCore;

namespace AttributeRouting80Rc.Controllers
{
    [ODataRouting]
    [Route("v{version}")]
    public class HandleCustomerController : Controller
    {
        private readonly CustomerOrderContext _db;

        public HandleCustomerController(CustomerOrderContext context)
        {
            _db = context;
            _db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            if (context.Customers.Count() == 0)
            {
                foreach (var b in DataSource.GetCustomers())
                {
                    context.Customers.Add(b);
                    foreach (var order in b.Orders)
                    {
                        context.Orders.Add(order);
                    }
                }

                context.SaveChanges();
            }
        }

        [EnableQuery]
        [HttpGet("Customers")]
        [HttpGet("Customers/$count")]
        public IActionResult Get(string version)
        {
            // do something for the version
            return Ok(_db.Customers);
        }

        [HttpGet("Customers/{key}/Default.PlayPiano(kind={kind},name={name})")]
        [HttpGet("Customers/{key}/PlayPiano(kind={kind},name={name})")]
        public string LetUsPlayPiano(string version, int key, int kind, string name)
        {
            return $"[{version}], Customer {key} is playing Piano (kind={kind},name={name})";
        }
    }
}
