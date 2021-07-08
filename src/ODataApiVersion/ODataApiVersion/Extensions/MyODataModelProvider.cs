// Copyright saxu@microsoft.com.  All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace ODataApiVersion.Extensions
{
    public class MyODataModelProvider : IODataModelProvider
    {
        private IDictionary<string, IEdmModel> _cached = new Dictionary<string, IEdmModel>();

        public IEdmModel GetEdmModel(string apiVersion)
        {
            if (_cached.TryGetValue(apiVersion, out IEdmModel model))
            {
                return model;
            }

            model = BuildEdmModel(apiVersion);
            _cached[apiVersion] = model;
            return model;
        }

        private static IEdmModel BuildEdmModel(string version)
        {
            switch (version)
            {
                case "1.0":
                    return BuildV1Model();

                case "2.0":
                    return BuildV2Model();
            }

            throw new NotSupportedException($"The input version '{version}' is not supported!");
        }

        private static IEdmModel BuildV1Model()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Models.v1.Customer>("Customers");

            return builder.GetEdmModel();
        }

        private static IEdmModel BuildV2Model()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Models.v2.Customer>("Customers");

            return builder.GetEdmModel();
        }
    }
}
