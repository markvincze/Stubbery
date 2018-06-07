using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;

namespace Stubbery
{
    internal class ApiHost
    {
        private readonly IApiStartup startup;

        private IApplicationLifetime appLifetime;

        private IWebHost webHost;

        public ApiHost(IApiStartup startup)
        {
            if (startup == null)
            {
                throw new ArgumentNullException(nameof(startup));
            }

            this.startup = startup;
        }

        public string StartHosting(int? port)
        {
            var hostingPort = port ?? PickFreeTcpPort();
            var hostBuilder = new WebHostBuilder()
                .UseKestrel()
                .UseUrls($"http://localhost:{hostingPort}/")
                .ConfigureServices(startup.ConfigureServices)
                .Configure(startup.Configure);

            webHost = hostBuilder.Build();
            webHost.Start();

            appLifetime = webHost.Services.GetRequiredService<IApplicationLifetime>();

            var serverAddresses = webHost.ServerFeatures.Get<IServerAddressesFeature>();

            return serverAddresses.Addresses.First();
        }

        public void Stop()
        {
            if (appLifetime != null)
            {
                appLifetime.StopApplication();
                appLifetime.ApplicationStopping.WaitHandle.WaitOne();
            }

            if (webHost != null)
            {
                webHost.Dispose();
            }
        }

        private int PickFreeTcpPort()
        {
            var l = new TcpListener(IPAddress.Loopback, 0);

            l.Start();
            var port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();

            return port;
        }
    }
}