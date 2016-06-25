using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Stubbery.IntegrationTests
{
    public class ComplexSetupTest
    {
        private readonly HttpClient httpClient = new HttpClient();

        [Fact]
        public async Task MultipleSetups_OnlyOneMatch_TheMatchingReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Get("/testget1/{arg1}", (req, args) => $"testresponse,{args.Route.arg2}")
                    .IfHeader("Header1", "TestValue1")
                    .IfHeader("Header2", "TestValue2")
                    .IfContentType("custom/stubbery")
                    .IfAccept("custom/accept")
                    .IfRoute("/testget2/{arg2}")
                    .StatusCode(StatusCodes.Status300MultipleChoices)
                    .Header("ResponseHeader1", "ResponseValue1")
                    .Header("ResponseHeader2", "ResponseValue2");

                sut.Start();

                var requestMessage = new HttpRequestMessage(HttpMethod.Get, new UriBuilder(new Uri(sut.Address)) {Path = "/testget2/argValue"}.Uri);
                requestMessage.Headers.Add("Header1", "TestValue1");
                requestMessage.Headers.Add("Header2", "TestValue2");
                requestMessage.Content = new StringContent("", Encoding.UTF8, "custom/stubbery");
                requestMessage.Headers.Add("Accept", "custom/accept");

                var result = await httpClient.SendAsync(requestMessage);

                Assert.Equal(HttpStatusCode.MultipleChoices, result.StatusCode);

                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal("testresponse,argValue", resultString);

                Assert.Equal("ResponseValue1", result.Headers.GetValues("ResponseHeader1").First());
                Assert.Equal("ResponseValue2", result.Headers.GetValues("ResponseHeader2").First());
            }
        }
    }
}