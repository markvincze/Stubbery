using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Stubbery.IntegrationTests
{
    public class RouteConditionTest
    {
        private readonly HttpClient httpClient = new HttpClient();

        [Fact]
        public async Task Route_RouteDifferent_NotFoundReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Get("/testget", (req, args) => "testresponse");

                sut.Start();

                var result = await httpClient.GetAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/testget2" }.Uri);

                Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            }
        }

        [Fact]
        public async Task Route_RouteSame_ResultReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Get("/testget", (req, args) => "testresponse");

                sut.Start();

                var result = await httpClient.GetAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/testget" }.Uri);

                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);
                Assert.Equal("testresponse", resultString);
            }
        }

        [Fact]
        public async Task Route_MultipleRoutesOneIsEqual_ResultReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Request(HttpMethod.Get)
                    .IfRoute("/testget2")
                    .IfRoute("/testget")
                    .Response((req, args) => "testresponse");

                sut.Start();

                var result = await httpClient.GetAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/testget" }.Uri);

                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);
                Assert.Equal("testresponse", resultString);
            }
        }

        [Fact]
        public async Task Route_RouteIncludesMatchingQuery_ResultReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Get("/testget?foo=bar", (req, args) => "testresponse");

                sut.Start();

                var result = await httpClient.GetAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/testget", Query = "?foo=bar" }.Uri);

                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);
                Assert.Equal("testresponse", resultString);
            }
        }

        [Fact]
        public async Task Route_RouteIncludesNotMatchingQuery_NotFoundReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Get("/testget?foo=bar", (req, args) => "testresponse");

                sut.Start();

                var result = await httpClient.GetAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/testget", Query = "?foo=qux" }.Uri);

                Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            }
        }
    }
}