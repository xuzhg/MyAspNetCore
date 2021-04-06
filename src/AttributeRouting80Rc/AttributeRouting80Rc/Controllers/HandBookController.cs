// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.

using System.Linq;
using AttributeRouting80Rc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace AttributeRouting80Rc.Controllers
{
    public class HandleBookController : ODataController
    {
        private BookStoreContext _db;

        public HandleBookController(BookStoreContext context)
        {
            _db = context;
            _db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            if (context.Books.Count() == 0)
            {
                foreach (var b in DataSource.GetBooks())
                {
                    context.Books.Add(b);
                    context.Presses.Add(b.Press);
                }
                context.SaveChanges();
            }
        }

        [EnableQuery(PageSize = 1)]
        [HttpGet("odata/Books")]
        [HttpGet("odata/Books/$count")]
        public IActionResult Get()
        {
            return Ok(_db.Books);
        }

        [EnableQuery]
        [HttpGet("odata/Books({id})")]
        [HttpGet("odata/Books/{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_db.Books.FirstOrDefault(c => c.Id == id));
        }

        [Route("odata/Books({key})")]
        [HttpPatch]
        [HttpDelete]
        public IActionResult OperationBook(int key)
        {
            // the return is just for test.
            return Ok(_db.Books.FirstOrDefault(c => c.Id == key));
        }
    }
}
