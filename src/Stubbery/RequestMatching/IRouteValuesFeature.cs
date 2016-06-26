using Microsoft.AspNetCore.Routing;

namespace Stubbery.RequestMatching
{
    internal interface IRouteValuesFeature
    {
        RouteValueDictionary RouteValues { get; }
    }
}