using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Stubbery.IntegrationTests
{
    public class HeaderConditionsTest
    {
        private readonly HttpClient httpClient = new HttpClient();

        public async Task Get_OneHeaderCondition_ResponseOnlyReturnedIfHeaderPresent()
        {
            using (var sut = new ApiStub())
            {
                sut.Get(
                    "/testget",
                    (req, args) => "testresponse");

                sut.Start();

                var result = await httpClient.GetAsync(
                    new UriBuilder(new Uri(sut.Address)) { Path = "/testget" }.Uri);

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);

                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal("testresponse", resultString);
            }
        }
    }
}
