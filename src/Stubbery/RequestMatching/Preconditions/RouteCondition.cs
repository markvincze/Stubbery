using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Stubbery.RequestMatching.Preconditions
{
    public class RouteCondition : IPrecondition
    {
        private readonly string route;

        public RouteCondition(string route)
        {
            this.route = route.TrimStart('/');
        }

        public bool Match(HttpContext context)
        {
            var routeMatcher = new RouteMatcher();

            var result = routeMatcher.Match(route, context.Request.Path);

            if (result != null)
            {
                context.Features[typeof(IRouteValuesFeature)] = new RouteValuesFeature(result);

                return true;
            }

            return false;
        }

        private class RouteValuesFeature : IRouteValuesFeature
        {
            public RouteValueDictionary RouteValues { get; }

            public RouteValuesFeature(RouteValueDictionary routeValues)
            {
                RouteValues = routeValues;
            }
        }
    }
}