using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Stubbery.IntegrationTests
{
    public class ApiStubTest
    {
        public class Get
        {
            private readonly HttpClient httpClient = new HttpClient();

            [Fact]
            public async Task Get_CalledSetupRoute_SetupResponseReturned()
            {
                using (var sut = new ApiStub())
                {
                    sut.Get(
                        "/testget",
                        req => "testresponse");

                    sut.Start();

                    var result = await httpClient.GetAsync(
                        new UriBuilder(new Uri(sut.Address)) { Path = "/testget" }.Uri);

                    Assert.Equal(HttpStatusCode.OK, result.StatusCode);

                    var resultString = await result.Content.ReadAsStringAsync();

                    Assert.Equal("testresponse", resultString);
                }
            }

            [Fact]
            public async Task Get_CalledDifferentRoute_NotFoundReturned()
            {
                using (var sut = new ApiStub())
                {
                    sut.Get(
                        "/testget",
                        req => "testresponse");

                    sut.Start();

                    var result = await httpClient.GetAsync(
                        new UriBuilder(new Uri(sut.Address)) { Path = "/differentroute" }.Uri);

                    Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
                }
            }

            [Fact]
            public async Task Get_CalledDifferentMethod_NotFoundReturned()
            {
                using (var sut = new ApiStub())
                {
                    sut.Get(
                        "/testget",
                        req => "testresponse");

                    sut.Start();

                    var result = await httpClient.DeleteAsync(
                        new UriBuilder(new Uri(sut.Address)) { Path = "/testget" }.Uri);

                    Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
                }
            }

            [Fact]
            public async Task Get_CalledSetupRoute_RouteDataAvailable()
            {
                using (var sut = new ApiStub())
                {
                    sut.Get(
                        "/testget/{myArg}",
                        req => "testresponse");

                    sut.Start();

                    var result = await httpClient.GetAsync(
                        new UriBuilder(new Uri(sut.Address)) { Path = "/testget/alma" }.Uri);

                    Assert.Equal(HttpStatusCode.OK, result.StatusCode);

                    var resultString = await result.Content.ReadAsStringAsync();

                    Assert.Equal("testresponse", resultString);
                }
            }
        }

        public class Delete
        {
            private readonly HttpClient httpClient = new HttpClient();

            [Fact]
            public async Task Delete_CalledSetupRoute_SetupResponseReturned()
            {
                using (var sut = new ApiStub())
                {
                    sut.Delete(
                        "/testdelete",
                        req => "testresponse");

                    sut.Start();

                    var result = await httpClient.DeleteAsync(
                        new UriBuilder(new Uri(sut.Address)) { Path = "/testdelete" }.Uri);

                    Assert.Equal(HttpStatusCode.OK, result.StatusCode);

                    var resultString = await result.Content.ReadAsStringAsync();

                    Assert.Equal("testresponse", resultString);
                }
            }

            [Fact]
            public async Task Delete_CalledDifferentRoute_NotFoundReturned()
            {
                using (var sut = new ApiStub())
                {
                    sut.Delete(
                        "/testdelete",
                        req => "testresponse");

                    sut.Start();

                    var result = await httpClient.DeleteAsync(
                        new UriBuilder(new Uri(sut.Address)) { Path = "/differentroute" }.Uri);

                    Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
                }
            }

            [Fact]
            public async Task Delete_CalledDifferentMethod_NotFoundReturned()
            {
                using (var sut = new ApiStub())
                {
                    sut.Delete(
                        "/testdelete",
                        req => "testresponse");

                    sut.Start();

                    var result = await httpClient.GetAsync(
                        new UriBuilder(new Uri(sut.Address)) { Path = "/testdelete" }.Uri);

                    Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
                }
            }
        }

        public class Post
        {
            private readonly HttpClient httpClient = new HttpClient();

            [Fact]
            public async Task Post_CalledSetupRoute_SetupResponseReturned()
            {
                using (var sut = new ApiStub())
                {
                    sut.Post(
                        "/testpost",
                        req => "testresponse");

                    sut.Start();

                    var result = await httpClient.PostAsync(
                        new UriBuilder(new Uri(sut.Address)) { Path = "/testpost" }.Uri,
                        new StringContent(""));

                    Assert.Equal(HttpStatusCode.OK, result.StatusCode);

                    var resultString = await result.Content.ReadAsStringAsync();

                    Assert.Equal("testresponse", resultString);
                }
            }

            [Fact]
            public async Task Post_CalledDifferentRoute_NotFoundReturned()
            {
                using (var sut = new ApiStub())
                {
                    sut.Post(
                        "/testpost",
                        req => "testresponse");

                    sut.Start();

                    var result = await httpClient.PostAsync(
                        new UriBuilder(new Uri(sut.Address)) { Path = "/differentroute" }.Uri,
                        new StringContent(""));

                    Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
                }
            }

            [Fact]
            public async Task Post_CalledDifferentMethod_NotFoundReturned()
            {
                using (var sut = new ApiStub())
                {
                    sut.Post(
                        "/testpost",
                        req => "testresponse");

                    sut.Start();

                    var result = await httpClient.PutAsync(
                        new UriBuilder(new Uri(sut.Address)) { Path = "/testpost" }.Uri,
                        new StringContent(""));

                    Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
                }
            }
        }

        public class Put
        {
            private readonly HttpClient httpClient = new HttpClient();

            [Fact]
            public async Task Put_CalledSetupRoute_SetupResponseReturned()
            {
                using (var sut = new ApiStub())
                {
                    sut.Put(
                        "/testput",
                        req => "testresponse");

                    sut.Start();

                    var result = await httpClient.PutAsync(
                        new UriBuilder(new Uri(sut.Address)) { Path = "/testput" }.Uri,
                        new StringContent(""));

                    Assert.Equal(HttpStatusCode.OK, result.StatusCode);

                    var resultString = await result.Content.ReadAsStringAsync();

                    Assert.Equal("testresponse", resultString);
                }
            }

            [Fact]
            public async Task Put_CalledDifferentRoute_NotFoundReturned()
            {
                using (var sut = new ApiStub())
                {
                    sut.Put(
                        "/testput",
                        req => "testresponse");

                    sut.Start();

                    var result = await httpClient.PutAsync(
                        new UriBuilder(new Uri(sut.Address)) { Path = "/differentroute" }.Uri,
                        new StringContent(""));

                    Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
                }
            }

            [Fact]
            public async Task Put_CalledDifferentMethod_NotFoundReturned()
            {
                using (var sut = new ApiStub())
                {
                    sut.Put(
                        "/testput",
                        req => "testresponse");

                    sut.Start();

                    var result = await httpClient.PostAsync(
                        new UriBuilder(new Uri(sut.Address)) { Path = "/testput" }.Uri,
                        new StringContent(""));

                    Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
                }
            }
        }
    }
}