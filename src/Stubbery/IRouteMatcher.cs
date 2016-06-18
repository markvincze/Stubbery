using Microsoft.AspNetCore.Routing;

namespace Stubbery
{
    internal interface IRouteMatcher
    {
        RouteValueDictionary Match(string routeTemplate, string requestPath);
    }
}