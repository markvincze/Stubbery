using System;
using System.Collections.Generic;
using System.Net.Http;
using Stubbery.RequestMatching;

namespace Stubbery
{
    public class ApiStub : IDisposable
    {
        private readonly ICollection<Setup> configuredEndpoints = new List<Setup>();

        private readonly ApiHost apiHost;

        private ApiStubState state = ApiStubState.Stopped;

        private string address;

        public ApiStub()
        {
            var startup = new ApiStubWebAppStartup(new ApiStubRequestHandler(configuredEndpoints));

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

        public ISetup Get()
        {
            return Request(HttpMethod.Get);
        }

        public ISetup Get(string route, CreateStubResponse responder)
        {
            return Request(HttpMethod.Get)
                .IfRoute(route)
                .Response(responder);
        }

        public ISetup Post(string route, CreateStubResponse responder)
        {
            return Request(HttpMethod.Post)
                .IfRoute(route)
                .Response(responder);
        }

        public ISetup Post()
        {
            return Request(HttpMethod.Post);
        }

        public ISetup Put(string route, CreateStubResponse responder)
        {
            return Request(HttpMethod.Put)
                .IfRoute(route)
                .Response(responder);
        }

        public ISetup Put()
        {
            return Request(HttpMethod.Put);
        }

        public ISetup Delete(string route, CreateStubResponse responder)
        {
            return Request(HttpMethod.Delete)
                .IfRoute(route)
                .Response(responder);
        }

        public ISetup Delete()
        {
            return Request(HttpMethod.Delete);
        }

        public ISetup Request(params HttpMethod[] methods)
        {
            var setup = new Setup(methods);

            configuredEndpoints.Add(setup);

            return setup;
        }
    }
}