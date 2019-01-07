using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using Moq;
using Stubbery.RequestMatching;
using Xunit;

namespace Stubbery.IntegrationTests
{
    public class RoutingHttpContextExtensionsTest
    {
        [Fact]
        public void GetRouteValues_CalledOnNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ((HttpContext) null).GetRouteValues());
        }

        [Fact]
        public void GetRouteValues_NoRoutesValuesFeature_EmptyRouteValueDictionaryReturned()
        {
            var httpContext = new Mock<HttpContext>();
            var features = new Mock<IFeatureCollection>();

            features.SetupGet(f => f[typeof(IRouteValuesFeature)]).Returns(null);

            httpContext.SetupGet(h => h.Features).Returns(features.Object);

            var result = httpContext.Object.GetRouteValues();

            Assert.Empty(result);
        }

        [Fact]
        public void GetRouteValues_RoutesValuesFeatureAvailable_RoutesValuesFeatureReturned()
        {
            var httpContext = new Mock<HttpContext>();
            var features = new Mock<IFeatureCollection>();
            var routeValues = new Mock<RouteValueDictionary>();
            var routeValuesFeature = new DummRouteValuesFeature(routeValues.Object);

            features.SetupGet(f => f[typeof(IRouteValuesFeature)]).Returns(routeValuesFeature);

            httpContext.SetupGet(h => h.Features).Returns(features.Object);

            var result = httpContext.Object.GetRouteValues();

            Assert.Same(routeValues.Object, result);
        }
    }

    public class DummRouteValuesFeature : IRouteValuesFeature
    {
        public DummRouteValuesFeature(RouteValueDictionary dictionary)
        {
            RouteValues = dictionary;
        }

        public RouteValueDictionary RouteValues { get; }
    }
}