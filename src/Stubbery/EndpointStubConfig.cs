using System;
using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace Stubbery
{
    internal class EndpointStubConfig
    {
        public HttpMethod Method { get; }

        public string Route { get; }

        public Func<HttpRequest, dynamic> Response { get; }

        public EndpointStubConfig(HttpMethod method, string route, Func<HttpRequest, dynamic> response)
        {
            Method = method;
            Route = route;
            Response = response;
        }
    }
}