using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Stubbery.IntegrationTests
{
    public class HeaderConditionTest
    {
        private readonly HttpClient httpClient = new HttpClient();

        [Fact]
        public async Task IfHeader_OneHeaderConditionAndHeaderNotPresent_NotFoundReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Get("/testget", (req, args) => "testresponse")
                    .IfHeader("HeaderTest", "headerValue");

                sut.Start();

                var result = await httpClient.GetAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/testget" }.Uri);

                Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            }
        }

        [Fact]
        public async Task IfHeader_OneHeaderConditionAndHeaderPresentWithWrongValue_NotFoundReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Get(
                    "/testget",
                    (req, args) => "testresponse")
                    .IfHeader("HeaderTest", "headerValue");

                sut.Start();

                var requestMessage = new HttpRequestMessage(HttpMethod.Get, new UriBuilder(new Uri(sut.Address)) {Path = "/testget"}.Uri);
                requestMessage.Headers.Add("HeaderTest", "wrongValue");

                var result = await httpClient.SendAsync(requestMessage);

                Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            }
        }

        [Fact]
        public async Task IfHeader_OneHeaderConditionAndHeaderPresent_ResponseReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Get(
                    "/testget",
                    (req, args) => "testresponse")
                    .IfHeader("HeaderTest", "headerValue");

                sut.Start();

                var requestMessage = new HttpRequestMessage(HttpMethod.Get, new UriBuilder(new Uri(sut.Address)) {Path = "/testget"}.Uri);
                requestMessage.Headers.Add("HeaderTest", "headerValue");

                var result = await httpClient.SendAsync(requestMessage);

                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal("testresponse", resultString);
            }
        }

        [Fact]
        public async Task IfHeader_TwoHeaderConditionsAndOnlyOneHeaderPresent_NotFoundReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Get(
                    "/testget",
                    (req, args) => "testresponse")
                    .IfHeader("HeaderTest1", "headerValue1")
                    .IfHeader("HeaderTest2", "headerValue2");

                sut.Start();

                var requestMessage = new HttpRequestMessage(HttpMethod.Get, new UriBuilder(new Uri(sut.Address)) {Path = "/testget"}.Uri);
                requestMessage.Headers.Add("HeaderTest1", "headerValue1");

                var result = await httpClient.SendAsync(requestMessage);

                Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            }
        }

        [Fact]
        public async Task IfHeader_TwoHeaderConditionsAndBothHeadersPresent_ResponseReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Get(
                    "/testget",
                    (req, args) => "testresponse")
                    .IfHeader("HeaderTest1", "headerValue1")
                    .IfHeader("HeaderTest2", "headerValue2");

                sut.Start();

                var requestMessage = new HttpRequestMessage(HttpMethod.Get, new UriBuilder(new Uri(sut.Address)) {Path = "/testget"}.Uri);
                requestMessage.Headers.Add("HeaderTest1", "headerValue1");
                requestMessage.Headers.Add("HeaderTest2", "headerValue2");

                var result = await httpClient.SendAsync(requestMessage);

                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal("testresponse", resultString);
            }
        }

        [Fact]
        public async Task IfHeaders_CustomTwoHeaderConditionsAndHeadersNotPresent_NotFoundReturned()
        {
            using (var sut = new ApiStub())
            {
                sut
                    .Get("/testget", (req, args) => "testresponse")
                    .IfHeaders(headers => headers["HeaderTest1"] == "headerValue1" && headers["HeaderTest2"] == "headerValue2");

                sut.Start();

                var requestMessage = new HttpRequestMessage(HttpMethod.Get, new UriBuilder(new Uri(sut.Address)) {Path = "/testget"}.Uri);
                requestMessage.Headers.Add("HeaderTest1", "headerValue1");

                var result = await httpClient.SendAsync(requestMessage);

                Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            }
        }

        [Fact]
        public async Task IfHeaders_CustomTwoHeaderConditionsAndBothHeadersPresent_ResponseReturned()
        {
            using (var sut = new ApiStub())
            {
                sut
                    .Get("/testget", (req, args) => "testresponse")
                    .IfHeaders(headers => headers["HeaderTest1"] == "headerValue1" && headers["HeaderTest2"] == "headerValue2");

                sut.Start();

                var requestMessage = new HttpRequestMessage(HttpMethod.Get, new UriBuilder(new Uri(sut.Address)) {Path = "/testget"}.Uri);
                requestMessage.Headers.Add("HeaderTest1", "headerValue1");
                requestMessage.Headers.Add("HeaderTest2", "headerValue2");

                var result = await httpClient.SendAsync(requestMessage);

                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal("testresponse", resultString);
            }
        }
    }
}