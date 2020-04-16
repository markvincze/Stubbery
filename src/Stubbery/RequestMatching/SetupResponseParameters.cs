using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;

namespace Stubbery.RequestMatching
{
    internal class SetupResponseParameters
    {
        public Func<HttpRequest, RequestArguments, int> StatusCodeProvider { get; set; } = (req, args) => StatusCodes.Status200OK;
        public ICollection<Func<HttpRequest, RequestArguments, KeyValuePair<string, string>[]>> HeaderProviders { get; set; } = new List<Func<HttpRequest, RequestArguments, KeyValuePair<string, string>[]>>();

        public CreateStubResponse Responder { get; set; }

        public async Task SendResponseAsync(HttpContext httpContext)
        {
            var routeValues = httpContext.GetRouteValues();

            var arguments = new RequestArguments(
                new DynamicValues(routeValues),
                new DynamicValues(httpContext.Request.Query),
                httpContext.Request.Body);

            httpContext.Response.StatusCode = StatusCodeProvider(httpContext.Request, arguments);

            foreach (var headerProvider in HeaderProviders) {
                var headers = headerProvider(httpContext.Request, arguments);
                foreach (var header in headers) {
                    httpContext.Response.Headers[header.Key] = header.Value;
                }
            }

            var response = Responder(httpContext.Request, arguments);

            if (response is string stringResponse)
            {
                await httpContext.Response.WriteAsync(stringResponse);
                return;
            }

            if (response is IActionResult actionResultResponse)
            {
                ActionContext context = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
                await (actionResultResponse).ExecuteResultAsync(context);
                return;
            }

            if (response is object objectResponse)
            {
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(objectResponse));
                return;
            }

            await httpContext.Response.WriteAsync((string)response);
            return;
        }
    }
}
