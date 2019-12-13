using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Stubbery.RequestMatching;

namespace Stubbery
{
    internal class ApiStubRequestHandler : IApiStubRequestHandler
    {
        private readonly ICollection<Setup> configuredEndpoints;

        public ApiStubRequestHandler(ICollection<Setup> configuredEndpoints)
        {
            this.configuredEndpoints = configuredEndpoints;
        }

        public async Task HandleAsync(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

            foreach (var endpoint in configuredEndpoints)
            {
                if (await endpoint.IsMatch(httpContext))
                {
                    await endpoint.SendResponseAsync(httpContext);
                    break;
                }
            }

            await httpContext.Request.Body.ReadAsStringAsync();
        }
    }
}