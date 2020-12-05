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
            this.startup = startup ?? throw new ArgumentNullException(nameof(startup));
        }

        public string StartHosting(int? port)
        {
            // this one can cause port collission
            //var hostingPort = port ?? PickFreeTcpPort();

            // this works
            var hostingPort = port ?? 0;
            var hostBuilder = new WebHostBuilder()
                .UseKestrel()
                .UseUrls($"http://127.0.0.1:{hostingPort}/")
                .ConfigureServices(startup.ConfigureServices)
                .Configure(startup.Configure);

            webHost = hostBuilder.Build();
            webHost.Start();

            appLifetime = webHost.Services.GetRequiredService<IApplicationLifetime>();

            var serverAddresses = webHost.ServerFeatures.Get<IServerAddressesFeature>();

            return serverAddresses.Addresses.First(it => Uri.IsWellFormedUriString(it, UriKind.Absolute));
        }

        public void Stop()
        {
            if (appLifetime != null)
            {
                appLifetime.StopApplication();
                appLifetime.ApplicationStopping.WaitHandle.WaitOne();
            }

            webHost?.Dispose();
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