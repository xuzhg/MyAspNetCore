// Copyright saxu@microsoft.com.  All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Abstracts;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.OData.Routing;
using Microsoft.AspNetCore.OData.Routing.Template;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.Extensions.Options;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;

namespace ODataApiVersion.Extensions
{
    internal class MyODataRoutingMatcherPolicy : MatcherPolicy, IEndpointSelectorPolicy
    {
        private readonly IODataTemplateTranslator _translator;
        private readonly IODataModelProvider _provider;
        private readonly ODataOptions _options;

        public MyODataRoutingMatcherPolicy(IODataTemplateTranslator translator,
            IODataModelProvider provider,
            IOptions<ODataOptions> options)
        {
            _translator = translator;
            _provider = provider;
            _options = options.Value;
        }

        public override int Order => 900 - 1; // minus 1 to make sure it's running before built-in OData matcher policy

        public bool AppliesToEndpoints(IReadOnlyList<Endpoint> endpoints)
        {
            return endpoints.Any(e => e.Metadata.OfType<ODataRoutingMetadata>().FirstOrDefault() != null);
        }

        public Task ApplyAsync(HttpContext httpContext, CandidateSet candidates)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            IODataFeature odataFeature = httpContext.ODataFeature();
            if (odataFeature.Path != null)
            {
                // If we have the OData path setting, it means there's some Policy working.
                // Let's skip this default OData matcher policy.
                return Task.CompletedTask;
            }

            for (var i = 0; i < candidates.Count; i++)
            {
                ref CandidateState candidate = ref candidates[i];
                if (!candidates.IsValidCandidate(i))
                {
                    continue;
                }

                IODataRoutingMetadata metadata = candidate.Endpoint.Metadata.OfType<IODataRoutingMetadata>().FirstOrDefault();
                if (metadata == null)
                {
                    continue;
                }

                // Get api-version query from HttpRequest?
                QueryStringApiVersionReader reader = new QueryStringApiVersionReader("api-version");
                string apiVersionStr = reader.Read(httpContext.Request);
                if (apiVersionStr == null)
                {
                    candidates.SetValidity(i, false);
                    continue;
                }
                ApiVersion apiVersion = ApiVersion.Parse(apiVersionStr);

                IEdmModel model = GetEdmModel(apiVersion);
                if (model == null)
                {
                    candidates.SetValidity(i, false);
                    continue;
                }

                if (!IsApiVersionMatch(candidate.Endpoint.Metadata, apiVersion))
                {
                    candidates.SetValidity(i, false);
                    continue;
                }

                ODataTemplateTranslateContext translatorContext
                    = new ODataTemplateTranslateContext(httpContext, candidate.Endpoint, candidate.Values, model);

                try
                {
                    ODataPath odataPath = _translator.Translate(metadata.Template, translatorContext);
                    if (odataPath != null)
                    {
                        odataFeature.RoutePrefix = metadata.Prefix;
                        odataFeature.Model = model;
                        odataFeature.Path = odataPath;

                        ODataOptions options = new ODataOptions();
                        UpdateQuerySetting(options);
                        options.AddRouteComponents(model);
                        odataFeature.Services = options.GetRouteServices(string.Empty);

                        MergeRouteValues(translatorContext.UpdatedValues, candidate.Values);
                    }
                    else
                    {
                        candidates.SetValidity(i, false);
                    }
                }
                catch
                {
                    candidates.SetValidity(i, false);
                }
            }

            return Task.CompletedTask;
        }

        private void UpdateQuerySetting(ODataOptions options)
        {
            options.QuerySettings.EnableSelect = _options.QuerySettings.EnableSelect;
            options.QuerySettings.EnableCount = _options.QuerySettings.EnableCount;
            options.QuerySettings.EnableExpand = _options.QuerySettings.EnableExpand;
            options.QuerySettings.EnableFilter = _options.QuerySettings.EnableFilter;
            options.QuerySettings.EnableOrderBy = _options.QuerySettings.EnableOrderBy;
            options.QuerySettings.EnableSkipToken = _options.QuerySettings.EnableSkipToken;
            options.QuerySettings.MaxTop = _options.QuerySettings.MaxTop;
        }

        private static void MergeRouteValues(RouteValueDictionary updates, RouteValueDictionary source)
        {
            foreach (var data in updates)
            {
                source[data.Key] = data.Value;
            }
        }

        private IEdmModel GetEdmModel(ApiVersion apiVersion)
        {
            return _provider.GetEdmModel(apiVersion.ToString());
        }

        private static bool IsApiVersionMatch(EndpointMetadataCollection metadata, ApiVersion apiVersion)
        {
            var apiVersions = metadata.OfType<ApiVersionAttribute>().ToArray();
            if (apiVersions.Length == 0)
            {
                // If no [ApiVersion] on the controller,
                // Let's simply return true, it means it can work the input version or any version.
                return true;
            }

            foreach (var item in apiVersions)
            {
                if (item.Versions.Contains(apiVersion))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
