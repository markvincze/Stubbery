using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Stubbery.IntegrationTests
{
    public class ArgumentsTest
    {
        private readonly HttpClient httpClient = new HttpClient();

        [Fact]
        public async Task Get_CalledSetupRoute_RouteDataAvailable()
        {
            using (var sut = new ApiStub())
            {
                sut.Get(
                    "/testget/{myArg}",
                    (req, args) => $"testresponse arg: {args.Route.myArg}");

                sut.Start();

                var result = await httpClient.GetAsync(
                    new UriBuilder(new Uri(sut.Address)) { Path = "/testget/orange" }.Uri);

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);

                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal("testresponse arg: orange", resultString);
            }
        }

        [Fact]
        public async Task Get_CalledQueryString_QueryParameterAvailable()
        {
            using (var sut = new ApiStub())
            {
                sut.Get(
                    "/testget",
                    (req, args) => $"testresponse arg: {args.Query.myArg}");

                sut.Start();

                var result = await httpClient.GetAsync(
                    new UriBuilder(new Uri(sut.Address)) { Path = "/testget", Query = "?myArg=orange" }.Uri);

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);

                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal("testresponse arg: orange", resultString);
            }
        }

        [Fact]
        public async Task Get_MultipleRouteAndQueryParameters_AllParameterAvailable()
        {
            using (var sut = new ApiStub())
            {
                sut.Get(
                    "/testget/{arg1}/part/{arg2}",
                    (req, args) => $"testresponse arg1: {args.Route.arg1} arg2: {args.Route.arg2} qarg1: {args.Query.qarg1} qarg2: {args.Query.qarg2}");

                sut.Start();

                var result = await httpClient.GetAsync(
                    new UriBuilder(new Uri(sut.Address)) { Path = "/testget/orange/part/apple", Query = "?qarg1=melon&qarg2=pear" }.Uri);

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);

                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal("testresponse arg1: orange arg2: apple qarg1: melon qarg2: pear", resultString);
            }
        }


        [Fact]
        public async Task Get_OptionalRouteParameter_ItCanBeOmitted()
        {
            using (var sut = new ApiStub())
            {
                sut.Get(
                    "/testget/{arg:string?}",
                    (req, args) => "testresponse");

                sut.Start();

                var result = await httpClient.GetAsync(
                    new UriBuilder(new Uri(sut.Address)) { Path = "/testget" }.Uri);

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);

                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal("testresponse", resultString);
            }
        }

        [Fact]
        public async Task Get_RouteParameterWithDefaultValue_DefaultValueReturned()
        {
            using (var sut = new ApiStub())
            {
                sut.Get(
                    "/testget/{arg:string=apple}",
                    (req, args) => $"testresponse arg: {args.Route.arg}");

                sut.Start();

                var result = await httpClient.GetAsync(
                    new UriBuilder(new Uri(sut.Address)) { Path = "/testget" }.Uri);

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);

                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal("testresponse arg: apple", resultString);
            }
        }

        [Fact]
        public async Task Post_BodyPassed_BodyAvailable()
        {
            using (var sut = new ApiStub())
            {
                sut.Post(
                    "/testpost",
                    (req, args) => $"testresponse body: {args.Body.ReadAsString()}");

                sut.Start();

                var result = await httpClient.PostAsync(
                    new UriBuilder(new Uri(sut.Address)) { Path = "/testpost" }.Uri,
                    new StringContent("orange"));

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);

                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal("testresponse body: orange", resultString);
            }
        }
    }
}
