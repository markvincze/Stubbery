using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Stubbery
{
    internal class ApiStubRequestHandler : IApiStubRequestHandler
    {
        private readonly ICollection<EndpointStubConfig> configuredEndpoints;

        private readonly IRouteMatcher routeMatcher;

        public ApiStubRequestHandler(ICollection<EndpointStubConfig> configuredEndpoints, IRouteMatcher routeMatcher)
        {
            this.configuredEndpoints = configuredEndpoints;
            this.routeMatcher = routeMatcher;
        }

        public async Task HandleAsync(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

            foreach (var configuredEndpoint in configuredEndpoints)
            {
                if (httpContext.Request.Method != configuredEndpoint.Method.Method)
                {
                    continue;
                }

                var result = routeMatcher.Match(configuredEndpoint.Route, httpContext.Request.Path);

                if (result != null)
                {
                    httpContext.Response.StatusCode = StatusCodes.Status200OK;

                    var arguments = new RequestArguments(
                        new DynamicValues(result),
                        new DynamicValues(httpContext.Request.Query),
                        httpContext.Request.Body);

                    await httpContext.Response.WriteAsync((string)configuredEndpoint.Response(httpContext.Request, arguments));
                }
            }
        }
    }
}