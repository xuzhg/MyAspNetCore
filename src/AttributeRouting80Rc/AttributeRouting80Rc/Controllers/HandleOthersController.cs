// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Attributes;

namespace AttributeRouting80Rc.Controllers
{
    public class HandleOthersController : Controller
    {
        [ODataRouting]
        [HttpGet("odata/Orders/{key}")]
        public IActionResult Get(int key)
        {
            return Ok($"Orders{key} from OData");
        }

        [HttpGet("odata/Orders({key})")]
        public IActionResult GetOrder(int key)
        {
            return Ok($"Orders{key} from non-OData");
        }
    }
}
