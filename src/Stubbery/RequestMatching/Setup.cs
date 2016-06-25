using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Stubbery.RequestMatching.Preconditions;

namespace Stubbery.RequestMatching
{
    public class Setup : ISetup
    {
        private readonly List<IPrecondition> andConditions;

        private readonly Dictionary<ConditionGroup, List<IPrecondition>> orConditions;

        private readonly SetupResponseParameters setupResponse;

        public Setup(HttpMethod[] methods)
        {
            andConditions = new List<IPrecondition>();

            orConditions = new Dictionary<ConditionGroup, List<IPrecondition>>
            {
                [ConditionGroup.Method] = new List<IPrecondition>(),
                [ConditionGroup.Route] = new List<IPrecondition>()
            };

            orConditions[ConditionGroup.Method].AddRange(methods.Select(m => new MethodCondition(m.Method)));

            setupResponse = new SetupResponseParameters();
        }

        public ISetup IfHeader(string header, string value)
        {
            andConditions.Add(new HeaderCondition(h => h.ContainsKey(header) && h[header].ToString() == value));

            return this;
        }

        public ISetup IfHeaders(Func<IHeaderDictionary, bool> check)
        {
            andConditions.Add(new HeaderCondition(check));

            return this;
        }

        public ISetup IfContentType(string contentType)
        {
            andConditions.Add(new ContentTypeCondition(c => c == contentType));

            return this;
        }

        public ISetup IfContentType(Func<string, bool> check)
        {
            andConditions.Add(new ContentTypeCondition(check));

            return this;
        }

        public ISetup IfAccept(string accept)
        {
            andConditions.Add(new AcceptCondition(a => a == accept));

            return this;
        }

        public ISetup IfAccept(Func<string, bool> check)
        {
            andConditions.Add(new AcceptCondition(check));

            return this;
        }

        public ISetup Route(string routeTemplate)
        {
            orConditions[ConditionGroup.Route].Add(new RouteCondition(routeTemplate));

            return this;
        }

        public ISetup Response(CreateStubResponse responder)
        {
            if (responder == null)
            {
                throw new ArgumentNullException(nameof(responder));
            }

            if (setupResponse.Responder != null)
            {
                throw new InvalidOperationException("The response is already set. It cannot be set multiple times.");
            }

            setupResponse.Responder = responder;

            return this;
        }

        public ISetup StatusCode(int statusCode)
        {
            setupResponse.StatusCode = statusCode;

            return this;
        }

        public ISetup Headers(params KeyValuePair<string, string>[] headers)
        {
            setupResponse.Headers.AddRange(headers);

            return this;
        }

        public bool IsMatch(HttpContext httpContext)
        {
            return orConditions.All(
                       conditionGroup => conditionGroup.Value.Any(condition => condition.Match(httpContext))) &&
                   andConditions.All(condition => condition.Match(httpContext));
        }

        public async Task SendResponseAsync(HttpContext httpContext)
        {
            await setupResponse.SendResponseAsync(httpContext);
        }
    }
}