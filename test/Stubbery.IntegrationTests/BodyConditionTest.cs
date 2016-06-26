using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Stubbery;
using Xunit;

namespace Stubbery.IntegrationTests
{
    public class BodyConditionTest
    {
        private readonly HttpClient httpClient = new HttpClient();

        [Fact]
        public async Task IfBody_BodyDifferent_NotFoundReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Post("/testget", (req, args) => "testresponse")
                    .IfBody(s => s.ReadAsString().Contains("bodyCondition"));

                sut.Start();

                var result = await httpClient.PostAsync(
                    new UriBuilder(new Uri(sut.Address)) { Path = "/testget" }.Uri,
                    new StringContent(""));

                Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            }
        }

        [Fact]
        public async Task IfBody_BodyEqual_ResultReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Post("/testget", (req, args) => "testresponse")
                    .IfBody(s => s.ReadAsString().Contains("bodyCondition"));

                sut.Start();

                var result = await httpClient.PostAsync(
                    new UriBuilder(new Uri(sut.Address)) { Path = "/testget" }.Uri,
                    new StringContent("content bodyCondition content"));

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);

                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal("testresponse", resultString);
            }
        }

        [Fact]
        public async Task IfBody_BodyEqual_BodyCanBeUsedInTheResponderToo()
        {
            using (var sut = new ApiStub())
            {
                sut.Post("/testget", (req, args) => "testresponse" + args.Body.ReadAsString())
                    .IfBody(s => s.ReadAsString().Contains("bodyContent"));

                sut.Start();

                var result = await httpClient.PostAsync(
                    new UriBuilder(new Uri(sut.Address)) { Path = "/testget" }.Uri,
                    new StringContent("bodyContent"));

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);

                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal("testresponsebodyContent", resultString);
            }
        }
    }
}