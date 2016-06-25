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
        public async void ResponseBody_BodySet_BodyReturned()
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
        public async void StatusCode_StatusCodeSet_StatusCodeReturned()
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
        public async void Header_HeadersAdded_HeadersReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Get("/testget", (req, args) => "testresponse")
                    .Header("header1", "headerValue1")
                    .Header("header2", "headerValue2");

                sut.Start();

                var result = await httpClient.GetAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/testget" }.Uri);

                Assert.Equal("headerValue1", result.Headers.GetValues("header1").First());
                Assert.Equal("headerValue2", result.Headers.GetValues("header2").First());
            }
        }

        [Fact]
        public async void Headers_HeadersAdded_HeadersReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Get("/testget", (req, args) => "testresponse")
                    .Headers(new KeyValuePair<string, string>("header1", "headerValue1"), new KeyValuePair<string, string>("header2", "headerValue2"));

                sut.Start();

                var result = await httpClient.GetAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/testget" }.Uri);

                Assert.Equal("headerValue1", result.Headers.GetValues("header1").First());
                Assert.Equal("headerValue2", result.Headers.GetValues("header2").First());
            }
        }
    }
}