// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Attributes;

namespace AttributeRouting80Rc.Controllers
{
    [Route("v{version}")]
    public class HandAbolusteController : Controller
    {
        [ODataRouting]
        [HttpGet("/odata/orders({key})/SendTo(lat={lat},lon={lon})")]
        public string SendTo(int key, double lat, double lon)
        {
            return $"Send Order({key}) to location at ({lat},{lon})";
        }
    }
}
