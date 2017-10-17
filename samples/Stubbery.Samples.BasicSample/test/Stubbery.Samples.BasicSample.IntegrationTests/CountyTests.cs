using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Stubbery.Samples.BasicSample.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Configuration.Memory;
using Xunit;

namespace Stubbery.Samples.BasicSample.IntegrationTests
{
    public class CountyTests
    {
        private ApiStub StartStub()
        {
            var postcodeApiStub = new ApiStub();

            postcodeApiStub.Get(
                "/postcodes/{postcode}",
                (request, args) =>
                {
                    if (args.Route.postcode == "postcodeOk")
                    {
                        return "{ \"status\": 200, \"result\": { \"admin_county\": \"CountyName\" } }";
                    }

                    if (args.Route.postcode == "postcodeNotFound")
                    {
                        return "{ \"status\": 404 }";
                    }

                    return "{ \"status\": 500 }";
                });

            postcodeApiStub.Start();

            return postcodeApiStub;
        }

        private TestServer StartApiUnderTest(ApiStub postcodeApiStub)
        {
            var server = new TestServer(
                WebHost.CreateDefaultBuilder()
                    .UseStartup<Startup>()
                    .ConfigureAppConfiguration((ctx, b) =>
                    {
                        b.Add(new MemoryConfigurationSource
                        {
                            InitialData = new Dictionary<string, string>
                            {
                                // Replace the real api URL with the stub.
                                ["PostCodeApiUrl"] = postcodeApiStub.Address
                            }
                        });
                    }));

            return server;
        }

        [Fact]
        public async Task GetCountyName_CountyFound_CountyNameReturned()
        {
            using (var stub = StartStub())
            using (var server = StartApiUnderTest(stub))
            using (var client = server.CreateClient())
            {
                var response = await client.GetAsync("/countyname/postcodeOk");

                var responseString = await response.Content.ReadAsStringAsync();

                Assert.Equal("CountyName", responseString);
            }
        }

        [Fact]
        public async Task GetCountyName_CountyNotFound_NotFoundReturned()
        {
            using (var stub = StartStub())
            using (var server = StartApiUnderTest(stub))
            using (var client = server.CreateClient())
            {
                var response = await client.GetAsync("/countyname/postcodeNotFound");

                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }

        [Fact]
        public async Task GetCountyName_Error_InternalServerErrorReturned()
        {
            using (var stub = StartStub())
            using (var server = StartApiUnderTest(stub))
            using (var client = server.CreateClient())
            {
                var response = await client.GetAsync("/countyname/postcodeError");

                Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            }
        }
    }
}