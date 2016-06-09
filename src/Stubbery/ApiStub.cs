using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace Stubbery
{
    public class ApiStub : IDisposable
    {
        private readonly ICollection<EndpointStubConfig> configuredEndpoints = new List<EndpointStubConfig>();

        public void Get(string route, Func<HttpRequest, dynamic> func)
        {
            configuredEndpoints.Add(new EndpointStubConfig(HttpMethod.Get, route, func));
        }

        public void Post(string route, Func<HttpRequest, dynamic> func)
        {
            configuredEndpoints.Add(new EndpointStubConfig(HttpMethod.Post, route, func));
        }

        public void Put(string route, Func<HttpRequest, dynamic> func)
        {
            configuredEndpoints.Add(new EndpointStubConfig(HttpMethod.Put, route, func));
        }

        public void Delete(string route, Func<HttpRequest, dynamic> func)
        {
            configuredEndpoints.Add(new EndpointStubConfig(HttpMethod.Delete, route, func));
        }

        public string Address
        {
            get
            {
                var serverAddresses = webHost.ServerFeatures.Get<IServerAddressesFeature>();

                return serverAddresses.Addresses.First();
            }
        }

        public void Start()
        {
            var startup = new ApiStubWebAppStartup(configuredEndpoints);

            Run(startup);
        }

        private IApplicationLifetime appLifetime;
        private IWebHost webHost;

        public void Run(IApiStartup startup)
        {
            if (startup == null)
            {
                throw new ArgumentNullException(nameof(startup));
            }

            var hostBuilder = new WebHostBuilder()
                .UseKestrel()
                .UseUrls($"http://localhost:{PickFreeTcpPort()}/")
                .Configure(startup.Configure);

            webHost = hostBuilder.Build();
            webHost.Start();

            appLifetime = webHost.Services.GetRequiredService<IApplicationLifetime>();
        }

        public void Stop()
        {
            appLifetime.StopApplication();
            appLifetime.ApplicationStopping.WaitHandle.WaitOne();
            webHost.Dispose();
        }

        public void Dispose()
        {
            this.Stop();
        }

        private int PickFreeTcpPort()
        {
            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }
    }
}