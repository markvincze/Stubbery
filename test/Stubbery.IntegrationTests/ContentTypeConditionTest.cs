using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Stubbery.IntegrationTests
{
    public class ContentTypeConditionTest
    {
        private readonly HttpClient httpClient = new HttpClient();

        [Fact]
        public async Task IfContentType_ContentTypeDifferent_NotFoundReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Get("/testget", (req, args) => "testresponse")
                    .IfContentType("custom/stubbery");

                sut.Start();

                var result = await httpClient.GetAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/testget" }.Uri);

                Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            }
        }

        [Fact]
        public async Task IfContentType_ContentTypeEqual_ResultReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Post("/testget", (req, args) => "testresponse")
                    .IfContentType("custom/stubbery");

                sut.Start();

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, new UriBuilder(new Uri(sut.Address)) {Path = "/testget"}.Uri)
                {
                    Content = new StringContent("", Encoding.UTF8, "custom/stubbery")
                };

                var result = await httpClient.SendAsync(requestMessage);

                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);
                Assert.Equal("testresponse", resultString);
            }
        }

        [Fact]
        public async Task IfContentType_MultipleContentTypesOneIsEqual_ResultReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Post("/testget", (req, args) => "testresponse")
                    .IfContentType("custom/stubbery")
                    .IfContentType("custom/stubbery2");

                sut.Start();

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, new UriBuilder(new Uri(sut.Address)) {Path = "/testget"}.Uri)
                {
                    Content = new StringContent("", Encoding.UTF8, "custom/stubbery")
                };

                var result = await httpClient.SendAsync(requestMessage);

                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);
                Assert.Equal("testresponse", resultString);
            }
        }

        [Fact]
        public async Task IfContentType_CustomNotMatchingFunc_NotFoundReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Post("/testget", (req, args) => "testresponse")
                    .IfContentType(contentType => contentType.Contains("does not contain"));

                sut.Start();

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, new UriBuilder(new Uri(sut.Address)) {Path = "/testget"}.Uri)
                {
                    Content = new StringContent("", Encoding.UTF8, "custom/stubbery")
                };

                var result = await httpClient.SendAsync(requestMessage);

                Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            }
        }

        [Fact]
        public async Task IfContentType_CustomMatchingFunc_ResponseReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Post("/testget", (req, args) => "testresponse")
                    .IfContentType(contentType => contentType.Contains("stubbery"));

                sut.Start();

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, new UriBuilder(new Uri(sut.Address)) {Path = "/testget"}.Uri)
                {
                    Content = new StringContent("", Encoding.UTF8, "custom/stubbery")
                };

                var result = await httpClient.SendAsync(requestMessage);

                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);
                Assert.Equal("testresponse", resultString);
            }
        }
    }
}