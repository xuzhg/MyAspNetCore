using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.AspNetCore.OData.Formatter.Value;
using Microsoft.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;

namespace OmitNullPropertySample.Extensions
{
    public class OmitNullResourceSerializer : ODataResourceSerializer
    {
        public OmitNullResourceSerializer(IODataSerializerProvider serializerProvider) : base(serializerProvider)
        {
        }

        public override ODataProperty CreateStructuralProperty(IEdmStructuralProperty structuralProperty, ResourceContext resourceContext)
        {
            bool isOmitNulls = resourceContext.Request.IsOmitNulls();
            if (isOmitNulls)
            {
                object propertyValue = resourceContext.GetPropertyValue(structuralProperty.Name);
                if (propertyValue == null)
                {
                    // it MUST specify the Preference-Applied response header with omit-values=nulls
                    resourceContext.Request.SetPreferenceAppliedResponseHeader();
                    return null;
                }
            }

            return base.CreateStructuralProperty(structuralProperty, resourceContext);
        }

        public override ODataNestedResourceInfo CreateComplexNestedResourceInfo(IEdmStructuralProperty complexProperty, PathSelectItem pathSelectItem, ResourceContext resourceContext)
        {
            bool isOmitNulls = resourceContext.Request.IsOmitNulls();
            if (isOmitNulls)
            {
                object propertyValue = resourceContext.GetPropertyValue(complexProperty.Name);
                if (propertyValue == null || propertyValue is NullEdmComplexObject)
                {
                    resourceContext.Request.SetPreferenceAppliedResponseHeader();
                    return null;
                }
            }

            return base.CreateComplexNestedResourceInfo(complexProperty, pathSelectItem, resourceContext);
        }

        public override ODataNestedResourceInfo CreateNavigationLink(IEdmNavigationProperty navigationProperty, ResourceContext resourceContext)
        {
            bool isOmitNulls = resourceContext.Request.IsOmitNulls();
            if (isOmitNulls)
            {
                object propertyValue = resourceContext.GetPropertyValue(navigationProperty.Name);
                if (propertyValue == null || propertyValue is NullEdmComplexObject)
                {
                    resourceContext.Request.SetPreferenceAppliedResponseHeader();
                    return null;
                }
            }

            return base.CreateNavigationLink(navigationProperty, resourceContext);
        }
    }
}
