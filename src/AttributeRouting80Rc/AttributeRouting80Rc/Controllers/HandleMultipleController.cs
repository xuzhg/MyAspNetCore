// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Attributes;

namespace AttributeRouting80Rc.Controllers
{
    [Route("odata")]
    [Route("v{version}")]
    public class HandleMultipleController : Controller
    {
        [ODataRouting]
        [HttpGet("orders")]
        public string Get(string version)
        {
            if (version != null)
            {
                return $"Orders from version = {version}";
            }
            else
            {
                return "Orders from odata";
            }
        }
    }
}
