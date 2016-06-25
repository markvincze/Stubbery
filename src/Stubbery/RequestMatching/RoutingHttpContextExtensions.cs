using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Stubbery.RequestMatching
{
    public static class RoutingHttpContextExtensions
    {
        public static RouteValueDictionary GetRouteValues(this HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            var routingFeature = httpContext.Features[typeof(IRouteValuesFeature)] as IRouteValuesFeature;
            return routingFeature?.RouteValues;
        }
    }
}