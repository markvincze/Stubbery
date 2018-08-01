using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Stubbery.IntegrationTests
{
    public class BodyConditionTest
    {
        private readonly HttpClient httpClient = new HttpClient();

        [Fact]
        public async Task IfBody_BodyIsDifferent_NotFoundReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Post("/testpost", (req, args) => "testresponse")
                    .IfBody(body => body.Contains("testpost"));
                
                sut.Start();

                var result = await httpClient.PostAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/testpost" }.Uri,
                    new StringContent("testrequest"));
                
                Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            }
        }
        
        [Fact]
        public async Task IfBody_BodyIsEqual_ResultReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Post("/testpost", (req, args) => "testresponse")
                    .IfBody(body => body.Contains("testpost"));
                
                sut.Start();

                var result = await httpClient.PostAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/testpost" }.Uri,
                    new StringContent("testpost"));

                var resultString = await result.Content.ReadAsStringAsync();
                
                Assert.Equal(HttpStatusCode.OK, result.StatusCode);
                Assert.Equal("testresponse", resultString);
            }
        }
        
        [Fact]
        public async Task IfBody_MultipleBodyOneIsEqual_ResultReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Post("/testpost", (req, args) => "testresponse")
                    .IfBody(body => body.Contains("testpost"))
                    .IfBody(body => body.Contains("posttest"));
                
                sut.Start();

                var result = await httpClient.PostAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/testpost" }.Uri,
                    new StringContent("posttest"));

                var resultString = await result.Content.ReadAsStringAsync();
                
                Assert.Equal(HttpStatusCode.OK, result.StatusCode);
                Assert.Equal("testresponse", resultString);
            }
        }
    }
}