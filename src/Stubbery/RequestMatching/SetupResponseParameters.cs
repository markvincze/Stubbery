using System.Buffers;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Internal;

namespace Stubbery.RequestMatching
{
    internal class SetupResponseParameters
    {
        private static readonly MemoryPoolHttpResponseStreamWriterFactory responseStreamWriterFactory = new MemoryPoolHttpResponseStreamWriterFactory(ArrayPool<byte>.Shared, ArrayPool<char>.Shared);

        public int StatusCode { get; set; } = StatusCodes.Status200OK;

        public CreateStubResponse Responder { get; set; }

        public List<KeyValuePair<string, string>> Headers { get; } = new List<KeyValuePair<string, string>>();

        public async Task SendResponseAsync(HttpContext httpContext, OutputFormatter defaultOutputFormatter)
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

            var responseObject = Responder(httpContext.Request, arguments);

            if (responseObject is string str)
            {
                await httpContext.Response.WriteAsync(str);
            }
            else
            {
                var c = new OutputFormatterWriteContext(
                    httpContext,
                    responseStreamWriterFactory.CreateWriter,
                    responseObject.GetType(),
                    responseObject);

                if (defaultOutputFormatter is TextOutputFormatter textOutputFormatter)
                {
                    await textOutputFormatter.WriteResponseBodyAsync(c, Encoding.UTF8);
                }
                else
                {
                    await defaultOutputFormatter.WriteResponseBodyAsync(c);
                }
            }
        }
    }
}