using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Stubbery.IntegrationTests
{
    public class FormatterTest
    {
        [Fact]
        public async Task Formatter_NonStringResponseProvided_FormattedAsJson()
        {
            using (var sut = new ApiStub())
            using (var httpClient = new HttpClient())
            {
                sut.Get("/testget", (req, args) => new { foo = "bar" });

                sut.Start();

                var result = await httpClient.GetAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/testget" }.Uri);

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);

                var resultString = await result.Content.ReadAsStringAsync();
                var resultJson = JObject.Parse(resultString);

                Assert.Equal("bar", resultJson["foo"].Value<string>());
            }
        }

        class TestType
        {
            public string Foo { get; set; }
        }

        [Fact]
        public async Task Formatter_DefaultFormatterChanged_FormattedAsXml()
        {
            using (var sut = new ApiStub
            {
                //DefaultOutputFormatter = new XmlSerializerOutputFormatter()
                DefaultOutputFormatter = new XmlDataContractSerializerOutputFormatter()
            })
            using (var httpClient = new HttpClient())
            {
                sut.Get("/testget", (req, args) => new TestType { Foo = "bar" });

                sut.Start();

                var result = await httpClient.GetAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/testget" }.Uri);

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);

                var resultString = await result.Content.ReadAsStringAsync();
                var resultJson = JObject.Parse(resultString);

                Assert.Equal("bar", resultJson["foo"].Value<string>());
            }
        }
    }
}