using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Stubbery.IntegrationTests
{
    public class QueryArgConditionTest
    {
        private readonly HttpClient httpClient = new HttpClient();

        [Fact]
        public async Task QueryArg_QueryArgDifferent_NotFoundReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Request(HttpMethod.Get)
                    .IfQueryArg("testArg", "testValue")
                    .Response((req, args) => "testresponse");

                sut.Start();

                var result = await httpClient.GetAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/test", Query = "testArg=testValue2"}.Uri);

                Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            }
        }

        [Fact]
        public async Task QueryArg_QueryArgSame_ResultReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Request(HttpMethod.Get)
                    .IfQueryArg("testArg", "testValue")
                    .Response((req, args) => "testresponse");

                sut.Start();

                var result = await httpClient.GetAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/test", Query = "testArg=testValue"}.Uri);
                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);
                Assert.Equal("testresponse", resultString);
            }
        }

        [Fact]
        public async Task QueryArg_ValueContainsCharacterToEncode_DecodedStringProperlyMatched()
        {
            using (var sut = new ApiStub())
            {
                sut.Request(HttpMethod.Get)
                    .IfQueryArg("testArg", "test value")
                    .Response((req, args) => "testresponse");

                sut.Start();

                var result = await httpClient.GetAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/test", Query = "testArg=test%20value"}.Uri);
                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);
                Assert.Equal("testresponse", resultString);
            }
        }

        [Fact]
        public async Task QueryArg_MultipleValues_FirstValueMatchedCorrectly()
        {
            using (var sut = new ApiStub())
            {
                sut.Request(HttpMethod.Get)
                    .IfQueryArg("testArg", "value1")
                    .Response((req, args) => "testresponse");

                sut.Start();

                var result = await httpClient.GetAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/test", Query = "testArg=value1&testArg=value2"}.Uri);
                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);
                Assert.Equal("testresponse", resultString);
            }
        }


        [Fact]
        public async Task QueryArg_MultipleValues_SecondValueMatchedCorrectly()
        {
            using (var sut = new ApiStub())
            {
                sut.Request(HttpMethod.Get)
                    .IfQueryArg("testArg", "value2")
                    .Response((req, args) => "testresponse");

                sut.Start();

                var result = await httpClient.GetAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/test", Query = "testArg=value1&testArg=value2"}.Uri);
                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);
                Assert.Equal("testresponse", resultString);
            }
        }
    }
}
