using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Stubbery
{
    internal class ApiStubWebAppStartup : IApiStartup
    {
        private readonly ICollection<EndpointStubConfig> configuredEndpoints;

        public ApiStubWebAppStartup(ICollection<EndpointStubConfig> configuredEndpoints)
        {
            this.configuredEndpoints = configuredEndpoints;
        }

        public void Configure(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                var match = configuredEndpoints.FirstOrDefault(config =>
                {
                    if (context.Request.Method != config.Method.ToString())
                    {
                        return false;
                    }

                    Regex regex = new Regex(config.Route);
                    return regex.IsMatch(context.Request.Path.Value);
                });

                if (match != null)
                {
                    await context.Response.WriteAsync((string) match.Response(context.Request));
                }
                else
                {
                    context.Response.StatusCode = 404;
                }
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
        }
    }

    public interface IApiStartup
    {
        void Configure(IApplicationBuilder app);

        void ConfigureServices(IServiceCollection services);
    }
}