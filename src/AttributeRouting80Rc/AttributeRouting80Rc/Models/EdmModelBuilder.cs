// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.

using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace AttributeRouting80Rc.Models
{
    public static class EdmModelBuilder
    {
        public static IEdmModel BuildCustomerModel()
        {
            ODataConventionModelBuilder builder = new();
            builder.EntitySet<Customer>("Customers");
            builder.EntitySet<Order>("Orders");

            var function = builder.EntityType<Customer>().Function("PlayPiano").Returns<string>();
            function.Parameter<int>("kind");
            function.Parameter<string>("name");

            ConfigOrder(builder);

            return builder.GetEdmModel();
        }

        public static IEdmModel BuildBookModel()
        {
            ODataConventionModelBuilder builder = new();
            builder.EntitySet<Book>("Books");
            builder.EntitySet<Press>("Presses");
            builder.EntitySet<Order>("Orders");

            ConfigOrder(builder);

            return builder.GetEdmModel();
        }

        private static void ConfigOrder(ODataConventionModelBuilder builder)
        {
            var function = builder.EntityType<Order>().Function("SendTo").Returns<string>();
            function.Parameter<double>("lat");
            function.Parameter<double>("lon");
        }
    }
}
