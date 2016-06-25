using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Stubbery
{
    public class ApiStub : IDisposable
    {
        private readonly ICollection<EndpointStubConfig> configuredEndpoints = new List<EndpointStubConfig>();

        private readonly ApiHost apiHost;

        private ApiStubState state = ApiStubState.Stopped;

        private string address;

        public ApiStub()
        {
            var startup = new ApiStubWebAppStartup(new ApiStubRequestHandler(configuredEndpoints, new RouteMatcher()));

            apiHost = new ApiHost(startup);
        }

        public string Address
        {
            get
            {
                if (state == ApiStubState.Stopped)
                {
                    throw new InvalidOperationException($"The api stub is not started yet. It can be started by calling the {nameof(Start)} method.");
                }

                return address;
            }
        }

        public void Start()
        {
            if (state == ApiStubState.Started)
            {
                throw new InvalidOperationException("The api stub is already started.");
            }

            address = apiHost.StartHosting();

            state = ApiStubState.Started;
        }

        public void Dispose()
        {
            apiHost.Stop();

            state = ApiStubState.Stopped;
        }

        public void Get(string route, CreateStubResponse responder)
        {
            Setup(HttpMethod.Get, route, responder);
        }

        public void Post(string route, CreateStubResponse responder)
        {
            Setup(HttpMethod.Post, route, responder);
        }

        public void Put(string route, CreateStubResponse responder)
        {
            Setup(HttpMethod.Put, route, responder);
        }

        public void Delete(string route, CreateStubResponse responder)
        {
            Setup(HttpMethod.Delete, route, responder);
        }

        public void Setup(HttpMethod method, string route, CreateStubResponse responder)
        {
            configuredEndpoints.Add(new EndpointStubConfig(method, route, responder));
        }
    }
}