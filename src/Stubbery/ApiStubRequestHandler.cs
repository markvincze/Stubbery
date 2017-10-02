using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
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

        public async Task HandleAsync(HttpContext httpContext, OutputFormatter defaultOutputFormatter)
        {
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

            var firstMatch = configuredEndpoints.FirstOrDefault(e => e.IsMatch(httpContext));

            if (firstMatch != null)
            {
                await firstMatch.SendResponseAsync(httpContext, defaultOutputFormatter);
            }
        }
    }
}