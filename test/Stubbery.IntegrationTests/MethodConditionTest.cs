using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Stubbery.IntegrationTests
{
    public class MethodConditionTest
    {
        private readonly HttpClient httpClient = new HttpClient();

        [Fact]
        public async Task Method_MethodDifferent_NotFoundReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Get("/testget", (req, args) => "testresponse")
                    .IfAccept("custom/stubbery");

                sut.Start();

                var result = await httpClient.DeleteAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/testget" }.Uri);

                Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            }
        }

        [Fact]
        public async Task Method_MethodSame_ResultReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Delete("/testget", (req, args) => "testresponse");

                sut.Start();

                var result = await httpClient.DeleteAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/testget" }.Uri);

                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);
                Assert.Equal("testresponse", resultString);
            }
        }

        [Fact]
        public async Task Method_MultipleMethodsOneIsEqual_ResultReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Request(HttpMethod.Get, HttpMethod.Delete)
                    .IfRoute("/testget")
                    .Response((req, args) => "testresponse");

                sut.Start();

                var result = await httpClient.DeleteAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/testget" }.Uri);

                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);
                Assert.Equal("testresponse", resultString);
            }
        }
    }
}