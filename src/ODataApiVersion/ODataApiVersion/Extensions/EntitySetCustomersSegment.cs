// Copyright saxu@microsoft.com.  All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.OData.Routing;
using Microsoft.AspNetCore.OData.Routing.Template;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;

namespace ODataApiVersion.Extensions
{
    public class EntitySetCustomersSegment : ODataSegmentTemplate
    {
        public override IEnumerable<string> GetTemplates(ODataRouteOptions options)
        {
            yield return "/Customers";
        }

        public override bool TryTranslate(ODataTemplateTranslateContext context)
        {
            // Support case-insenstivie
            var edmEntitySet = context.Model.EntityContainer.EntitySets()
                .FirstOrDefault(e => string.Equals("Customers", e.Name, StringComparison.OrdinalIgnoreCase));

            if (edmEntitySet != null)
            {
                EntitySetSegment segment = new EntitySetSegment(edmEntitySet);
                context.Segments.Add(segment);
                return true;
            }

            return false;
        }
    }
}
