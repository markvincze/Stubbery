using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Stubbery.IntegrationTests
{
    public class AcceptConditionTest
    {
        private readonly HttpClient httpClient = new HttpClient();

        [Fact]
        public async Task IfAccept_AcceptDifferent_NotFoundReturned()
        {
            using var sut = new ApiStub();

            sut.Get("/testget", (req, args) => "testresponse")
                .IfAccept("custom/stubbery");

            sut.Start();

            var result = await httpClient.GetAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/testget" }.Uri);

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task IfAccept_AcceptEqual_ResultReturned()
        {
            using var sut = new ApiStub();

            sut.Get("/testget", (req, args) => "testresponse")
                .IfAccept("custom/stubbery");

            sut.Start();

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, new UriBuilder(new Uri(sut.Address)) {Path = "/testget"}.Uri);
            requestMessage.Headers.Add("Accept", "custom/stubbery");

            var result = await httpClient.SendAsync(requestMessage);

            var resultString = await result.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("testresponse", resultString);
        }

        [Fact]
        public async Task IfAccept_MultipleAcceptsOneIsEqual_ResultReturned()
        {
            using var sut = new ApiStub();

            sut.Get("/testget", (req, args) => "testresponse")
                .IfAccept("custom/stubbery")
                .IfAccept("custom/stubbery2");

            sut.Start();

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, new UriBuilder(new Uri(sut.Address)) {Path = "/testget"}.Uri);
            requestMessage.Headers.Add("Accept", "custom/stubbery");

            var result = await httpClient.SendAsync(requestMessage);

            var resultString = await result.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("testresponse", resultString);
        }

        [Fact]
        public async Task IfAccept_CustomNotMatchingAcceptCondition_NotFoundReturned()
        {
            using var sut = new ApiStub();

            sut.Get("/testget", (req, args) => "testresponse")
                .IfAccept(accept => accept.Contains("does not contain"));

            sut.Start();

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, new UriBuilder(new Uri(sut.Address)) {Path = "/testget"}.Uri);
            requestMessage.Headers.Add("Accept", "custom/stubbery");

            var result = await httpClient.SendAsync(requestMessage);

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task IfAccept_CustomMatchingAcceptCondition_ResultReturned()
        {
            using var sut = new ApiStub();

            sut.Get("/testget", (req, args) => "testresponse")
                .IfAccept(accept => accept.Contains("stubbery"));

            sut.Start();

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, new UriBuilder(new Uri(sut.Address)) {Path = "/testget"}.Uri);
            requestMessage.Headers.Add("Accept", "custom/stubbery");

            var result = await httpClient.SendAsync(requestMessage);

            var resultString = await result.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("testresponse", resultString);
        }
    }
}