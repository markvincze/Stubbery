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
        public void GetRouteValues_NoRoutesValuesFeature_NullReturned()
        {
            var httpContext = new Mock<HttpContext>();
            var features = new Mock<IFeatureCollection>();

            features.SetupGet(f => f[typeof(IRouteValuesFeature)]).Returns(null);

            httpContext.SetupGet(h => h.Features).Returns(features.Object);

            var result = httpContext.Object.GetRouteValues();

            Assert.Null(result);
        }

        [Fact]
        public void GetRouteValues_RoutesValuesFeatureAvailable_RoutesValuesFeatureReturned()
        {
            var httpContext = new Mock<HttpContext>();
            var features = new Mock<IFeatureCollection>();
            var routeValues = new Mock<RouteValueDictionary>();
            var routeValuesFeature = new DummyRouteValuesFeature(routeValues.Object);

            features.SetupGet(f => f[typeof(IRouteValuesFeature)]).Returns(routeValuesFeature);

            httpContext.SetupGet(h => h.Features).Returns(features.Object);

            var result = httpContext.Object.GetRouteValues();

            Assert.Same(routeValues.Object, result);
        }
    }

    public class DummyRouteValuesFeature : IRouteValuesFeature
    {
        public DummyRouteValuesFeature(RouteValueDictionary dictionary)
        {
            RouteValues = dictionary;
        }

        public RouteValueDictionary RouteValues { get; }
    }
}