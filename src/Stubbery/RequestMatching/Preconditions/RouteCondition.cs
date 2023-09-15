﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;

namespace Stubbery.RequestMatching.Preconditions
{
    internal class RouteCondition : IPrecondition
    {
        private readonly string route;

        public RouteCondition(string route)
        {
            this.route = route.TrimStart('/');
            TemplateParser.Parse(route);
        }

        public async Task<bool> Match(HttpContext context)
        {
            var routeMatcher = new RouteMatcher();

            var result = routeMatcher.Match(route, context.Request.Path, context.Request.Query);

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