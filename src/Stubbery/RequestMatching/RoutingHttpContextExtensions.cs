using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Stubbery.RequestMatching
{
    /// <summary>
    /// Provides convenient extensions to access route arguments.
    /// </summary>
    public static class RoutingHttpContextExtensions
    {
        /// <summary>
        /// Returns the route arguments extracted during matching a <see cref="RouteCondition" />.
        /// </summary>
        /// <param name="httpContext">The HTTP context to extract the arguments from.</param>
        /// <returns>The extracted route arguments.</returns>
        public static RouteValueDictionary GetRouteValues(this HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            // If a route is successfully matched, the extracted arguments are added to the Features collection.
            var routingFeature = httpContext.Features[typeof(IRouteValuesFeature)] as IRouteValuesFeature;
            return routingFeature?.RouteValues;
        }
    }
}