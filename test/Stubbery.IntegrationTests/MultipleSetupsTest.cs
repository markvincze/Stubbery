using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Stubbery.IntegrationTests
{
    public class MultipleSetupsTest
    {
        private readonly HttpClient httpClient = new HttpClient();

        [Fact]
        public async Task MultipleSetups_OnlyOneMatch_TheMatchingReturned()
        {
            using var sut = new ApiStub();

            sut.Get("/testget1", (req, args) => "testresponse1");
            sut.Get("/testget2", (req, args) => "testresponse2");
            sut.Get("/testget3", (req, args) => "testresponse3");

            sut.Start();

            var result = await httpClient.GetAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/testget2" }.Uri);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            var resultString = await result.Content.ReadAsStringAsync();

            Assert.Equal("testresponse2", resultString);
        }

        [Fact]
        public async Task MultipleSetups_AllMatch_TheFirstOneReturned()
        {
            using var sut = new ApiStub();

            sut.Get("/testget", (req, args) => "testresponse1");
            sut.Get("/testget", (req, args) => "testresponse2");
            sut.Get("/testget", (req, args) => "testresponse3");

            sut.Start();

            var result = await httpClient.GetAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/testget" }.Uri);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            var resultString = await result.Content.ReadAsStringAsync();

            Assert.Equal("testresponse1", resultString);
        }

        [Fact]
        public async Task MultipleSetups_OneHasWrongRoute_OthersStillMatch()
        {
            using var sut = new ApiStub();

            sut.Get("/testget/one", (req, args) => "testresponse1");
            sut.Get("/testget//two", (req, args) => "doesn't match");
            sut.Get("/testget/three", (req, args) => "testresponse3");

            sut.Start();

            var result3 = await httpClient.GetAsync(new UriBuilder(new Uri(sut.Address))
            {
                Path = "/testget/three"
            }.Uri);

            Assert.Equal(HttpStatusCode.OK, result3.StatusCode);

            var resultString = await result3.Content.ReadAsStringAsync();

            Assert.Equal("testresponse3", resultString);
        }
    }
}
