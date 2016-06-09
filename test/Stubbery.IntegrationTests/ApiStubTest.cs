using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Stubbery.IntegrationTests
{
    public class ApiStubTest
    {
        private readonly HttpClient httpClient = new HttpClient();

        [Fact]
        public async Task Get_CalledSetupRoute_SetupResponseReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Get(
                    "/testget",
                    req => "testresponse");

                sut.Start();

                var result = await httpClient.GetAsync(
                    new UriBuilder(new Uri(sut.Address)) { Path = "/testget" }.Uri);

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);

                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal("testresponse", resultString);
            }
        }

        [Fact]
        public async Task Get_CalledDifferentRoute_NotFoundReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Get(
                    "/testget",
                    req => "testresponse");

                sut.Start();

                var result = await httpClient.GetAsync(
                    new UriBuilder(new Uri(sut.Address)) { Path = "/differentroute" }.Uri);

                Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            }
        }

        [Fact]
        public async Task Get_CalledDifferentMethod_NotFoundReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Get(
                    "/testget",
                    req => "testresponse");

                sut.Start();

                var result = await httpClient.DeleteAsync(
                    new UriBuilder(new Uri(sut.Address)) { Path = "/differentroute" }.Uri);

                Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            }
        }
    }
}