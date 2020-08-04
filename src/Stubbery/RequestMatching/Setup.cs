using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Stubbery.RequestMatching.Preconditions;

namespace Stubbery.RequestMatching
{
    internal class Setup : ISetup
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
                [ConditionGroup.Route] = new List<IPrecondition>(),
                [ConditionGroup.QueryArg] = new List<IPrecondition>(),
                [ConditionGroup.ContentType] = new List<IPrecondition>(),
                [ConditionGroup.Accept] = new List<IPrecondition>(),
                [ConditionGroup.Body] = new List<IPrecondition>()
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
            orConditions[ConditionGroup.ContentType].Add(new ContentTypeCondition(c => c != null && c.Contains(contentType)));

            return this;
        }

        public ISetup IfContentType(Func<string, bool> check)
        {
            orConditions[ConditionGroup.ContentType].Add(new ContentTypeCondition(check));

            return this;
        }

        public ISetup IfAccept(string accept)
        {
            orConditions[ConditionGroup.Accept].Add(new AcceptCondition(a => a == accept));

            return this;
        }

        public ISetup IfAccept(Func<string, bool> check)
        {
            orConditions[ConditionGroup.Accept].Add(new AcceptCondition(check));

            return this;
        }

        public ISetup IfRoute(string routeTemplate)
        {
            orConditions[ConditionGroup.Route].Add(new RouteCondition(routeTemplate));

            return this;
        }

        public ISetup IfQueryArg(string argName, string argValue)
        {
            orConditions[ConditionGroup.QueryArg].Add(new QueryArgCondition(argName, argValue));

            return this;
        }

        public ISetup IfBody(Func<string, bool> check)
        {
            orConditions[ConditionGroup.Body].Add(new BodyCondition(check));

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
            setupResponse.StatusCodeProvider = (req, args) => statusCode;

            return this;
        }

        public ISetup StatusCode(Func<HttpRequest, RequestArguments, int> statusCodeProvider)
        {
            setupResponse.StatusCodeProvider = statusCodeProvider;

            return this;
        }

        public ISetup Headers((string, string)[] headers)
        {
            setupResponse.HeaderProviders.Add((req, args) => headers);

            return this;
        }

        public ISetup Header(string header, string value)
        {
            setupResponse.HeaderProviders.Add((req, args) => new[] { (header, value) });

            return this;
        }

        public ISetup Header(Func<HttpRequest, RequestArguments, (string, string)> headerProvider)
        {
            setupResponse.HeaderProviders.Add((req, args) => new[] { headerProvider(req, args) });

            return this;
        }

        public ISetup Headers(Func<HttpRequest, RequestArguments, (string, string)[]> headersProvider)
        {
            setupResponse.HeaderProviders.Add(headersProvider);

            return this;
        }

        public async Task<bool> IsMatch(HttpContext httpContext)
        {
            foreach (var orGroup in orConditions.Where(g => g.Value.Count > 0))
            {
                var orGroupMatched = false;
                foreach (var condition in orGroup.Value)
                {
                    if (await condition.Match(httpContext))
                    {
                        orGroupMatched = true;
                        break;
                    }
                }

                if (!orGroupMatched)
                {
                    return false;
                }
            }

            foreach (var condition in andConditions)
            {
                if (!await condition.Match(httpContext))
                {
                    return false;
                }
            }

            return true;
        }

        public async Task SendResponseAsync(HttpContext httpContext)
        {
            await setupResponse.SendResponseAsync(httpContext);
        }
    }
}
