using Microsoft.AspNetCore.Routing;

namespace Stubbery.RequestMatching
{
    internal interface IRouteMatcher
    {
        RouteValueDictionary Match(string routeTemplate, string requestPath);
    }
}