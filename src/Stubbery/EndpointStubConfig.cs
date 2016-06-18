using System.Net.Http;

namespace Stubbery
{
    internal class EndpointStubConfig
    {
        public HttpMethod Method { get; }

        public string Route { get; }

        public CreateStubResponse Response { get; }

        public EndpointStubConfig(HttpMethod method, string route, CreateStubResponse responder)
        {
            Method = method;
            Route = route.TrimStart('/');
            Response = responder;
        }
    }
}