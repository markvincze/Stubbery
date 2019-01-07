using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Stubbery.RequestMatching
{
    internal class SetupResponseParameters
    {
        public int StatusCode { get; set; } = StatusCodes.Status200OK;

        public CreateStubResponse? Responder { get; set; }

        public List<KeyValuePair<string, string>> Headers { get; } = new List<KeyValuePair<string, string>>();

        public async Task SendResponseAsync(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = StatusCode;

            foreach (var header in Headers)
            {
                httpContext.Response.Headers[header.Key] = header.Value;
            }

            var routeValues = httpContext.GetRouteValues();

            var arguments = new RequestArguments(
                new DynamicValues(routeValues),
                new DynamicValues(httpContext.Request.Query),
                httpContext.Request.Body);

            var responseContent = Responder == null ? "" : (string)Responder(httpContext.Request, arguments);

            await httpContext.Response.WriteAsync(responseContent);
        }
    }
}