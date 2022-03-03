using AspNetClassic.Geography.Models;
using Microsoft.AspNet.OData.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace AspNetClassic.Geography
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            CustomerGeoContext.Init();

            config.MapODataServiceRoute("odata", "odata", EdmModelBuilder.BuildCustomerEdmModel());
        }
    }
}
