using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Stubbery.IntegrationTests
{
    public class BasicTest
    {
        public class Get
        {
            private readonly HttpClient httpClient = new HttpClient();

            [Fact]
            public async Task Get_CalledSetupRoute_SetupResponseReturned()
            {
                using var sut = new ApiStub();

                sut.Get("/testget", (req, args) => "testresponse");

                sut.Start();

                var result = await httpClient.GetAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/testget" }.Uri);

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);

                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal("testresponse", resultString);
            }

            [Fact]
            public async Task Get_CalledDifferentRoute_NotFoundReturned()
            {
                using var sut = new ApiStub();

                sut.Get("/testget", (req, args) => "testresponse");

                sut.Start();

                var result = await httpClient.GetAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/differentroute" }.Uri);

                Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            }

            [Fact]
            public async Task Get_CalledDifferentMethod_NotFoundReturned()
            {
                using var sut = new ApiStub();

                sut.Get("/testget", (req, args) => "testresponse");

                sut.Start();

                var result = await httpClient.DeleteAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/testget" }.Uri);

                Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            }


            [Fact]
            public async Task Get_SetupAfterStart_Works()
            {
                using var sut = new ApiStub();

                sut.Start();

                sut.Get("/testget", (req, args) => "testresponse");

                var result = await httpClient.GetAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/testget" }.Uri);

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);

                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal("testresponse", resultString);
            }

            [Fact]
            public async Task GetWithEnsureStarted_CalledSetupRoute_SetupResponseReturned()
            {
                using var sut = new ApiStub();

                sut.Get("/testget", (req, args) => "testresponse");

                sut.EnsureStarted();

                var result = await httpClient.GetAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/testget" }.Uri);

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);

                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal("testresponse", resultString);
            }
        }

        public class Delete
        {
            private readonly HttpClient httpClient = new HttpClient();

            [Fact]
            public async Task Delete_CalledSetupRoute_SetupResponseReturned()
            {
                using var sut = new ApiStub();

                sut.Delete("/testdelete", (req, args) => "testresponse");

                sut.Start();

                var result = await httpClient.DeleteAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/testdelete" }.Uri);

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);

                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal("testresponse", resultString);
            }

            [Fact]
            public async Task Delete_CalledDifferentRoute_NotFoundReturned()
            {
                using var sut = new ApiStub();

                sut.Delete("/testdelete", (req, args) => "testresponse");

                sut.Start();

                var result = await httpClient.DeleteAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/differentroute" }.Uri);

                Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            }

            [Fact]
            public async Task Delete_CalledDifferentMethod_NotFoundReturned()
            {
                using var sut = new ApiStub();

                sut.Delete("/testdelete", (req, args) => "testresponse");

                sut.Start();

                var result = await httpClient.GetAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/testdelete" }.Uri);

                Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            }

            [Fact]
            public async Task Delete_CalledSetupRouteWithFluentMethod_SetupResponseReturned()
            {
                using var sut = new ApiStub();

                sut.Delete().IfRoute("/testdelete").Response((req, args) => "testresponse");

                sut.Start();

                var result = await httpClient.DeleteAsync(new UriBuilder(new Uri(sut.Address)) { Path = "/testdelete" }.Uri);

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);

                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal("testresponse", resultString);
            }
        }

        public class Post
        {
            private readonly HttpClient httpClient = new HttpClient();

            [Fact]
            public async Task Post_CalledSetupRoute_SetupResponseReturned()
            {
                using var sut = new ApiStub();

                sut.Post("/testpost", (req, args) => "testresponse");

                sut.Start();

                var result = await httpClient.PostAsync(
                    new UriBuilder(new Uri(sut.Address)) { Path = "/testpost" }.Uri,
                    new StringContent(""));

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);

                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal("testresponse", resultString);
            }

            [Fact]
            public async Task Post_CalledDifferentRoute_NotFoundReturned()
            {
                using var sut = new ApiStub();

                sut.Post("/testpost", (req, args) => "testresponse");

                sut.Start();

                var result = await httpClient.PostAsync(
                    new UriBuilder(new Uri(sut.Address)) { Path = "/differentroute" }.Uri,
                    new StringContent(""));

                Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            }

            [Fact]
            public async Task Post_CalledDifferentMethod_NotFoundReturned()
            {
                using var sut = new ApiStub();

                sut.Post("/testpost", (req, args) => "testresponse");

                sut.Start();

                var result = await httpClient.PutAsync(
                    new UriBuilder(new Uri(sut.Address)) { Path = "/testpost" }.Uri,
                    new StringContent(""));

                Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            }

            [Fact]
            public async Task Post_CalledSetupRouteWithFluentMethod_SetupResponseReturned()
            {
                using var sut = new ApiStub();

                sut.Post()
                    .IfRoute("/testpost")
                    .Response((req, args) => "testresponse");

                sut.Start();

                var result = await httpClient.PostAsync(
                    new UriBuilder(new Uri(sut.Address)) {Path = "/testpost"}.Uri,
                    new StringContent(""));

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);

                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal("testresponse", resultString);
            }
        }

        public class Put
        {
            private readonly HttpClient httpClient = new HttpClient();

            [Fact]
            public async Task Put_CalledSetupRoute_SetupResponseReturned()
            {
                using var sut = new ApiStub();

                sut.Put("/testput", (req, args) => "testresponse");

                sut.Start();

                var result = await httpClient.PutAsync(
                    new UriBuilder(new Uri(sut.Address)) { Path = "/testput" }.Uri,
                    new StringContent(""));

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);

                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal("testresponse", resultString);
            }

            [Fact]
            public async Task Put_CalledDifferentRoute_NotFoundReturned()
            {
                using var sut = new ApiStub();

                sut.Put("/testput", (req, args) => "testresponse");

                sut.Start();

                var result = await httpClient.PutAsync(
                    new UriBuilder(new Uri(sut.Address)) { Path = "/differentroute" }.Uri,
                    new StringContent(""));

                Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            }

            [Fact]
            public async Task Put_CalledDifferentMethod_NotFoundReturned()
            {
                using var sut = new ApiStub();

                sut.Put("/testput", (req, args) => "testresponse");

                sut.Start();

                var result = await httpClient.PostAsync(
                    new UriBuilder(new Uri(sut.Address)) { Path = "/testput" }.Uri,
                    new StringContent(""));

                Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            }

            [Fact]
            public async Task Put_CalledSetupRouteWithFluentMethod_SetupResponseReturned()
            {
                using var sut = new ApiStub();

                sut.Put()
                    .IfRoute("/testput")
                    .Response((req, args) => "testresponse");

                sut.Start();

                var result = await httpClient.PutAsync(
                    new UriBuilder(new Uri(sut.Address)) { Path = "/testput" }.Uri,
                    new StringContent(""));

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);

                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal("testresponse", resultString);
            }
        }

        public class OtherVerbs
        {
            private readonly HttpClient httpClient = new HttpClient();

            [Fact]
            public async Task Get_CalledSetupOptions_ResponseReturned()
            {
                using var sut = new ApiStub();

                sut.Request(HttpMethod.Options)
                    .IfRoute("/testoptions")
                    .Response((req, args) => "testresponse");

                sut.Start();

                var result = await httpClient.SendAsync(
                    new HttpRequestMessage
                    {
                        RequestUri = new UriBuilder(new Uri(sut.Address)) { Path = "/testoptions" }.Uri,
                        Method = HttpMethod.Options
                    });

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);

                var resultString = await result.Content.ReadAsStringAsync();

                Assert.Equal("testresponse", resultString);
            }
        }
    }
}