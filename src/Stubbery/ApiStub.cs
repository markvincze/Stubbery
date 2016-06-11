using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace Stubbery
{
    public class ApiStub : IDisposable
    {
        private ApiStubState state = ApiStubState.Stopped;

        private readonly ICollection<EndpointStubConfig> configuredEndpoints = new List<EndpointStubConfig>();

        private IApplicationLifetime appLifetime;

        private IWebHost webHost;

        public string Address
        {
            get
            {
                if (state == ApiStubState.Stopped)
                {
                    throw new InvalidOperationException($"The api stub is not started yet. It can be started by calling the {nameof(Start)} method.");
                }

                var serverAddresses = webHost.ServerFeatures.Get<IServerAddressesFeature>();

                return serverAddresses.Addresses.First();
            }
        }

        public void Start()
        {
            if (state == ApiStubState.Started)
            {
                throw new InvalidOperationException("The api stub is already started.");
            }

            var startup = new ApiStubWebAppStartup(configuredEndpoints);

            Run(startup);

            state = ApiStubState.Started;
        }

        private void Run(IApiStartup startup)
        {
            if (startup == null)
            {
                throw new ArgumentNullException(nameof(startup));
            }

            var hostBuilder = new WebHostBuilder()
                .UseKestrel()
                .UseUrls($"http://localhost:{PickFreeTcpPort()}/")
                .ConfigureServices(startup.ConfigureServices)
                .Configure(startup.Configure);

            webHost = hostBuilder.Build();
            webHost.Start();

            appLifetime = webHost.Services.GetRequiredService<IApplicationLifetime>();
        }

        private int PickFreeTcpPort()
        {
            var l = new TcpListener(IPAddress.Loopback, 0);

            l.Start();
            var port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();

            return port;
        }

        public void Stop()
        {
            if (state == ApiStubState.Stopped)
            {
                throw new InvalidOperationException("The api stub is not started.");
            }

            appLifetime.StopApplication();
            appLifetime.ApplicationStopping.WaitHandle.WaitOne();
            webHost.Dispose();

            state = ApiStubState.Stopped;

        }

        public void Dispose()
        {
            Stop();
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