using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Stubbery.IntegrationTests
{
    public class ResponseParametersTest
    {
        private readonly HttpClient httpClient = new HttpClient();

        [Fact]
        public void ResponseBody_NullPassed_ArgumentNullException()
        {
            using (var sut = new ApiStub())
            {
                Assert.Throws<ArgumentNullException>(() => sut.Get().Response(null));
            }
        }

        [Fact]
        public void ResponseBody_CalledTwice_InvalidOperationException()
        {
            using (var sut = new ApiStub())
            {
                Assert.Throws<InvalidOperationException>(
                    () => sut.Get().Response((req, args) => "1").Response((req, args) => "2"));
            }
        }

        [Fact]
        public async Task ResponseBody_BodySet_BodyReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Get("/testget", (req, args) => "testresponse");

                sut.Start();

                var result = await httpClient.GetAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/testget" }.Uri);

                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal("testresponse", resultString);
            }
        }

        [Fact]
        public async Task StatusCode_StatusCodeValueSet_StatusCodeReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Get("/testget", (req, args) => "testresponse")
                    .StatusCode(StatusCodes.Status206PartialContent);

                sut.Start();

                var result = await httpClient.GetAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/testget" }.Uri);

                Assert.Equal(HttpStatusCode.PartialContent, result.StatusCode);
            }
        }

        [Fact]
        public async Task StatusCode_StatusCodeProviderSet_StatusCodeReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Get("/testget", (req, args) => "testresponse")
                    .StatusCode((req, args) => args.Query.testquery == "Success" ? StatusCodes.Status200OK : StatusCodes.Status500InternalServerError);

                sut.Start();

                var resultSuccess = await httpClient.GetAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/testget", Query = "?testquery=Success"}.Uri);

                Assert.Equal(HttpStatusCode.OK, resultSuccess.StatusCode);

                var resultFailure = await httpClient.GetAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/testget", Query = "?testquery=Failure"}.Uri);

                Assert.Equal(HttpStatusCode.InternalServerError, resultFailure.StatusCode);
            }
        }

        [Fact]
        public async Task Header_HeadersAdded_HeadersReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Get("/testget", (req, args) => "testresponse")
                    .Header("Header1", "HeaderValue1")
                    .Header("Header2", "HeaderValue2");

                sut.Start();

                var result = await httpClient.GetAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/testget" }.Uri);

                Assert.Equal("HeaderValue1", result.Headers.GetValues("Header1").First());
                Assert.Equal("HeaderValue2", result.Headers.GetValues("Header2").First());
            }
        }

        [Fact]
        public async Task Headers_HeadersAdded_HeadersReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Get("/testget", (req, args) => "testresponse")
                    .Headers(new KeyValuePair<string, string>("Header1", "HeaderValue1"), new KeyValuePair<string, string>("Header2", "HeaderValue2"));

                sut.Start();

                var result = await httpClient.GetAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/testget" }.Uri);

                Assert.Equal("HeaderValue1", result.Headers.GetValues("Header1").First());
                Assert.Equal("HeaderValue2", result.Headers.GetValues("Header2").First());
            }
        }
    }
}