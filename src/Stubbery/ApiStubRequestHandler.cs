using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Stubbery.RequestMatching;
using System.IO;

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
            using (var bodyStream = new MemoryStream())
            {
                // Ensure the request body is fully read to avoid hanging connections on linux
                await httpContext.Request.Body.CopyToAsync(bodyStream);
                httpContext.Request.Body = bodyStream;
                bodyStream.Position = 0;

                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

                var firstMatch = configuredEndpoints.FirstOrDefault(e => e.IsMatch(httpContext));

                if (firstMatch != null)
                {
                    await firstMatch.SendResponseAsync(httpContext);
                }
            }
        }
    }
}