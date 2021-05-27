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

        [Theory]
        [InlineData("/testget//two")]
        [InlineData(":hey/hello:8080?what")]
        public void MultipleSetups_OneHasWrongRoute_ThrowsException(string route)
        {
            using var sut = new ApiStub();

            Assert.Throws<ArgumentException>(() => sut.Get(route, 
                (req, args) => "doesn't match"));
        }
    }
}
