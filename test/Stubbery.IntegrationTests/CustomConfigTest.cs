using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Stubbery.IntegrationTests
{
    public class CustomConfigTest
    {
        public class CustomPort
        {
            private readonly HttpClient httpClient = new HttpClient();

            [Fact]
            public void Start_StartsStubWithCustomPort_WhenPortIsSet()
            {
                using (var apiStub = new ApiStub())
                {
                    apiStub.Port = 1010;

                    apiStub.Start();

                    var stubPort = apiStub.Address.Split(':')[2];
                    Assert.Equal(stubPort, "1010");
                }
            }

            [Fact]
            public void Port_ThrowsInvalidOperationException_WhenBeingSetAndStubAlreadyStarted()
            {
                using (var apiStub = new ApiStub())
                {
                    apiStub.Start();

                    Assert.Throws<InvalidOperationException>(() => apiStub.Port = 1234);
                }
            }

            [Fact]
            public async Task Get_CallsStubWithCustomPort_WhenCustomPortIsSet()
            {
                using (var apiStub = new ApiStub())
                {
                    apiStub.Get("/testget", (req, args) => "testresponse");
                    apiStub.Port = 1020;
                    apiStub.Start();

                    var result = await httpClient.GetAsync(new UriBuilder(new Uri(apiStub.Address)) { Path = "/testget" }.Uri);

                    Assert.Equal(HttpStatusCode.OK, result.StatusCode);

                    var resultString = await result.Content.ReadAsStringAsync();

                    Assert.Equal("testresponse", resultString);
                }
            }
        }
    }
}