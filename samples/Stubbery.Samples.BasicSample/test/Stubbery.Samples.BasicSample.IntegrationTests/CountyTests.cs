using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Stubbery.Samples.BasicSample.Web;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Stubbery.Samples.BasicSample.IntegrationTests
{
    public class CountyTests
    {
        private readonly TestServer server;

        private readonly ApiStub postcodeApiStub;

        private IHostingEnvironment CreateHostingEnvironment()
        {
            var hostingEnvironment = new HostingEnvironment();

            var appEnvironment = PlatformServices.Default.Application;

            var applicationName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            hostingEnvironment.Initialize(applicationName, appEnvironment.ApplicationBasePath, new WebHostOptions());

            return hostingEnvironment;
        }

        public CountyTests()
        {
            postcodeApiStub = new ApiStub();

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

            var hostingEnv = CreateHostingEnvironment();
            var loggerFactory = new LoggerFactory();

            var startup = new Startup(hostingEnv);

            server = new TestServer(new WebHostBuilder()
                .Configure(app => startup.Configure(app, hostingEnv, loggerFactory))
                .ConfigureServices(
                    services =>
                    {
                        startup.ConfigureServices(services);

                        // Replace the real api URL with the stub.
                        startup.Configuration["PostCodeApiUrl"] = postcodeApiStub.Address;
                    }));
        }

        [Fact]
        public async Task GetCountyName_CountyFound_CountyNameReturned()
        {
            using (var client = server.CreateClient().AcceptJson())
            {
                var response = await client.GetAsync("/countyname/postcodeOk");

                var responseString = await response.Content.ReadAsStringAsync();

                Assert.Equal("CountyName", responseString);
            }
        }

        [Fact]
        public async Task GetCountyName_CountyNotFound_NotFoundReturned()
        {
            using (var client = server.CreateClient().AcceptJson())
            {
                var response = await client.GetAsync("/countyname/postcodeNotFound");

                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }

        [Fact]
        public async Task GetCountyName_Error_InternalServerErrorReturned()
        {
            using (var client = server.CreateClient().AcceptJson())
            {
                var response = await client.GetAsync("/countyname/postcodeError");

                Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            }
        }
    }
}