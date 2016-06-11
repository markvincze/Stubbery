using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
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

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
        }

        public void Configure(IApplicationBuilder app)
        {
            try
            {
                RouteBuilder builder = new RouteBuilder(app);

                foreach (var endpoint in configuredEndpoints)
                {
                    builder.MapVerb(
                        endpoint.Method.Method,
                        endpoint.Route.TrimStart('/'),
                        async context =>
                        {
                            var arguments = new RequestArguments(
                                new DynamicValues(context.GetRouteData().Values), 
                                new DynamicValues(context.Request.Query), 
                                context.Request.Body);

                            await context.Response.WriteAsync((string)endpoint.Response(context.Request, arguments));
                        });
                }

                app.UseRouter(builder.Build());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}